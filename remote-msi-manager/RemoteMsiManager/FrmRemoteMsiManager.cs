using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace RemoteMsiManager
{
    public partial class FrmRemoteMsiManager : Form
    {
        private string _password = String.Empty;
        private readonly DataGridViewCellStyle _errorCell = new DataGridViewCellStyle();

        public FrmRemoteMsiManager(String[] args)
        {
            InitializeComponent();

            _errorCell.BackColor = Color.Goldenrod;
            _errorCell.SelectionBackColor = Color.Goldenrod;
            SetUIFromArgs(args);
            SetVersion();
        }

        #region (Methods)

        private void SetVersion()
        {
            Version currentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Text += " - V" + currentVersion.ToString();
        }

        private void SetUIFromArgs(string[] args)
        {
            string argComputers = String.Empty;
            string argUsername = String.Empty;
            string argPassword = String.Empty;

            try
            {
                foreach (string arg in args)
                {
                    string command = arg.Substring(0, 3).ToLower();
                    string value = arg.Substring(3);
                    switch (command)
                    {
                        case "-i=":
                        case "/i=":
                        case "-i:":
                        case "/i:":
                            txtBxPattern.Text = value;
                            break;
                        case "-x=":
                        case "/x=":
                        case "-x:":
                        case "/x:":
                            txtBxExceptions.Text = value;
                            break;
                        case "-c=":
                        case "/c=":
                        case "-c:":
                        case "/c:":
                            argComputers = value;
                            break;
                        case "-u=":
                        case "/u=":
                        case "-u:":
                        case "/u:":
                            argUsername = value;
                            break;
                        case "-p=":
                        case "/p=":
                        case "-p:":
                        case "/p:":
                            argPassword = value;
                            break;
                    }
                }
                if (!String.IsNullOrEmpty(argComputers))
                {
                    AddComputers(argComputers, argUsername, argPassword);
                }
            }
            catch (Exception) { }
        }

        private void AddComputers(string computers, string username, string password)
        {
            foreach (string computer in GetComputerList(computers))
            {
                try
                {
                    AddComputer(new Computer(computer, username, password));
                }
                catch (Exception) { }
            }
        }

        private List<String> GetComputerList(string computers)
        {
            List<String> computerList = new List<string>();

            try
            {
                string[] computerArray = computers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string computer in computerArray)
                {
                    computerList.Add(computer);
                }
            }
            catch (Exception) { }

            return computerList;
        }

        private void DisplayProductForComputer(Computer computer)
        {
            Action displayProductsAction = () =>
                {
                    try
                    {
                        dgvProducts.Rows.Clear();
                        if (!computer.ProductsRetrievalInProgress)
                        {
                            List<DataGridViewRow> rows = new List<DataGridViewRow>();

                            if (String.IsNullOrEmpty(txtBxPattern.Text))
                                txtBxPattern.Text = "%";

                            List<MsiProduct> displayedProducts = FilterInstalledProducts(computer.Products, txtBxPattern.Text, txtBxExceptions.Text);
                            foreach (MsiProduct product in displayedProducts)
                            {
                                DataGridViewRow row = new DataGridViewRow();
                                row.CreateCells(dgvProducts, new Object[] { product, product.IdentifyingNumber, product.Name, product.Version, MsiProduct.GetFormattedInstallDate(product.InstallDate) });
                                rows.Add(row);
                            }
                            dgvProducts.Rows.AddRange(rows.ToArray());
                            dgvProducts.ClearSelection();
                            UpdateProductCount(computer, displayedProducts.Count);
                            if (dgvProducts.SortedColumn != null)
                            { dgvProducts.Sort(dgvProducts.SortedColumn, dgvProducts.SortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending); }
                            else
                            { dgvProducts.Sort(dgvProducts.Columns["ProductName"], ListSortDirection.Ascending); }
                        }
                    }
                    catch (Exception) { }
                };
            Invoke(displayProductsAction);
        }

        private string GetAllSelectedMsiProductCodes()
        {
            string result = String.Empty;

            foreach (DataGridViewRow row in dgvProducts.SelectedRows)
            {
                result += row.Cells["identifyingNumber"].Value.ToString() + ";";
            }
            result = result.Substring(0, result.Length - 1);

            return result;
        }

        private void AddComputer(Computer computerToAdd)
        {
            int index = dgvComputers.Rows.Add();
            computerToAdd.ProductsRetrieved += computer_ProductsRetrieved;

            try
            {
                computerToAdd.GetCurrentLogonUsername();
            }
            catch (Exception ex)
            {

                dgvComputers.Rows[index].Cells["UserName"].Style = _errorCell;
                dgvComputers.Rows[index].Cells["UserName"].ToolTipText = ex.Message;
            }
            computerToAdd.RetrieveProductsAsynch();
            dgvComputers.Rows[index].Cells["Computer"].Value = computerToAdd;
            dgvComputers.Rows[index].Cells["ComputerName"].Value = computerToAdd.ComputerName;
            dgvComputers.Rows[index].Cells["UserName"].Value = computerToAdd.RemoteUsername;

            if (dgvComputers.Rows[index].Selected)
            {
                btnQueryComputer.Cursor = Cursors.No;
                btnQueryComputer.Enabled = true;
                btnQueryComputer.Image = Properties.Resources.HourglassAnimated;
            }
        }

        private List<MsiProduct> FilterInstalledProducts(List<MsiProduct> allInstalledProducts, string pattern, string exception)
        {
            List<MsiProduct> productsToDisplay = new List<MsiProduct>();
            List<string> productToFind = MsiProduct.SplitMsiProductCodes(pattern);
            List<string> exceptions = MsiProduct.SplitMsiProductCodes(exception);

            foreach (MsiProduct installedProduct in allInstalledProducts)
            {
                foreach (string product in productToFind)
                {
                    if (MsiProduct.PatternMatchMsiCode(installedProduct.IdentifyingNumber, product))
                    {
                        bool displayIt = true;
                        foreach (string currentException in exceptions)
                        {
                            if (MsiProduct.PatternMatchMsiCode(installedProduct.IdentifyingNumber, currentException))
                            {
                                displayIt = false;
                                break;
                            }
                        }
                        if (displayIt)
                        {
                            productsToDisplay.Add(installedProduct);
                            break;
                        }
                    }
                }
            }

            return productsToDisplay;
        }

        private void RemoveUninstalledProducts(List<MsiProduct> uninstalledProducts)
        {
            Computer targetComputer = (Computer)dgvComputers.SelectedRows[0].Cells["Computer"].Value;

            foreach (MsiProduct uninstalledProduct in uninstalledProducts)
            {
                try
                {
                    RemoveProduct(uninstalledProduct);
                }
                catch (Exception) { }
            }
            UpdateProductCount(targetComputer, dgvProducts.Rows.Count);
        }

        private void RemoveProduct(MsiProduct productToRemove)
        {
            DataGridViewRow rowToRemove = null;

            foreach (DataGridViewRow row in dgvProducts.Rows)
            {
                if ((row.Cells["Product"].Value as MsiProduct) == productToRemove)
                {
                    rowToRemove = row;
                    break;
                }
            }
            if (rowToRemove != null)
                dgvProducts.Rows.Remove(rowToRemove);
        }

        private void ShowProductDetails(int rowIndex)
        {
            try
            {
                MsiProduct selectedProduct = (MsiProduct)dgvProducts.Rows[rowIndex].Cells["Product"].Value;
                FrmProductProperties properties = new FrmProductProperties(selectedProduct);
                properties.ShowDialog();
            }
            catch (Exception) { }
        }

        private void UnintallSelectedProducts()
        {
            try
            {
                if (dgvComputers.SelectedRows != null && dgvComputers.SelectedRows.Count == 1)
                {
                    Computer targetComputer = (Computer)dgvComputers.SelectedRows[0].Cells["Computer"].Value;

                    if (dgvProducts.SelectedRows != null && dgvProducts.SelectedRows.Count > 0)
                    {
                        List<MsiProduct> productToUninstall = new List<MsiProduct>();

                        foreach (DataGridViewRow row in dgvProducts.SelectedRows)
                        {
                            productToUninstall.Add((MsiProduct)row.Cells["Product"].Value);
                        }
                        FrmUninstallProduct frmUninstall = new FrmUninstallProduct(targetComputer, productToUninstall);
                        frmUninstall.ShowDialog();
                        RemoveUninstalledProducts(frmUninstall.UninstalledProducts);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateProductCount(Computer computer, int displayedProductCount)
        {
            try
            {
                foreach (DataGridViewRow row in dgvComputers.Rows)
                {
                    Computer tempComputer = (Computer)row.Cells["Computer"].Value;

                    if (tempComputer == computer)
                    {
                        row.Cells["Total"].Value = tempComputer.Products.Count;
                        if (tempComputer.LastErrorThrown != null)
                        {
                            row.Cells["Total"].Style = _errorCell;
                            row.Cells["Total"].ToolTipText = tempComputer.LastErrorThrown.Message;
                        }
                        else
                        {
                            row.Cells["Total"].Style = row.InheritedStyle;
                            row.Cells["Total"].ToolTipText = String.Empty;
                        }
                        row.Cells["Displayed"].Value = displayedProductCount;
                        break;
                    }
                }
            }
            catch (Exception) { }
        }

        private void ReportShowDetails()
        {
            try
            {
                var productName = dgvProducts.SelectedRows[0].Cells["ProductName"].Value.ToString();
                var productVersion = dgvProducts.SelectedRows[0].Cells["Version"].Value.ToString();
            }
            catch (Exception) { }
        }

        #endregion (Methods)

        #region (Events)

        // Button

        private void btnQueryComputer_Click(object sender, EventArgs e)
        {
            try
            {
                Computer computerToQuery = null;
                if (dgvComputers.SelectedRows != null && dgvComputers.SelectedRows.Count == 1)
                {
                    computerToQuery = (Computer)dgvComputers.SelectedRows[0].Cells["Computer"].Value;
                    dgvComputers.SelectedRows[0].Cells["Total"].Value = String.Empty;
                    dgvComputers.SelectedRows[0].Cells["Displayed"].Value = String.Empty;
                }
                try
                {
                    if (computerToQuery != null && !computerToQuery.ProductsRetrievalInProgress)
                    {
                        btnQueryComputer.Image = Properties.Resources.HourglassAnimated;
                        btnQueryComputer.Cursor = Cursors.No;
                        dgvProducts.Rows.Clear();
                        computerToQuery.ProductsRetrieved += computer_ProductsRetrieved;
                        computerToQuery.RetrieveProductsAsynch();
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnUninstall_Click(object sender, EventArgs e)
        {
            UnintallSelectedProducts();
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            FrmInstallProduct frmInstallProduct = new FrmInstallProduct((Computer)dgvComputers.SelectedRows[0].Cells["Computer"].Value);
            frmInstallProduct.ShowDialog();
        }

        private void btnAddRemoteComputer_Click(object sender, EventArgs e)
        {
            FrmAddRemoteComputer addRemoteComputer = new FrmAddRemoteComputer
            {
                Password = _password
            };

            if (addRemoteComputer.ShowDialog() == DialogResult.OK)
            {
                AddComputer(new Computer(addRemoteComputer.ComputerName, addRemoteComputer.Username, addRemoteComputer.Password));
                _password = addRemoteComputer.Password;
            }
        }

        private void btnRemoveComputers_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvComputers.SelectedRows != null && dgvComputers.SelectedRows.Count != 0)
                {
                    DataGridViewRow[] selectedRows = new DataGridViewRow[dgvComputers.SelectedRows.Count];
                    dgvComputers.SelectedRows.CopyTo(selectedRows, 0);
                    foreach (DataGridViewRow row in selectedRows)
                    {
                        if ((row.Cells["Computer"].Value as Computer).ComputerLocation == RemoteMsiManager.Computer.ComputerLocations.Remote)
                        { dgvComputers.Rows.Remove(row); }
                    }
                }
            }
            catch (Exception) { }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnDeletePatternFilter_Click(object sender, EventArgs e)
        {
            txtBxPattern.Text = "%";
        }

        private void btnDeleteExceptionFilter_Click(object sender, EventArgs e)
        {
            txtBxExceptions.Text = String.Empty;
        }

        private void btnRefreshFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvComputers.SelectedRows != null && dgvComputers.SelectedRows.Count == 1)
                {
                    Computer selectedComputer = (Computer)dgvComputers.SelectedRows[0].Cells["Computer"].Value;
                    DisplayProductForComputer(selectedComputer);
                }
            }
            catch (Exception) { }
        }

        // DataGridView Computers

        private void computer_ProductsRetrieved(Computer computer)
        {
            computer.ProductsRetrieved -= computer_ProductsRetrieved;
            if (dgvComputers.SelectedRows != null && dgvComputers.SelectedRows.Count == 1 && (dgvComputers.SelectedRows[0].Cells["Computer"].Value as Computer) == computer)
            {
                DisplayProductForComputer(computer);

                Action btnAction = () =>
                {
                    btnQueryComputer.Image = Properties.Resources.Search24x24;
                    btnQueryComputer.Cursor = Cursors.Default;
                    btnQueryComputer.Enabled = true;
                };
                Invoke(btnAction);
            }
            else
            { UpdateProductCount(computer, 0); }
        }

        private void dgvComputers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && dgvComputers.SelectedRows != null && dgvComputers.SelectedRows.Count == 1)
            {
                Computer selectedComputer = (Computer)dgvComputers.SelectedRows[0].Cells["Computer"].Value;
                if (selectedComputer.ComputerLocation == RemoteMsiManager.Computer.ComputerLocations.Remote)
                {
                    FrmAddRemoteComputer frmAddComputer = new FrmAddRemoteComputer(selectedComputer.ComputerName, selectedComputer.Username);
                    if (frmAddComputer.ShowDialog() == DialogResult.OK)
                    {
                        selectedComputer.Username = frmAddComputer.Username;
                        selectedComputer.Password = frmAddComputer.Password;
                    }
                }
            }
        }

        private void dgvComputers_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvComputers.SelectedRows != null)
                {
                    if (dgvComputers.SelectedRows.Count == 1)
                    {
                        if (dgvComputers.SelectedRows[0].Cells["Computer"].Value != null)
                        {
                            Computer selectedComputer = (Computer)dgvComputers.SelectedRows[0].Cells["Computer"].Value;
                            btnRemoveComputers.Enabled = selectedComputer.ComputerLocation == RemoteMsiManager.Computer.ComputerLocations.Remote;
                            btnInstallProduct.Enabled = true;

                            if (selectedComputer.ProductsRetrievalInProgress)
                            {
                                btnQueryComputer.Cursor = Cursors.No;
                                btnQueryComputer.Image = Properties.Resources.HourglassAnimated;
                            }
                            else
                            {
                                btnQueryComputer.Cursor = Cursors.Default;
                                btnQueryComputer.Image = Properties.Resources.Search24x24;
                                btnQueryComputer.Enabled = true;
                            }
                            DisplayProductForComputer(selectedComputer);
                        }
                        else
                            btnRemoveComputers.Enabled = false;
                    }
                    else
                    {
                        dgvProducts.Rows.Clear();
                        btnQueryComputer.Cursor = Cursors.No;
                        btnRemoveComputers.Enabled = (dgvComputers.SelectedRows.Count > 0);
                        btnInstallProduct.Enabled = false;
                    }
                }
                else
                {
                    dgvProducts.Rows.Clear();
                    btnQueryComputer.Cursor = Cursors.No;
                    btnRemoveComputers.Enabled = false;
                }
            }
            catch (Exception) { }
        }

        // DataGridView Products

        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                btnUninstall.Enabled = (dgvProducts.SelectedRows != null && dgvProducts.SelectedRows.Count > 0);
            }
            catch (Exception) { }
        }

        private void dgvProducts_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column == dgvProducts.Columns["InstallDate"])
            {
                string date1 = e.CellValue1.ToString();
                string date2 = e.CellValue2.ToString();
                date1 = date1.Substring(6) + date1.Substring(3, 2) + date1.Substring(0, 2);
                date2 = date2.Substring(6) + date2.Substring(3, 2) + date2.Substring(0, 2);
                e.SortResult = String.Compare(date1, date2);
                e.Handled = true;
            }
            else if (e.Column == dgvProducts.Columns["Version"])
            {
                string version1 = e.CellValue1.ToString();
                string version2 = e.CellValue2.ToString();

                version1 = MsiProduct.GetConcatenatedVersion(version1);
                version2 = MsiProduct.GetConcatenatedVersion(version2);
                e.SortResult = String.Compare(version1, version2);
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void dgvProducts_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                if (dgvProducts.SelectedRows != null)
                {
                    if (dgvProducts.SelectedRows.Count == 0 || !dgvProducts.SelectedRows.Contains(dgvProducts.Rows[e.RowIndex]))
                    {
                        if ((ModifierKeys & Keys.Control) != Keys.Control)
                        { dgvProducts.ClearSelection(); }
                        dgvProducts.Rows[e.RowIndex].Selected = true;
                    }

                    int selectionCount = dgvProducts.SelectedRows.Count;
                    for (int i = 0; i < ctxMnuProducts.Items.Count; i++)
                    {
                        ctxMnuProducts.Items[i].Enabled = true;
                    }
                    if (selectionCount > 1)
                    {
                        ctxMnuProducts.Items[0].Enabled = false;
                        ctxMnuProducts.Items[1].Enabled = false;
                        ctxMnuProducts.Items[2].Enabled = false;
                        ctxMnuProducts.Items[3].Enabled = false;
                    }

                    ctxMnuProducts.Show(Cursor.Position);
                }
            }
        }

        private void dgvProducts_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvProducts.SelectedRows != null && dgvProducts.SelectedRows.Count == 1)
                {
                    ReportShowDetails();
                    ShowProductDetails(dgvProducts.SelectedRows[0].Index);
                }
            }
            catch (Exception) { }
        }

        // Form

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            AddComputer(new Computer(Environment.MachineName));
        }

        // ToolStripItem Click

        private void tlStrShowDetails_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProducts.SelectedRows != null && dgvProducts.SelectedRows.Count == 1)
                {
                    ReportShowDetails();
                    ShowProductDetails(dgvProducts.SelectedRows[0].Index);
                }
            }
            catch (Exception) { }
        }

        private void tlStrSetAsPattern_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProducts.SelectedRows != null && dgvProducts.SelectedRows.Count > 0)
                {
                    txtBxPattern.Text = GetAllSelectedMsiProductCodes();
                }
            }
            catch (Exception) { }
        }

        private void tlStrSetAsException_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProducts.SelectedRows != null && dgvProducts.SelectedRows.Count > 0)
                {
                    txtBxExceptions.Text = GetAllSelectedMsiProductCodes();
                }
            }
            catch (Exception) { }
        }

        private void tlStrUninstallProducts_Click(object sender, EventArgs e)
        {
            UnintallSelectedProducts();
        }

        private void tlStrCopyEntireLine_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProducts.SelectedRows != null && dgvProducts.SelectedRows.Count == 1)
                {
                    string content = String.Empty;

                    DataGridViewRow selectedRow = dgvProducts.SelectedRows[0];
                    foreach (DataGridViewCell cell in selectedRow.Cells)
                    {
                        if (cell.Visible)
                        { content += cell.Value.ToString() + ";"; }
                    }
                    content = content.Substring(0, content.Length - 1);
                    Clipboard.Clear();
                    Clipboard.SetText(content);
                }
            }
            catch (Exception) { }
        }

        private void tlStrCopyMsiProductCode_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProducts.SelectedRows != null && dgvProducts.SelectedRows.Count == 1)
                {
                    Clipboard.Clear();
                    Clipboard.SetText(dgvProducts.SelectedRows[0].Cells["identifyingNumber"].Value.ToString());
                }
            }
            catch (Exception) { }
        }

        // TextBox Change

        private void txtBxPattern_TextChanged(object sender, EventArgs e)
        {
            int carretPosition = txtBxPattern.SelectionStart;
            int textLength = txtBxPattern.TextLength;
            txtBxPattern.Text = MsiProduct.RemoveUnvantedCharacters(txtBxPattern.Text);
            carretPosition -= textLength - txtBxPattern.TextLength;
            txtBxPattern.SelectionStart = System.Math.Max(0, System.Math.Min(carretPosition, txtBxPattern.TextLength));
        }

        private void txtBxExceptions_TextChanged(object sender, EventArgs e)
        {
            int carretPosition = txtBxExceptions.SelectionStart;
            int textLength = txtBxExceptions.TextLength;
            txtBxExceptions.Text = MsiProduct.RemoveUnvantedCharacters(txtBxExceptions.Text);
            carretPosition -= textLength - txtBxExceptions.TextLength;
            txtBxExceptions.SelectionStart = System.Math.Max(0, System.Math.Min(carretPosition, txtBxExceptions.TextLength));
        }

        #endregion (Events)
    }
}
