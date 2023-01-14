using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace RemoteMsiManager
{
    public partial class FrmInstallProduct : Form
    {
        private readonly Computer _targetComputer;
        private readonly Localization _localization = Localization.GetInstance();

        internal FrmInstallProduct(Computer targetComputer)
        {
            InitializeComponent();

            if (Directory.Exists(Properties.Settings.Default.NetworkInstallSource))
            {
                txtBxLocalPackage.Text = Properties.Settings.Default.NetworkInstallSource;
            }

            txtBxTargetComputer.Text = targetComputer.ComputerName;
            _targetComputer = targetComputer;
        }

        #region (Methods)

        /// <summary>
        /// Check whether or not, all user inputs are valids.
        /// </summary>
        /// <param name="fileChecked">Indicate whether or not the user have checked a new file</param>
        /// <param name="folderChecked">Indicate wether or not the user have checked a new folder</param>
        /// <returns>Return true if all user inputs are valids</returns>
        private void ValidateData()
        {
            bool _sourceFileExists = true;
            bool _additionalFilesExists = true;
            bool _additionalFoldersExists = true;

            try
            {
                _sourceFileExists = File.Exists(txtBxLocalPackage.Text) && txtBxLocalPackage.Text.ToLower().EndsWith(".msi");

                foreach (string file in chkLstFiles.Items)
                {
                    if (!File.Exists(file))
                    {
                        _additionalFilesExists = false;
                        break;
                    }
                }

                foreach (string folder in chklstFolders.Items)
                {
                    if (!Directory.Exists(folder))
                    {
                        _additionalFoldersExists = false;
                        break;
                    }
                }
            }
            catch (Exception) { }

            btnInstall.Enabled = _sourceFileExists && _additionalFilesExists && _additionalFoldersExists;
            btnRemoveFiles.Enabled = (chkLstFiles.CheckedItems != null && chkLstFiles.CheckedItems.Count != 0);
            btnRemoveFolders.Enabled = (chklstFolders.CheckedItems != null && chklstFolders.CheckedItems.Count != 0);
        }

        private void DisplayStatus(string message)
        {
            txtBxStatus.Text = message;
            txtBxStatus.Refresh();
        }

        private void DisplayResult(uint resultCode)
        {
            if (MsiProduct.IsSuccess(resultCode))
            {
                if (MsiProduct.IsRebootNeeded(resultCode))
                {
                    txtBxResult.Text = _localization.GetLocalizedString("SuccessPendingReboot");
                }
                else
                {
                    txtBxResult.Text = _localization.GetLocalizedString("Success");
                }
                txtBxResult.BackColor = Color.LightGreen;
            }
            else
            {
                txtBxResult.Text = _localization.GetLocalizedString("Failed") + "(" + resultCode.ToString() + ") : " + MsiProduct.GetErrorMessage(resultCode);
                txtBxResult.BackColor = Color.Orange;
            }
            txtBxResult.Refresh();
        }

        /// <summary>
        /// Unable or disable bouton, textbox and checkedListbox of the UI
        /// </summary>
        /// <param name="isEnabled">true to enable the UI, false to disable the UI</param>
        private void LockUI(bool isEnabled)
        {
            btnBrowse.Enabled = isEnabled;
            btnInstall.Enabled = isEnabled;
            btnClose.Enabled = isEnabled;
            btnAddAdditionnalFiles.Enabled = isEnabled;
            btnRemoveFiles.Enabled = isEnabled;
            btnAddFolders.Enabled = isEnabled;
            btnRemoveFolders.Enabled = isEnabled;

            txtBxLocalPackage.Enabled = isEnabled;
            txtBxOptions.Enabled = isEnabled;
            ChkBxNeverRestart.Enabled = isEnabled;

            chkLstFiles.Enabled = isEnabled;
            chklstFolders.Enabled = isEnabled;
        }

        #endregion (Methods)

        #region (Events)

        // Buttons

        /// <summary>
        /// Allows the user to browse the file system to select one and only one MSI file. The file must exists.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                using (var fileBrowser = new OpenFileDialog())
                {
                    fileBrowser.InitialDirectory = txtBxLocalPackage.Text;
                    fileBrowser.Filter = "MSI Files|*.MSI";
                    if (fileBrowser.ShowDialog() == DialogResult.OK)
                    {
                        txtBxLocalPackage.Text = fileBrowser.FileName;
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void BtnAddAdditionnalFiles_Click(object sender, EventArgs e)
        {
            try
            {
                using (var fileBrowser = new OpenFileDialog())
                {
                    fileBrowser.InitialDirectory = txtBxLocalPackage.Text;
                    fileBrowser.Filter = "All Files|*.*|MST Files|*.MST";
                    fileBrowser.Multiselect = true;
                    if (fileBrowser.ShowDialog() == DialogResult.OK)
                    {
                        foreach (string file in fileBrowser.FileNames)
                        {
                            if (!chkLstFiles.Items.Contains(file))
                            {
                                chkLstFiles.Items.Add(file);
                            }
                            else
                                MessageBox.Show(_localization.GetLocalizedString("filealreadyincludes") + "\r\n" + file);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void BtnAddFolders_Click(object sender, EventArgs e)
        {
            try
            {
                using (var folderBrowser = new FolderBrowserDialog())
                {
                    folderBrowser.SelectedPath = txtBxLocalPackage.Text;
                    if (folderBrowser.ShowDialog() == DialogResult.OK)
                    {
                        if (!chklstFolders.Items.Contains(folderBrowser.SelectedPath))
                            chklstFolders.Items.Add(folderBrowser.SelectedPath);
                        else
                            MessageBox.Show(_localization.GetLocalizedString("folderalreadyincludes") + "\r\n" + folderBrowser.SelectedPath);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void BtnRemoveFiles_Click(object sender, EventArgs e)
        {
            try
            {
                while (chkLstFiles.CheckedItems.Count != 0)
                {
                    try
                    {
                        chkLstFiles.Items.Remove(chkLstFiles.CheckedItems[0]);
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            ValidateData();
        }

        private void BtnRemoveFolders_Click(object sender, EventArgs e)
        {
            try
            {
                while (chklstFolders.CheckedItems.Count != 0)
                {
                    try
                    {
                        chklstFolders.Items.Remove(chklstFolders.CheckedItems[0]);
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ValidateData();
        }

        private void BtnInstall_Click(object sender, EventArgs e)
        {
            LockUI(false);
            txtBxResult.Clear();
            txtBxResult.Refresh();
            string rootFolder = @"\\" + _targetComputer.ComputerName + @"\C$\Windows";
            string subFolder = Path.Combine(@"Temp\MsiManager", Path.GetRandomFileName());

            try
            {
                List<string> additionalFiles = new List<string>();
                List<string> additionalFolders = new List<string>();
                FileInfo mainFileInfo = new FileInfo(txtBxLocalPackage.Text);

                foreach (var item in chkLstFiles.Items)
                {
                    additionalFiles.Add(item.ToString());
                }
                foreach (var item in chklstFolders.Items)
                {
                    additionalFolders.Add(item.ToString());
                }

                DisplayStatus(_localization.GetLocalizedString("Copying"));
                _targetComputer.CopySourceToRemoteComputer(rootFolder, subFolder, mainFileInfo.FullName, additionalFiles, additionalFolders);

                DisplayStatus(_localization.GetLocalizedString("Installing"));
                var options = txtBxOptions.Text;

                if (ChkBxNeverRestart.Checked && !options.ToLower().Contains("reboot=reallysuppress"))
                {
                    options += (!string.IsNullOrEmpty(options) ? " " : string.Empty) + "REBOOT=ReallySuppress";
                }
                uint result = _targetComputer.InstallProduct(Path.Combine(rootFolder, subFolder, mainFileInfo.Name), options);

                DisplayResult(result);
            }
            catch (Computer.CopyFailedException ex)
            {
                txtBxResult.Text = ex.Message;
                txtBxResult.BackColor = Color.Orange;
            }
            catch (Exception ex)
            {
                txtBxResult.Text = ex.Message;
                txtBxResult.BackColor = Color.Orange;
            }
            try
            {
                DisplayStatus(_localization.GetLocalizedString("DeletingTemporaryFiles"));
                Directory.Delete(Path.Combine(rootFolder, subFolder), true);
                NetUse.UnMount(rootFolder);
                DisplayStatus(String.Empty);
            }
            catch (Exception) { }
            LockUI(true);
        }

        /// <summary>
        /// Saves the path to the main file and close the Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(txtBxLocalPackage.Text))
                {
                    FileInfo sourceFile = new FileInfo(txtBxLocalPackage.Text);

                    if (Directory.Exists(sourceFile.DirectoryName))
                    {
                        Properties.Settings.Default.NetworkInstallSource = sourceFile.DirectoryName;
                        Properties.Settings.Default.Save();
                    }
                }
            }
            catch (Exception) { }
            DialogResult = DialogResult.OK;
        }

        // ListBoxes

        private void ChkLstFiles_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
                btnRemoveFiles.Enabled = true;
            else
            {
                btnRemoveFiles.Enabled = (chkLstFiles.CheckedItems != null && chkLstFiles.CheckedItems.Count > 1);
            }
        }

        private void ChklstFolders_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
                btnRemoveFolders.Enabled = true;
            else
            {
                btnRemoveFolders.Enabled = (chklstFolders.CheckedItems != null && chklstFolders.CheckedItems.Count > 1);
            }
        }

        // TextBoxes

        private void TxtBxLocation_TextChanged(object sender, EventArgs e)
        {
            ValidateData();
        }

        private void TxtBxLocation_Leave(object sender, EventArgs e)
        {
            ValidateData();
        }

        #endregion (Events)
    }
}
