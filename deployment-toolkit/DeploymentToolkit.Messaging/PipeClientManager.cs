using DeploymentToolkit.Messaging.Events;
using DeploymentToolkit.Messaging.Messages;
using DeploymentToolkit.Modals;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;

namespace DeploymentToolkit.Messaging
{
    public class PipeClientManager : IDisposable
    {

        public int ConnectedClients => _clients.Count;
        public event EventHandler<NewMessageEventArgs> OnNewMessage;

        internal event EventHandler OnTrayStarted;
        internal event EventHandler OnTrayStopped;

        internal DeploymentStep CurrentStep = DeploymentStep.Start;

        internal string TrayAppExeName => ToolkitEnvironment.EnvironmentVariables.DeploymentToolkitTrayExeName;
        private string _trayAppExeNameLowered;
        internal string TrayAppExeNameLowered
        {
            get
            {
                if(_trayAppExeNameLowered == null)
                {
                    _trayAppExeNameLowered = TrayAppExeName.ToLower();
                }

                return _trayAppExeNameLowered;
            }
        }
        private string _trayAppExeNameWithoutExtension;
        internal string TrayAppExeNameWithoutExtension
        {
            get
            {
                if(_trayAppExeNameWithoutExtension == null)
                {
                    _trayAppExeNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(TrayAppExeName);
                }

                return _trayAppExeNameWithoutExtension;
            }
        }

        private const int TrayAppStartTimeOut = 5000;
        private static int _trayAppRetrys = 3;

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly TimeoutManager _timeoutManager;

        private ManagementEventWatcher _startWatcher;
        private ManagementEventWatcher _stopWatcher;

        private readonly object _collectionLock = new object();
        /// <summary>
        /// Key: ProcessID
        /// Value: PipeClient(Manager)
        /// </summary>
        private readonly Dictionary<int, PipeClient> _clients = new Dictionary<int, PipeClient>();
        private string _lastSentMessage = null;

        private readonly object _messageLock = new object();
        private bool _hasReceivedDeferMessage = false;
        private bool _hasReceivedCloseMessage = false;
        private bool _hasReceivedRestartMessage = false;

        public PipeClientManager()
        {
            var processes = Process.GetProcessesByName(TrayAppExeNameWithoutExtension);
            if(processes.Length == 0)
            {
                TryStartTray(true);
            }

            if(_trayAppRetrys == 0)
            {
                _logger.Warn("Failed to start tray apps");
            }
            else
            {
                processes = Process.GetProcessesByName(TrayAppExeNameWithoutExtension);

                _logger.Debug($"Found {processes.Length} running instances of {TrayAppExeName}");
                foreach(var process in processes)
                {
                    TryStartClient(process.Id, out _);
                }
                _logger.Info($"Successfully connected to {_clients.Count} tray apps");

                _logger.Trace("Starting TimeoutManager ...");
                _timeoutManager = new TimeoutManager(this);
            }

            _logger.Debug($"Watching for new starts or stopps of {TrayAppExeName}");
            MonitorWMI();
        }

        internal void TryStartTray(bool startup = false)
        {
            _trayAppRetrys = 3;

            if(!startup)
            {
                System.Threading.Thread.Sleep(TrayAppStartTimeOut);
            }

            var processes = Process.GetProcessesByName(TrayAppExeNameWithoutExtension);
            while(processes.Length == 0 && _trayAppRetrys-- > 0)
            {
                _logger.Info($"There is currently no tray app running on this system. Trying to start tray apps. Retries left: {_trayAppRetrys}");

                // Start tray apps if not running
                Util.WindowsEventLog.RequestTrayAppStart();
                Util.ProcessUtil.StartTrayAppForAllLoggedOnUsers();

                _logger.Trace("Waiting for start ...");
                System.Threading.Thread.Sleep(TrayAppStartTimeOut);

                _logger.Trace("Scanning for tray apps ...");
                processes = Process.GetProcessesByName(TrayAppExeNameWithoutExtension);
            }
        }

        internal void FakeReceivedMessage(NewMessageEventArgs e)
        {
            _logger.Debug($"Faking receive of {e.MessageId}");
            e.Simulated = true;

            OnNewMessageReceived(null, e);
        }

        private void OnNewMessageReceived(object sender, NewMessageEventArgs e)
        {

            if(!(sender is PipeClient) && !e.Simulated)
            {

                _logger.Warn("Received message from unknown source");
                return;
            }

            var client = (PipeClient)sender;
            switch(e.MessageId)
            {
                case MessageId.ContinueDeployment:
                {
                    var message = e.Message as ContinueMessage;
                    lock(_messageLock)
                    {
                        if(message.DeploymentStep == DeploymentStep.DeferDeployment)
                        {
                            if(_hasReceivedDeferMessage && !e.Simulated)
                            {
                                _logger.Trace($"Ignoring defer answer from session {client.SessionId} as there was already a prior response");
                                return;
                            }

                            _hasReceivedDeferMessage = true;
                        }
                        else if(message.DeploymentStep == DeploymentStep.CloseApplications)
                        {
                            if(_hasReceivedCloseMessage && !e.Simulated)
                            {
                                _logger.Trace($"Ignoring close apps answer from session {client.SessionId} as there was already a prior response");
                                return;
                            }

                            _hasReceivedCloseMessage = true;
                        }
                        else if(message.DeploymentStep == DeploymentStep.Restart)
                        {
                            if(_hasReceivedRestartMessage && !e.Simulated)
                            {
                                _logger.Trace($"Ignoring restart answer from session {client.SessionId} as there was already a prior response");
                                return;
                            }

                            _hasReceivedRestartMessage = true;
                        }
                    }

                    OnNewMessage.BeginInvoke(
                        client,
                        e,
                        OnNewMessage.EndInvoke,
                        null
                    );
                }
                break;

                case MessageId.AbortDeployment:
                {
                    var message = e.Message as AbortMessage;
                    lock(_messageLock)
                    {
                        if(message.DeploymentStep == DeploymentStep.Restart)
                        {
                            if(_hasReceivedRestartMessage && !e.Simulated)
                            {
                                _logger.Trace($"Ignoring restart answer from session {client.SessionId} as there was already a prior response");
                                return;
                            }

                            _hasReceivedRestartMessage = true;
                        }
                    }

                    OnNewMessage.BeginInvoke(
                        client,
                        e,
                        OnNewMessage.EndInvoke,
                        null
                    );
                }
                break;

                case MessageId.DeferDeployment:
                {
                    lock(_messageLock)
                    {
                        if(_hasReceivedDeferMessage)
                        {
                            _logger.Trace($"Ignoring answer from session {client.SessionId} as there was already a prior response");
                            return;
                        }

                        _hasReceivedDeferMessage = true;
                    }

                    // Notify our installation about the deferal
                    OnNewMessage.BeginInvoke(
                        client,
                        e,
                        OnNewMessage.EndInvoke,
                        null
                    );
                }
                break;
            }
        }

        private bool TryStartClient(int processId, out PipeClient client)
        {
            client = new PipeClient(processId);
            if(client.IsConnected)
            {
                _clients.Add(
                    processId,
                    client
                );

                client.OnNewMessage += OnNewMessageReceived;
                return true;
            }
            return false;
        }

        private void MonitorWMI()
        {
            if(_startWatcher != null)
            {
                _startWatcher.Dispose();
                _startWatcher = null;
            }

            _startWatcher = new ManagementEventWatcher(
                new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace")
            );
            _startWatcher.EventArrived += OnProcessStarted;
            _startWatcher.Start();

            if(_stopWatcher != null)
            {
                _stopWatcher.Dispose();
                _stopWatcher = null;
            }

            _stopWatcher = new ManagementEventWatcher(
                new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace")
            );
            _stopWatcher.EventArrived += OnProcessStopped;
            _stopWatcher.Start();
        }

        private void OnProcessStarted(object sender, EventArrivedEventArgs e)
        {
            var processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
            var processId = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);
            _logger.Trace($"New process started: [{processId}] {processName}");
            if(processName.ToLower() == TrayAppExeNameLowered)
            {
                _logger.Debug("New Tray app started. Initiating connection...");

                lock(_collectionLock)
                {
                    try
                    {
                        if(TryStartClient(processId, out var client))
                        {
                            if(!string.IsNullOrEmpty(_lastSentMessage))
                            {
                                // Send the last message to the client
                                client.SendMessage(_lastSentMessage);
                            }

                            OnTrayStarted?.BeginInvoke(
                                this,
                                null,
                                OnTrayStarted.EndInvoke,
                                null
                            );
                        }
                    }
                    catch(Exception ex)
                    {
                        _logger.Error(ex, $"Failed to process {processId}");
                    }
                }
            }
        }

        private void OnProcessStopped(object sender, EventArrivedEventArgs e)
        {
            var processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
            var processId = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);
            _logger.Trace($"Process ended: [{processId}] {processName}");
            if(_clients.ContainsKey(processId))
            {
                _logger.Debug($"Tray app closed. Disposing pipe");
                lock(_collectionLock)
                {
                    try
                    {
                        _clients[processId].Dispose();
                        _clients.Remove(processId);

                        OnTrayStopped?.BeginInvoke(
                            this,
                            null,
                            OnTrayStopped.EndInvoke,
                            null
                        );
                    }
                    catch(Exception ex)
                    {
                        _logger.Error(ex, $"Failed to process {processId}");
                    }
                }
            }
        }

        public void SendMessage(IMessage message)
        {
            _logger.Trace($"Sending {message.MessageId} to {_clients.Count} clients");
            var data = Serializer.SerializeMessage(message);
            lock(_collectionLock)
            {
                foreach(var client in _clients.Values)
                {
                    client.SendMessage(data);
                    _logger.Trace($"Sent message to {client.SessionId}");
                }

                _lastSentMessage = data;
            }

            if(message is DeferMessage)
            {
                CurrentStep = DeploymentStep.DeferDeployment;
            }
            else if(message is CloseApplicationsMessage)
            {
                CurrentStep = DeploymentStep.CloseApplications;
            }
            else if(message is DeploymentRestartMessage)
            {
                CurrentStep = DeploymentStep.Restart;
            }
            else if(message.MessageId == MessageId.DeploymentError || message.MessageId == MessageId.DeploymentSuccess)
            {
                CurrentStep = DeploymentStep.End;
            }
        }

        public void Dispose()
        {
            _logger.Trace("Disposing...");

            _logger.Trace("Stopping WMI watchers...");
            _startWatcher?.Stop();
            _startWatcher?.Dispose();
            _stopWatcher?.Stop();
            _stopWatcher?.Dispose();

            _logger.Trace("Stopping TimeoutManager ...");
            _timeoutManager?.Dispose();

            _logger.Trace("Stopping clients...");
            foreach(var client in _clients.Values)
            {
                client.Dispose();
            }

            _logger.Trace("Disposed");
        }
    }
}
