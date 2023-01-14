// Copyright (C) 2009 Daniel Bedarf
//
// For further information visit: http://wsusworkgroup.codeplex.com/
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Codeplex.DBedarf.WSUS.Workgroup.ClientSettingManager
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        #region UI Helper
        private void fillCombos()
        {
            //Values based on: http://technet.microsoft.com/de-de/library/cc708449(v=ws.10).aspx

            comboBoxAUOptions.Items.Clear();
            comboBoxAUOptions.DisplayMember = "Value";
            comboBoxAUOptions.Items.Add(new KeyValuePair<int, string>(2, "Notify before download"));
            comboBoxAUOptions.Items.Add(new KeyValuePair<int, string>(3, "Automatically download and notify of installation"));
            comboBoxAUOptions.Items.Add(new KeyValuePair<int, string>(4, "Automatically download and schedule installation"));
            comboBoxAUOptions.Items.Add(new KeyValuePair<int, string>(5, "Automatic Updates is required and users can configure it"));

            comboBoxScheduledInstallDay.Items.Clear();
            comboBoxScheduledInstallDay.DisplayMember = "Value";
            comboBoxScheduledInstallDay.Items.Add(new KeyValuePair<int, string>(0, "Every day"));
            comboBoxScheduledInstallDay.Items.Add(new KeyValuePair<int, string>(1, "Sunday"));
            comboBoxScheduledInstallDay.Items.Add(new KeyValuePair<int, string>(2, "Monday"));
            comboBoxScheduledInstallDay.Items.Add(new KeyValuePair<int, string>(3, "Tuesday"));
            comboBoxScheduledInstallDay.Items.Add(new KeyValuePair<int, string>(4, "Wednesday"));
            comboBoxScheduledInstallDay.Items.Add(new KeyValuePair<int, string>(5, "Thursday"));
            comboBoxScheduledInstallDay.Items.Add(new KeyValuePair<int, string>(6, "Friday"));
            comboBoxScheduledInstallDay.Items.Add(new KeyValuePair<int, string>(7, "Saturday"));

        }
        /// <summary>
        /// Handles errors and show a message to the user
        /// </summary>
        /// <param name="ex">The ex.</param>
        private void HandleError(Exception ex)
        {
            string msg = getErrorMessages(ex);
            MessageBox.Show(msg, "A problem occurred", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        /// <summary>
        /// Gets the error messages of exception and all inner exceptions.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>ErrorMessage</returns>
        private string getErrorMessages(Exception ex)
        {
            string retVal = string.Format("{0}\n", ex.Message);
            if (ex.InnerException != null) retVal += getErrorMessages(ex.InnerException);
            return retVal;
        }

        /// <summary>
        /// Shows a information - message to the user.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        private void ShowMessage(string msg)
        {
            MessageBox.Show(msg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        #endregion

        #region UI-Handling

        private void frmMain_Load(object sender, EventArgs e)
        {
            System.Version ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            this.Text = String.Format("{0} V{1}.{2} Rev: {3}", Application.ProductName, ver.Major, ver.Minor, ver.Build);

            fillCombos();
            ReadSettings(sender, e);
        }

        private void CheckRequiredField(TextBox txt)
        {
            if (string.IsNullOrEmpty(txt.Text))
            {
                errorProviderOK.SetError(txt, "");
                errorProviderNet.SetError(txt, "");
                errorProviderRed.SetError(txt, "Required");
            }
            else
            {
                errorProviderRed.SetError(txt, "");
                errorProviderOK.SetError(txt, "OK");
            }
        }

        private void txtWSUSServer_TextChanged(object sender, EventArgs e)
        {
            int offset = txtWSUSStateServer.Text.Length;
            if (offset > 0) offset = 1;

            if (txtWSUSServer.Text.Contains(txtWSUSStateServer.Text.Substring(0, txtWSUSStateServer.Text.Length - offset))) txtWSUSStateServer.Text = txtWSUSServer.Text;
            CheckRequiredField(txtWSUSServer);
            CheckRequiredField(txtWSUSStateServer);
        }

        private void chkEnableGroup_CheckStateChanged(object sender, EventArgs e)
        {
            txtGroupName.ReadOnly = !chkEnableGroup.Checked;
            if (!txtGroupName.ReadOnly)
            {
                CheckRequiredField(txtGroupName);
            }
            else
            {
                errorProviderRed.SetError(txtGroupName, "");
                errorProviderOK.SetError(txtGroupName, "");
                errorProviderNet.SetError(txtGroupName, "");
            }
        }

        private void txtGroupName_TextChanged(object sender, EventArgs e)
        {
            CheckRequiredField(txtGroupName);
        }

        private void txtWSUSLogServer_TextChanged(object sender, EventArgs e)
        {
            CheckRequiredField(txtWSUSStateServer);
        }

        #endregion

        #region Code-Action
        private void action_WUServiceStop(object sender, EventArgs e)
        {
            try
            {
                WSUSSettingManager.ServiceStop();
                ShowMessage("Service stopped");
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void action_WUServiceStart(object sender, EventArgs e)
        {
            try
            {
                WSUSSettingManager.ServiceStart();
                ShowMessage("Service started");
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }


        private void action_Exit(object sender, EventArgs e)
        {
            this.Close();
        }

        private void action_ShowAbout(object sender, EventArgs e)
        {
            frmAbout frm = new frmAbout();
            frm.ShowDialog();
        }
        #endregion

        #region Main-Action

        private void action_ExportRegToFile(object sender, EventArgs e)
        {
            try
            {
                saveRegFile_Dialog.FileName = String.Format("{0}_{1}", System.Environment.MachineName, DateTime.Now.ToString("yyyy-MM-dd_HHmm"));
                if (saveRegFile_Dialog.ShowDialog() == DialogResult.OK)
                {
                    WSUSSettingManager man = new WSUSSettingManager();
                    WSUSSettingManager.BackupCurrentSettings(saveRegFile_Dialog.FileName);
                }//end if safefile
            }//end try
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void ReadSettings(object sender, EventArgs e)
        {
            try
            {

                //read currentsettings without change reg
                WSUSSettingManager wsmgr = new WSUSSettingManager();
                txtWSUSServer.Text = wsmgr.WSUSServer;

                txtWSUSStateServer.Text = wsmgr.WSUSStateServer;
                chkEnableGroup.Checked = wsmgr.EnableGroupSettings;
                txtGroupName.Text = wsmgr.ComputergroupName;
                chkEnableUpdateInterval.Checked = wsmgr.DetectionFrequencyEnabled;

                int adu = wsmgr.DetectionFrequency;
                if (adu >= numUpdateInterval.Minimum && adu <= numUpdateInterval.Maximum)
                    numUpdateInterval.Value = adu;

                chkEnableRebootDelay.Checked = wsmgr.RebootRelaunchTimeoutEnabled;

                int rdl = wsmgr.RebootRelaunchTimeout;
                if (rdl >= numRebootDelayTime.Minimum && rdl <= numRebootDelayTime.Maximum)
                    numRebootDelayTime.Value = rdl;

                chkAutoInstallMinor.Checked = wsmgr.AutoInstallMinorUpdates;
                chkNoRebootWithUser.Checked = !wsmgr.AllowRebootIfUserLoggedOn;
                chkNonAdminInstall.Checked = wsmgr.AllowNonAdminInstall;

                int? auOptions = wsmgr.AUOptions;
                if (auOptions != null)
                {
                    selectKey(comboBoxAUOptions, wsmgr.AUOptions);
                }
                else
                {
                    comboBoxAUOptions.SelectedIndex = -1; // If no reg settings, we don't select any index by default
                }

                selectKey(comboBoxScheduledInstallDay, wsmgr.ScheduledInstallDay);

                comboBoxAUOptions_SelectedIndexChanged(sender, e); // We check what is selected in the combo box.
                selectKey(comboBoxScheduledInstallDay, wsmgr.ScheduledInstallDay);

                numScheduledInstallTime.Value = wsmgr.ScheduledInstallTime;

                chkDisableWindowsUpdateAccess.Checked = wsmgr.DisableWindowsUpdateAccess;
                chkAcceptTrustedPublisherCerts.Checked = wsmgr.AcceptTrustedPublisherCerts;

                chkRebootWarningTimeout.Checked = wsmgr.RebootWarningTimeoutEnabled;
                numRebootWarningTimeout.Value = wsmgr.RebootWarningTimeout;

                chkRescheduleWaitTime.Checked = wsmgr.RescheduleWaitTimeEnabled;
                numRescheduleWaitTime.Value = wsmgr.RescheduleWaitTime;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                HandleError(ex);
            }
        }

        private void selectKey(ComboBox combo, int key)
        {
            for (int i = 0; i < combo.Items.Count; i++)
            {
                KeyValuePair<int, string> kvp = (KeyValuePair<int, string>)combo.Items[i];
                if (kvp.Key == key)
                {
                    combo.SelectedIndex = i;
                    break;
                }
            }
        }

        private void cmdWriteSettings_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to enable the WSUS settings?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    saveRegFile_Dialog.FileName = String.Format("{0}_{1}", System.Environment.MachineName, DateTime.Now.ToString("yyyy-MM-dd_HHmm"));
                    if (saveRegFile_Dialog.ShowDialog() == DialogResult.OK)
                    {
                        WSUSSettingManager.BackupCurrentSettings(saveRegFile_Dialog.FileName);
                    }//end if safefile

                    WSUSSettingManager man = new WSUSSettingManager(true);
                    Cursor = Cursors.WaitCursor;

                    man.WSUSServer = txtWSUSServer.Text;
                    man.WSUSStateServer = txtWSUSStateServer.Text;
                    man.EnableGroupSettings = chkEnableGroup.Checked;
                    man.ComputergroupName = txtGroupName.Text;
                    man.DetectionFrequencyEnabled = chkEnableUpdateInterval.Checked;
                    man.DetectionFrequency = (int)numUpdateInterval.Value;
                    man.RebootRelaunchTimeoutEnabled = chkEnableRebootDelay.Checked;
                    man.RebootRelaunchTimeout = (int)numRebootDelayTime.Value;
                    man.AutoInstallMinorUpdates = chkAutoInstallMinor.Checked;
                    man.AllowRebootIfUserLoggedOn = !chkNoRebootWithUser.Checked;
                    man.AllowNonAdminInstall = chkNonAdminInstall.Checked;
                    if (comboBoxAUOptions.SelectedIndex != -1) // Only if a value is selected
                    {
                        man.AUOptions = ((KeyValuePair<int, string>)comboBoxAUOptions.SelectedItem).Key;
                    }
                    if (comboBoxScheduledInstallDay.SelectedItem != null)
                        man.ScheduledInstallDay = ((KeyValuePair<int, string>)comboBoxScheduledInstallDay.SelectedItem).Key;

                    man.ScheduledInstallTime = (int)numScheduledInstallTime.Value;
                    man.DisableWindowsUpdateAccess = chkDisableWindowsUpdateAccess.Checked;
                    man.AcceptTrustedPublisherCerts = chkAcceptTrustedPublisherCerts.Checked;
                    man.RebootWarningTimeoutEnabled = chkRebootWarningTimeout.Checked;
                    man.RebootWarningTimeout = (int)numRebootWarningTimeout.Value;
                    man.RescheduleWaitTimeEnabled = chkRescheduleWaitTime.Checked;
                    man.RescheduleWaitTime = (int)numRescheduleWaitTime.Value;


                    //chkEnableAutoUpdate.Checked 
                    WSUSSettingManager.ServiceRestart();
                    WSUSSettingManager.StartWUAUCtl(WSUSSettingManager.eWUAUCtlCmd.detectnow);
                    ShowMessage("WSUS has been successfully configured.");
                }//end try
                catch (Exception ex)
                {
                    HandleError(ex);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }

            }//end if really?
        }

        private void action_removeWSUSRegKeys(object sender, EventArgs e)
        {
            //Are you sure etc.;
            if (MessageBox.Show("Are you sure you want to remove the WSUS settings?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                WSUSSettingManager wsmgr = new WSUSSettingManager(true);
                bool backupDone = false;
                saveRegFile_Dialog.FileName = String.Format("{0}_{1}", System.Environment.MachineName, DateTime.Now.ToString("yyyy-MM-dd_HHmm"));
                if (saveRegFile_Dialog.ShowDialog() == DialogResult.OK)
                {
                    WSUSSettingManager.BackupCurrentSettings(saveRegFile_Dialog.FileName);
                    backupDone = true;
                }
                if (backupDone == false)
                {
                    //let the user overwrite the backup - flag
                    backupDone = (MessageBox.Show("Are you sure you want to remove the WSUS settings without backup?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes);
                }
                if (backupDone)
                {
                    WSUSSettingManager.RemoveWSUS();
                }

                // We then refresh the GUI from settings in the registry. The GUI will appear empty since
                // settings have been deleted
                ReadSettings(sender, e);
            }
        }

        private void comboBoxAUOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            // We only activate fields when AUOptions = 4 
            // 4 = Automatically download and schedule installation. (Only valid if values exist for ScheduledInstallDay and ScheduledInstallTime.)
            bool activeFields = false;
            if (comboBoxAUOptions.SelectedItem != null)
            {
                KeyValuePair<int, string> kvp = (KeyValuePair<int, string>)comboBoxAUOptions.SelectedItem;
                activeFields = kvp.Key == 4;
            }
            comboBoxScheduledInstallDay.Enabled = activeFields;
            numScheduledInstallTime.Enabled = activeFields;
        }

        #endregion

        #region WebCalls

        private void openMicrosoftKB(string number)
        {
            string lang = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            openHomepage(String.Format("http://support.microsoft.com/kb/{0}/{1}", number, lang));
        }

        private void openHomepage(string URL)
        {
            try
            {
                System.Diagnostics.Process.Start(URL);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void action_ShowWSUSHP_01(object sender, EventArgs e)
        {
            openHomepage("http://technet.microsoft.com/en-us/library/cc720464(v=ws.10).aspx");
        }

        private void action_ShowKB328010(object sender, EventArgs e)
        {
            openMicrosoftKB("328010");
        }

        private void action_ShowHomepage(object sender, EventArgs e)
        {
            openHomepage("http://wsusworkgroup.codeplex.com/");
        }

        private void action_ShowLicense(object sender, EventArgs e)
        {
            openHomepage("http://wsusworkgroup.codeplex.com/license");
        }

        private void action_OpenKBLogfile(object sender, EventArgs e)
        {
            openMicrosoftKB("902093");
        }

        private void action_OpenKbSusClientId(object sender, EventArgs e)
        {
            openMicrosoftKB("903262");
        }

        #endregion

        #region local Tools

        private void action_ShowWindowsUpdateAssistent(object sender, EventArgs e)
        {
            WSUSSettingManager.StartWUAUCtl(WSUSSettingManager.eWUAUCtlCmd.ShowSettingsDialog);
        }

        private void action_ShowWindowsUpdate(object sender, EventArgs e)
        {
            WSUSSettingManager.StartWUAUCtl(WSUSSettingManager.eWUAUCtlCmd.ShowWindowsUpdate);
        }

        private void action_ShowSystrayNotification(object sender, EventArgs e)
        {
            WSUSSettingManager.StartWUAUCtl(WSUSSettingManager.eWUAUCtlCmd.DemoUI);
        }

        private void action_SendReportToWSUS(object sender, EventArgs e)
        {
            WSUSSettingManager.StartWUAUCtl(WSUSSettingManager.eWUAUCtlCmd.reportnow);
        }

        private void action_removeWSUSCoockie(object sender, EventArgs e)
        {
            WSUSSettingManager.StartWUAUCtl(WSUSSettingManager.eWUAUCtlCmd.resetauthorization);
        }

        private void action_ForceDetectUpdates(object sender, EventArgs e)
        {
            WSUSSettingManager.StartWUAUCtl(WSUSSettingManager.eWUAUCtlCmd.detectnow);
        }

        private void action_OpenWindowsUpdateLogfile(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(System.Environment.ExpandEnvironmentVariables("%windir%\\WindowsUpdate.log"));
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void action_ResetSusClientId(object sender, EventArgs e)
        {
            WSUSSettingManager wsmgr = new WSUSSettingManager(true);
            wsmgr.ResetSusClientId();
            ShowMessage("SusClientID regenerated.");
        }

        #endregion

    }
}