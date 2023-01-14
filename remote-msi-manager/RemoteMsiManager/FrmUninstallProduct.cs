using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RemoteMsiManager
{
    internal partial class FrmUninstallProduct : Form
    {
        private readonly DataGridViewCellStyle _successStyle = new DataGridViewCellStyle();
        private readonly DataGridViewCellStyle _successRebootStyle = new DataGridViewCellStyle();
        private readonly DataGridViewCellStyle _failedStyle = new DataGridViewCellStyle();
        private readonly Localization _localization = Localization.GetInstance();

        internal FrmUninstallProduct(Computer targetComputer, List<MsiProduct> targetProducts)
        {
            InitializeComponent();

            _successStyle.BackColor = Color.LawnGreen;
            _successRebootStyle.BackColor = Color.Orange;
            _failedStyle.BackColor = Color.Red;

            TargetComputer = targetComputer;
            txtBxComputer.Text = targetComputer.ComputerName;
            foreach (MsiProduct msiProduct in targetProducts)
            {
                int index = dgvProducts.Rows.Add();
                dgvProducts.Rows[index].Cells["Product"].Value = msiProduct;
                dgvProducts.Rows[index].Cells["ProductName"].Value = msiProduct.Name;
                dgvProducts.Rows[index].Cells["Version"].Value = msiProduct.Version;
                dgvProducts.Rows[index].Cells["InstallDate"].Value = MsiProduct.GetFormattedInstallDate(msiProduct.InstallDate);
                dgvProducts.Rows[index].Cells["Result"].Value = String.Empty;
                dgvProducts.Rows[index].Cells["ResultCode"].Value = String.Empty;
            }
        }

        private Computer TargetComputer { get; set; }

        internal List<MsiProduct> UninstalledProducts { get; } = new List<MsiProduct>();

        private void UninstallProducts()
        {
            foreach (DataGridViewRow row in dgvProducts.Rows)
            {
                try
                {
                    uint resultCode = 0;
                    if (!uint.TryParse(row.Cells["ResultCode"].Value.ToString(), out resultCode) || !MsiProduct.IsSuccess(resultCode))
                    {
                        MsiProduct currentProduct = (MsiProduct)row.Cells["Product"].Value;
                        row.Cells["Result"].Value = _localization.GetLocalizedString("InProgress");
                        uint result = TargetComputer.UninstallProduct(currentProduct.IdentifyingNumber);

                        ReportUninstallResult(result, row, currentProduct);
                    }
                }
                catch (Exception ex)
                {

                    txtBxMessage.Text = _localization.GetLocalizedString("Failed") + " : " + ex.Message;
                    txtBxMessage.BackColor = Color.Orange;
                }
            }
        }

        private void ReportUninstallResult(uint resultCode, DataGridViewRow row, MsiProduct uninstalledProduct)
        {
            row.Cells["ResultCode"].Value = resultCode;

            if (MsiProduct.IsSuccess(resultCode))
            {
                txtBxMessage.BackColor = Color.LightGreen;
                if (MsiProduct.IsRebootNeeded(resultCode))
                {
                    row.Cells["Result"].Value = _localization.GetLocalizedString("SuccessPendingReboot");
                    row.Cells["Result"].Style = _successRebootStyle;
                    txtBxMessage.Text = _localization.GetLocalizedString("SuccessPendingReboot");
                    if (!UninstalledProducts.Contains(uninstalledProduct))
                    { UninstalledProducts.Add(uninstalledProduct); }
                }
                else
                {
                    row.Cells["Result"].Value = _localization.GetLocalizedString("Success");
                    row.Cells["Result"].Style = _successStyle;
                    txtBxMessage.Text = _localization.GetLocalizedString("Success");
                    if (!UninstalledProducts.Contains(uninstalledProduct))
                    { UninstalledProducts.Add(uninstalledProduct); }
                }
            }
            else
            {
                txtBxMessage.BackColor = Color.Orange;
                row.Cells["Result"].Value = _localization.GetLocalizedString("Failed") + "(" + resultCode + ")";
                row.Cells["Result"].Style = _failedStyle;
                txtBxMessage.Text = MsiProduct.GetErrorMessage(resultCode);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnUninstall_Click(object sender, EventArgs e)
        {
            btnClose.Enabled = false;
            btnUninstall.Enabled = false;
            UninstallProducts();
            btnClose.Enabled = true;
            btnUninstall.Enabled = true;
        }

        private void dgvProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    MsiProduct selectedProduct = (MsiProduct)dgvProducts.Rows[e.RowIndex].Cells["Product"].Value;
                    FrmProductProperties properties = new FrmProductProperties(selectedProduct);
                    properties.ShowDialog();
                }
            }
            catch (Exception) { }
        }

        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            txtBxMessage.Text = String.Empty;
            if (dgvProducts.SelectedRows != null && dgvProducts.SelectedRows.Count == 1)
            {
                uint errorCode;
                if (uint.TryParse(dgvProducts.SelectedRows[0].Cells["ResultCode"].Value.ToString(), out errorCode))
                {
                    txtBxMessage.Text = MsiProduct.GetErrorMessage(errorCode);
                }
            }
        }
    }
}
