using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace WSUSAdminAssistant
{
    ////////////////////////////////////////////////////////////////////////////////////
    /// Define some easily read task conditions
    //
    public enum TaskStatus
    {
        New,
        Queued,
        Pending,
        Running,
        TimedOut,
        Complete,
        Cancelled,
        Failed
    };

    ////////////////////////////////////////////////////////////////////////////////////
    // Class describing a single task
    //
    public class Task : INotifyPropertyChanged
    {
        public Task()
        {
            // All tasks are new until either set to queued or a dependent task ID is supplied
            Status = TaskStatus.New;

            // Default timeout is 30 seconds
            TimeoutInterval = new TimeSpan(0, 0, 30);
        }

        private int _TaskID;
        private TaskStatus _Status;
        private List<string> _Commands = new List<string>();
        private string _Output;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(property));
        }

        public int TaskID
        {
            get { return _TaskID; }
            set { _TaskID = value; OnPropertyChanged("TaskID"); }
        }

        private int _DependsOnTaskID;

        public int DependsOnTaskID
        {
            get { return _DependsOnTaskID; }

            set 
            {
                _DependsOnTaskID = value;
                
                // If a task was new, it can be marked as pending now
                if (Status == TaskStatus.New) Status = TaskStatus.Pending;
            }
        }

        /// <summary>
        /// Sets the task that this task is dependant on
        /// </summary>
        public Task DependsOnTask
        {
            set { DependsOnTaskID = value.TaskID; }
        }

        public TaskStatus Status
        {
            get { return _Status; }
            set { _Status = value; OnPropertyChanged("Status"); OnPropertyChanged("CurrentStatus"); }
        }

        public string CurrentStatus { get { return Status.ToString(); } }

        public DateTime TaskStarted;
        public DateTime TaskFinished;
        public TimeSpan TimeoutInterval;

        public string Computer;
        public IPAddress IP;
        public string IPAddress { get { return IP.ToString(); } }

        public clsConfig.SecurityCredential Credentials;

        public string Command { get { return string.Join(Environment.NewLine, _Commands.ToArray()); } }

        public List<string> Commands
        {
            get { return _Commands; }
        }

        public void AddCommand(string command)
        {
            _Commands.Add(command);
            OnPropertyChanged("Command");
        }

        public string Output
        {
            get { return _Output; }
            set { _Output = value; OnPropertyChanged("Output"); }
        }

        /// <summary>
        /// Mark a task as ready for execution
        /// </summary>
        public void Ready()
        {
            Status = TaskStatus.Queued;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////
    // Class that contains and processes a list of tasks
    //
    public class TaskCollection
    {
        // Class initialisation
        private clsConfig cfg;

        public TaskCollection(clsConfig cfgobject)
        {
            cfg = cfgobject;

            // Kick off the background worker
            wrkTaskManager.DoWork += wrkTaskManager_DoWork;
            wrkTaskManager.RunWorkerCompleted += wrkTaskManager_RunWorkerCompleted;
            wrkTaskManager.RunWorkerAsync(SynchronizationContext.Current);
        }

        void wrkTaskManager_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Debug.WriteLine("Task Worker exited: " + e.Error.Message);

            // Since it wasn't supposed to exit, restart it
            wrkTaskManager.RunWorkerAsync(SynchronizationContext.Current);
        }

        // Private class properties and methods
        private BackgroundWorker wrkTaskManager = new BackgroundWorker();
        private int _taskidcounter = 0;

        // Public properties and methods
        public BindingList<Task> Tasks = new BindingList<Task>();

        public Task AddTask(IPAddress ip, string computername, clsConfig.SecurityCredential credentials, string command)
        {
            // Has a valid PSExec path been supplied?
            if (cfg.PSExecPath == "")
                // No - throw exception
                throw new ConfigurationException("No valid path to PSExec has been set in Preferences.  Please set a path to PSExec in Helper Preferences.", "PSExec path not valid");

            // Did we get valid credentials?
            if (credentials == null)
            {
                // No - do we run with the current credentials?
                if (!cfg.RunWithLocalCreds)
                    // No - throw exception
                    throw new ConfigurationException("No credentials found for IP address " + ip.ToString() + " and running with local credentials disabled.  To run with local credentials, check \"Supply current credentials if no other security credentials found for IP address\" in General Preferences.",
                        "No local credentials found for remote PC");
            }

            // Build the task details
            Task t = new Task();

            t.TaskID = ++_taskidcounter;
            t.IP = ip;
            t.Credentials = credentials;
            t.Computer = computername;
            t.AddCommand(command);

            // Add the task to the list and return it
            this.Tasks.Add(t);
            return t;
        }

        // Events that can be triggered by task processing
        public delegate void TaskRunningEventHandler(object sender, EventArgs e);
        public event TaskRunningEventHandler TaskRun;

        protected virtual void OnRunning(Task task, EventArgs e)
        {
            if (TaskRun != null)
                TaskRun(task, e);
        }

        public void RemoveTask(int index) { this.Tasks.RemoveAt(index); }

        public void UpdateStatus(int index, TaskStatus status) { this.Tasks[index].Status = status; }

        public void UpdateOutput(int index, string output) { this.Tasks[index].Output = output; }

        // Background worker that processes tasks as they become available
        private string output;

        private void wrkTaskManager_DoWork(object sender, DoWorkEventArgs e)
        {
            // Make a note of the synchronization context
            SynchronizationContext ct = (SynchronizationContext)e.Argument;

            // Loop continuously, looking for new tasks.  Sleep for a second 
            do
            {
                Thread.Sleep(1000);

                // If there are no tasks, we don't need to do anything...
                while (this.Tasks.Count != 0)
                {
                    // Find the first completed, cancelled or task more than 5 minutes old and delete it
                    for (int i = 0; i < Tasks.Count; i++)
                        if ((Tasks[i].Status == TaskStatus.Complete || Tasks[i].Status == TaskStatus.TimedOut || Tasks[i].Status == TaskStatus.Cancelled) &&
                            DateTime.Now.Subtract(Tasks[i].TaskFinished).TotalSeconds > 300)
                        {
                            // Signal the main thread to remove the task
                            ct.Send(new SendOrPostCallback((o) => { RemoveTask(i); }), null);
                            //this.Tasks.Remove(Tasks[i]);
                            break;
                        }

                    // Get the first queued task
                    Task t = null;
                    int idx = -1;

                    for (int i = 0; i < Tasks.Count; i++)
                    {
                        if (this.Tasks[i].Status == TaskStatus.Running)
                            // Oops - we're already running one task - break before we start another one!
                            break;

                        if (this.Tasks[i].Status == TaskStatus.Queued)
                        {
                            // Found one - note it and break
                            t = this.Tasks[i];
                            idx = i;
                            break;
                        }
                    }

                    // Did we find a task?
                    if (t != null)
                    {
                        // Yes, build the task and run it
                        string param;

                        param = "\\\\" + t.IP.ToString() + " -e "; // Computer's IP address.  PSExec should not load account's profile (quicker startup)

                        // Add credentials only if we have some to add
                        if (t.Credentials != null)
                        {
                            if (t.Credentials.domain == null)
                                param += "-u " + t.Credentials.username;                                // Local user
                            else
                                param += "-u " + t.Credentials.domain + "\\" + t.Credentials.username;      // Domain user and password

                            param += " -p " + t.Credentials.password + " ";
                        }

                        string batchfile = null;

                        // How many commands do we have to run?
                        if (t.Commands.Count == 1)
                            // Only one command.  This can be run directly by PSExec
                            param += t.Commands[0];
                        else
                        {
                            // Multiple commands.  Build a batch file
                            batchfile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".bat");

                            // Turn echo off in the batch file.  Output comes in an illocical order otherwise.
                            List<string> batch = new List<string>(t.Commands);
                            batch.Insert(0, "@echo off");

                            // Write batch file and add it to the 
                            File.WriteAllLines(batchfile, batch);
                            param += "-c " + batchfile;
                        }

                        // Trigger an event so other code can take action it may want to
                        ct.Send(new SendOrPostCallback((o) => { UpdateStatus(idx, TaskStatus.Running); }), null);
                        ct.Send(new SendOrPostCallback((o) => { OnRunning(t, EventArgs.Empty); }), null);

                        // Run PSExec
                        var psexec = new Process
                        {
                            StartInfo = new ProcessStartInfo
                            {
                                FileName = cfg.PSExecPath,
                                Arguments = param,
                                UseShellExecute = false,
                                RedirectStandardOutput = true,
                                RedirectStandardError = true,
                                CreateNoWindow = true
                            }
                        };

                        psexec.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
                        psexec.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);

                        output = "";

                        psexec.Start();
                        psexec.BeginOutputReadLine();
                        psexec.BeginErrorReadLine();

                        // Read output until the timeout has expired the process has terminated
                        t.TaskStarted = DateTime.Now;

                        do
                        {
                            // Wait 200ms for more output
                            Thread.Sleep(200);
                            
                            // Flush output and process events
                            psexec.CancelErrorRead();
                            psexec.CancelOutputRead();
                            psexec.BeginOutputReadLine();
                            psexec.BeginErrorReadLine();

                            Application.DoEvents();

                            // Is the publically available output any different to the building output?
                            if ((output != "" && t.Output == null) || (t.Output != null && output != t.Output))
                                // Yes, update the publically available output
                                ct.Send(new SendOrPostCallback((o) => { UpdateOutput(idx, output); }), null);

                        } while (DateTime.Now.Subtract(t.TaskStarted) < t.TimeoutInterval && !psexec.HasExited);

                        // If we created a batch file, delete it
                        if (batchfile != null)
                        {
                            try
                            {
                                File.Delete(batchfile);
                            }
                            catch (Exception ex)
                            {
                                // If we can't delete the file, we can't delete the file.  It'll be caught by the next disk cleanup.  We can at least write something to the debug log.
                                Debug.WriteLine("Unable to remove {0}: {1}", batchfile, ex.ToString());
                            }
                        }

                        // Did task complete
                        if (psexec.HasExited)
                        {
                            // Yes, mark it as complete and release any dependent tasks
                            ct.Send(new SendOrPostCallback((o) => { UpdateStatus(idx, TaskStatus.Complete); }), null);
                            ct.Send(new SendOrPostCallback((o) => { ProcessDependentTasks(t.TaskID, TaskStatus.Complete); }), null);
                        }
                        else
                        {
                            // No, kill it and cancel all dependent tasks
                            psexec.Kill();

                            ct.Send(delegate { this.UpdateStatus(idx, TaskStatus.TimedOut); }, null);
                            ct.Send(delegate { ProcessDependentTasks(t.TaskID, TaskStatus.TimedOut); }, null);
                        }

                        // Note the time the task finished
                        t.TaskFinished = DateTime.Now;
                    }
                }
            } while (true);
        }

        private void ProcessDependentTasks(int taskid, TaskStatus status)
        {
            // Loop through all tasks, looking for tasks dependent on the supplied taskid
            foreach (Task t in this.Tasks)
            {
                if (t.DependsOnTaskID == taskid)
                {
                    // Found it - what was the result of the supplied taskid?
                    switch (status)
                    {
                        case TaskStatus.Complete:
                            // Dependent tasks can be queued
                            t.Status = TaskStatus.Queued;
                            break;

                        case TaskStatus.TimedOut:
                            // All dependent tasks of both this any any task dependent on this must be cancelled
                            t.Status = TaskStatus.Cancelled;
                            t.Output = "Task cancelled due to the failure of task #" + taskid.ToString();
                            t.TaskFinished = DateTime.Now;
                            ProcessDependentTasks(t.TaskID, TaskStatus.Cancelled);
                            break;

                        case TaskStatus.Cancelled:
                            // All dependent tasks of both this any any task dependent on this must be cancelled
                            t.Status = TaskStatus.Cancelled;
                            t.Output = "Task cancelled due to the cancellation of task #" + taskid.ToString();
                            t.TaskFinished = DateTime.Now;
                            ProcessDependentTasks(t.TaskID, TaskStatus.Cancelled);
                            break;
                    }
                }
            }
        }

        private void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (outLine.Data == null) return;

            // Process each output line individually
            string[] lines = outLine.Data.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string l in lines)
            {
                // Does the text match the kind of output we get from PSExec?
                if (!Regex.IsMatch(l, "^PsExec v.*remotely$") &&
                    !Regex.IsMatch(l, "^Copyright.*Russinovich$") &&
                    !Regex.IsMatch(l, "^Sysinternals - www.sysinternals.com$") &&
                    !Regex.IsMatch(l, @"^Connecting .*\.\.\.$") &&
                    !Regex.IsMatch(l, @"^Starting .*\.\.\.$"))
                    // No, we can add it to the string
                    output += System.Environment.NewLine + l;
            }

            // Strip all double carriage returns
            while (output.Replace(System.Environment.NewLine + System.Environment.NewLine, System.Environment.NewLine) != output)
                output = output.Replace(System.Environment.NewLine + System.Environment.NewLine, System.Environment.NewLine);

            // Strip any leading or tailing carriage returns
            while (output != output.Trim('\r', '\n'))
                output = output.Trim('\r', '\n');
        }
    }
}
