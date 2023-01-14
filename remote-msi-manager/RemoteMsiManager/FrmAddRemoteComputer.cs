using System;
using System.Windows.Forms;

namespace RemoteMsiManager
{
    public partial class FrmAddRemoteComputer : Form
    {
        private readonly Timer _chrono = new Timer();
        private readonly Localization _localization = Localization.GetInstance();

        public FrmAddRemoteComputer()
        {
            InitializeComponent();

            try
            {
                txtBxUsername.Text = Properties.Settings.Default.AdminUser;
                _chrono.Interval = 2000;
                _chrono.Tick += Chrono_Tick;
            }
            catch (Exception) { }
        }

        public FrmAddRemoteComputer(string computerName, string username)
            : this()
        {
            txtBxComputerName.Enabled = false;
            ComputerName = computerName;
            Username = username;
            if (!String.IsNullOrEmpty(txtBxUsername.Text))
            { txtBxPassword.Select(); }
            else
            { txtBxUsername.Select(); }
        }

        #region (internal properties)

        /// <summary>
        /// Name or IP address of the computer
        /// </summary>
        internal string ComputerName
        {
            get { return txtBxComputerName.Text; }
            set { txtBxComputerName.Text = value; }
        }

        /// <summary>
        /// Username used to query the remote computer
        /// </summary>
        internal string Username
        {
            get { return txtBxUsername.Text; }
            set { txtBxUsername.Text = value; }
        }

        /// <summary>
        /// Password used with the provided <see cref="Username"/> to query the remote computer
        /// </summary>
        internal string Password
        {
            get { return txtBxPassword.Text; }
            set { txtBxPassword.Text = value; }
        }

        #endregion (internal properties)

        #region (private methods)

        private void ValidateData()
        {
            btnOk.Enabled = !String.IsNullOrEmpty(txtBxComputerName.Text); // && !String.IsNullOrEmpty(txtBxUsername.Text) && !String.IsNullOrEmpty(txtBxPassword.Text);
        }

        private void PingRemoteComputer()
        {
            try
            {
                btnPing.Image = Properties.Resources.Orange16x16;
                toolTip1.SetToolTip(btnPing, _localization.GetLocalizedString("Testing"));
                if (!String.IsNullOrEmpty(txtBxComputerName.Text))
                {
                    using (var ping = new System.Net.NetworkInformation.Ping())
                    {
                        ping.PingCompleted += Ping_PingCompleted;
                        ping.SendAsync(txtBxComputerName.Text, null);
                    }
                }
            }
            catch (Exception)
            {

                btnPing.Image = Properties.Resources.Red16x16;
                toolTip1.SetToolTip(btnPing, _localization.GetLocalizedString("CantPing"));
            }
        }

        private void Ping_PingCompleted(object sender, System.Net.NetworkInformation.PingCompletedEventArgs e)
        {
            try
            {
                if (e.Reply != null && e.Reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    btnPing.Image = Properties.Resources.Green16x16;
                    toolTip1.SetToolTip(btnPing, _localization.GetLocalizedString("PingOK"));
                }
                else
                {
                    btnPing.Image = Properties.Resources.Red16x16;
                    toolTip1.SetToolTip(btnPing, _localization.GetLocalizedString("CantPing"));
                }
            }
            catch (Exception) { }
        }

        #endregion (private methods)

        #region (events)

        // Buttons

        private void BtnTestCredential_Click(object sender, EventArgs e)
        {
            Computer tempComputer = new Computer(ComputerName, Username, Password);
            btnOk.Enabled = false;
            btnCancel.Enabled = false;
            btnTestCredential.Enabled = false;
            Refresh();

            if (tempComputer.IsCredentialOk())
            {
                btnTestCredential.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                btnTestCredential.BackColor = System.Drawing.Color.OrangeRed;
            }
            btnOk.Enabled = true;
            btnCancel.Enabled = true;
            btnTestCredential.Enabled = true;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.AdminUser = txtBxUsername.Text;
                Properties.Settings.Default.Save();
            }
            catch (Exception) { }

            DialogResult = DialogResult.OK;
        }

        private void BtnShowPassword_MouseDown(object sender, MouseEventArgs e)
        {
            txtBxPassword.UseSystemPasswordChar = false;
        }

        private void BtnShowPassword_MouseUp(object sender, MouseEventArgs e)
        {
            txtBxPassword.UseSystemPasswordChar = true;
        }

        private void BtnPing_Click(object sender, EventArgs e)
        {
            PingRemoteComputer();
        }

        // Textboxes

        private void TxtBxes_TextChanged(object sender, EventArgs e)
        {
            btnTestCredential.BackColor = System.Drawing.SystemColors.Control;

            if (String.IsNullOrEmpty(txtBxComputerName.Text))
            {
                btnPing.Image = Properties.Resources.Red16x16;
                toolTip1.SetToolTip(btnPing, _localization.GetLocalizedString("CantPing"));
            }
            else
            {
                if (!_chrono.Enabled)
                    _chrono.Start();
                else
                {
                    _chrono.Stop();
                    _chrono.Start();
                }
            }

            ValidateData();
        }

        private void TxtBxComputerName_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtBxComputerName.Text))
            {
                PingRemoteComputer();
            }
        }

        // Timer

        private void Chrono_Tick(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtBxComputerName.Text))
            {
                _chrono.Stop();
                PingRemoteComputer();
            }
        }

        #endregion (events)
    }
}
