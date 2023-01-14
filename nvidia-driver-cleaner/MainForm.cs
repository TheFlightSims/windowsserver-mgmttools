using NvidiaCleaner.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace NvidiaCleaner
{
    public partial class MainForm : Form, IMainForm
    {
        private const string logFileName = "NCleaner.log";
        private readonly string _osPath = Path.GetPathRoot(Environment.SystemDirectory);
        private readonly List<string> _errors = new List<string>();
        private double totalCleanedMb = 0;

        public MainForm()
        {
            InitializeComponent();
            //Initial Settings.
            cExtracted.Checked = true;
            cDownloaded.Checked = true;
        }

        //Calculates the percentage of each checked setting except generate log.
        public int BarPercentage
        {
            get
            {
                int total = 0, result = 0;
                if (cDownloaded.Checked == true)
                    total++;
                if (cExtracted.Checked == true)
                    total++;
                if (cRepository.Checked == true)
                    total++;
                switch (total)
                {
                    case 1:
                        result = 100;
                        break;
                    case 2:
                        result = 50;
                        break;
                    case 3:
                        result = 33;
                        progressBar.Maximum = 99;
                        break;
                    case 4:
                        result = 25;
                        break;
                    default:
                        break;
                }
                return result;
            }
        }

        public void DeleteExtractedDrivers()
        {
            DeleteExtractedDrivers(Text);
        }

        //Method that delets the extracted drivers at System:\NVIDIA path.
        public void DeleteExtractedDrivers(string Text)
        {
            string path = _osPath + @"NVIDIA";
            double deletedTotal = 0;
            Text += @" Extracted Drivers " + Environment.NewLine;
            if (Directory.Exists(path))
            {
                Text += path + @" FOUND! " + Environment.NewLine;

                long sizeB = NVC.GetDirectorySize(path);
                double sizeMB = NVC.SizeConvert(sizeB, "MB");

                try
                {
                    Directory.Delete(path, true);
                    Text += path + @" Deleted! " + sizeMB + @" MB cleaned!" + Environment.NewLine;
                    deletedTotal += sizeMB;
                    progressBar.Value += BarPercentage;
                }
                catch (Exception e)
                {
                    _errors.Add(e.ToString());
                    Text += path + @" is not deleted! " + Environment.NewLine + @"Error: " + e.Message + Environment.NewLine;
                }
            }
            else
            {
                Text += path + @" NOT FOUND! PASSING... " + Environment.NewLine;
                progressBar.Value += BarPercentage;
            }

            Text += @"Total " + deletedTotal + @" MB Deleted in " + path + Environment.NewLine;
            Text += @" Extracted Drivers End " + Environment.NewLine + Environment.NewLine;
            totalCleanedMb += deletedTotal;
        }

        public void DeleteDownloadedDrivers()
        {
            string path = _osPath + @"ProgramData\NVIDIA Corporation\Downloader";
            double deletedTotal = 0;
            Text += @" Downloaded Drivers " + Environment.NewLine;

            if (Directory.Exists(path))
            {
                //ONLY DELETE latest and random numbers.
                string whiteList = "config"; //Maybe add latest but not necessary.
                foreach (var dir in Directory.GetDirectories(path))
                {
                    if (!dir.Contains(whiteList))
                    {
                        Text += dir.Substring(45) + @" FOUND! " + Environment.NewLine;
                        long sizeB = NVC.GetDirectorySize(dir);
                        double sizeMB = NVC.SizeConvert(sizeB, "MB");

                        try
                        {
                            Directory.Delete(dir, true);
                            deletedTotal += sizeMB;
                            Text += dir.Substring(45) + @" Deleted! " + sizeMB + @" MB cleaned!" + Environment.NewLine + Environment.NewLine;
                        }
                        catch (Exception e)
                        {
                            _errors.Add(e.ToString());
                            Text += dir + @" is not deleted! " + Environment.NewLine + @"Error: " + e.Message + Environment.NewLine + Environment.NewLine;
                        }
                    }
                }
                progressBar.Value += BarPercentage;
            }
            else
            {
                Text += path + @" NOT FOUND! PASSING... " + Environment.NewLine;
                progressBar.Value += BarPercentage;
            }
            Text += @"Total " + deletedTotal + @" MB Deleted in " + path + Environment.NewLine;
            Text += @" Downloaded Drivers End " + Environment.NewLine + Environment.NewLine;
            totalCleanedMb += deletedTotal;
        }

        //Method that delets the downloaded drivers at System:\Program Files\NVIDIA Corporation\Installer2 path.
        public void DeleteRepository()
        {
            //DO NOT DELETE THE INSTALLER2 FOLDER ITSELF!!!
            //Delete Repository. (Comment by TheFlightSims employee: really?)
            string path = _osPath + @"Program Files\NVIDIA Corporation\Installer2";
            double deletedTotal = 0;
            Text += @" Repository " + Environment.NewLine;

            if (Directory.Exists(path))
            {
                foreach (var dir in Directory.GetDirectories(path))
                {
                    Text += dir.Substring(47) + @" FOUND! " + Environment.NewLine;
                    long sizeB = NVC.GetDirectorySize(dir);
                    double sizeMB = NVC.SizeConvert(sizeB, "MB");

                    try
                    {
                        Directory.Delete(dir, true);
                        deletedTotal += sizeMB;
                        Text += dir.Substring(47) + @" Deleted! " + sizeMB + @" MB cleaned!" + Environment.NewLine + Environment.NewLine;
                    }
                    catch (Exception e)
                    {
                        _errors.Add(e.ToString());
                        Text += dir + @" is not deleted! " + Environment.NewLine + @"Error: " + e.Message + Environment.NewLine + Environment.NewLine;
                    }
                }
                progressBar.Value += BarPercentage;
            }
            else
            {
                Text += path + @" NOT FOUND! PASSING... " + Environment.NewLine;
                progressBar.Value += BarPercentage;
            }
            Text += @"Total " + deletedTotal + @" MB Deleted in " + path + Environment.NewLine;
            Text += @"-----Repository End-----" + Environment.NewLine + Environment.NewLine;
            totalCleanedMb += deletedTotal;
        }

        public void DeleteLogs()
        {
            string[] path = { _osPath + @"ProgramData\NVIDIA Corporation\GeForce Experience\Logs", _osPath + @"ProgramData\NVIDIA Corporation\NVIDIA GeForce Experience\Logs" };

            for (int i = 0; i < 2; i++)
            {
                double deletedTotal = 0;
                Text += @" Unneccessary Logs " + (i + 1 + " ") + Environment.NewLine;
                if (Directory.Exists(path[i]))
                {
                    long sizeB = NVC.GetDirectorySize(path[i]);
                    double sizeMB = NVC.SizeConvert(sizeB, " MB");

                    foreach (var item in Directory.GetFiles(path[i]))
                    {
                        try
                        {
                            if (item.EndsWith(".log"))
                            {
                                File.Delete(item);
                                Text += item + @" Deleted! " + Environment.NewLine;
                            }
                        }
                        catch (Exception e)
                        {
                            _errors.Add(e.ToString());
                            Text += item + @" is not deleted! " + Environment.NewLine + @"Error: " + e.Message + Environment.NewLine + Environment.NewLine;
                        }
                    }
                    deletedTotal += sizeMB;
                }
                else
                {
                    Text += path[i] + @" NOT FOUND! PASSING... " + Environment.NewLine;
                }
                Text += @"Total " + deletedTotal + @" MB Deleted in " + path[i] + Environment.NewLine;
                Text += @" Unneccessary Logs " + (i + 1) + @" End " + Environment.NewLine + Environment.NewLine;
                totalCleanedMb += deletedTotal;
            }
        }

        public void DeleteWindowsDrivers()
        {
            //Way too dangerous to proceed. Had been disabled
        }

        public void GenerateLog()
        {
            //TODO: Implement real log method.

            string path = Environment.CurrentDirectory;

            if (_errors.Count != 0)
            {
                try
                {
                    System.IO.File.WriteAllLines(path + @"\" + logFileName, _errors);
                }
                catch
                {
                    MessageBox.Show(@" Error while creating log.");
                }
            }
        }

        private void FreeUpSpaceButton_Click(object sender, EventArgs e)
        {
            progressBar.Value = 0;
            tabSettings.Enabled = false;

            GenerateLog();

            try
            {
                if (cExtracted.Checked == true)
                    DeleteExtractedDrivers();

                if (cDownloaded.Checked == true)
                {
                    DeleteDownloadedDrivers();
                    DeleteLogs();
                }

                if (cRepository.Checked == true)
                    DeleteRepository();
                MessageBox.Show(@"Cleaned " + totalCleanedMb + " MiB", @"Completed");
            }
            catch (Exception ex)
            {
                _errors.Add(ex.Message);
            }
            tabSettings.Enabled = true;
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
    }
}