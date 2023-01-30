using System;
using System.Collections;
using System.Data;
using System.Management;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using Microsoft.ConfigurationManagement.AdminConsole.AppManFoundation;
using Microsoft.ConfigurationManagement.ApplicationManagement;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine;
using System.Diagnostics;
using System.Linq;
using Microsoft.Deployment.WindowsInstaller;
using Microsoft.Deployment.WindowsInstaller.Linq;
using Microsoft.ConfigurationManagement.DesiredConfigurationManagement;
using Microsoft.SystemsManagementServer.DesiredConfigurationManagement.Expressions;
using Microsoft.ConfigurationManagement.DesiredConfigurationManagement.ExpressionOperators;
using Rule = Microsoft.SystemsManagementServer.DesiredConfigurationManagement.Rules.Rule;
using NoncomplianceSeverity = Microsoft.SystemsManagementServer.DesiredConfigurationManagement.Rules.NoncomplianceSeverity;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net.Http;
using System.Net;

namespace CM_App_Creator
{
    // Author: Christoffer Bennerstedt - email bennerstedt77@gmail.com - official repo: https://github.com/CBennerstedt/CMAppCreator
    public partial class Default : Page

    {
        private static AppManWrapper wrapper;
        private static ApplicationFactory factory;

        public ConstantValue Constant { get; set; }

        public string httpusernameformatted = HttpContext.Current.User.Identity.Name.Split('\\')[1]; // we're just assuming the app has been correctly configured in IIS with windows authentication so we split on domain\username..

        private readonly string _siteServer = Properties.Settings.Default.SiteServer;
        private readonly string _dpGroupName = Properties.Settings.Default.DPGroupName;
        private readonly string _siteCode = Properties.Settings.Default.SiteCode;
        private readonly string _startingDir = Properties.Settings.Default.UNCPath.ToLower();
        private readonly string _folderNameDetection = Properties.Settings.Default.FolderNameDetection.ToLower();
        private readonly string _CMAppFolder = Properties.Settings.Default.CMAppFolder;
        private readonly string _securityScope = Properties.Settings.Default.SecurityScope;
        private readonly string _collectionPrefix = Properties.Settings.Default.CollectionPrefix;
        private readonly string _defaultLanguage = Properties.Settings.Default.DefaultLanguage;
        private readonly bool _DisplayContactAndExecutionTime = Properties.Settings.Default.DisplayContactAndExecutionTime;
        public readonly bool _AddBranding = Properties.Settings.Default.AddBranding;
        private readonly string _BrandingText = Properties.Settings.Default.BrandingText;

        private static byte[] icon2bytes;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                // Handle GET Requests
                InitializeControls();
                if (TestConnection(_siteServer))
                {
                    QueryCollections();
                    QueryUserCategories();
                    QueryAppCategories();
                }
            }
            else
            {
                //  Handle POST Requests
            }

        }

        /// <summary>
        /// Put all controls to be initialized on page load here..
        /// </summary>
        private void InitializeControls()
        {
            CreateCMApp.Attributes.Add("data-toggle", "tooltip");
            CreateCMApp.Attributes.Add("data-placement", "bottom");
            CreateCMApp.Attributes.Add("data-delay", "500");
            CreateCMApp.Enabled = false;
            if (_DisplayContactAndExecutionTime)
            {
                divContactAndExectutionTimeInputs.Visible = true;
            }
            // Below 4 are tied to the div named divContactAndExecutionTimeInputs, and can be modified to be shown or not in web.config AppSetting _DisplayContactAndExecutionTime
            appowner.Value = httpusernameformatted;     // Initial value
            appcontact.Value = httpusernameformatted;   // Initial value
            maxexecutiontime.Value = "120";             // Initial value
            estimatedexecutiontime.Value = "60";        // Initial value. Will be modified later in code when calculating based on disk requirement rule

            lblCurrentDir.InnerText = _startingDir;
            FolderParent.Visible = false;
            SetContentFolder.Visible = false;
            List<string> folders = ShowDirectoriesIn(_startingDir);
            ListDirs.DataSource = folders;
            ListDirs.DataBind();

            if (folders.Count > 0)
            {
                ShowFilesIn(_startingDir);
            }
        }

        // Initializes the default authoring scope and establishes connection to the SMS Provider.  
        // <param name="_siteServerName">A string containing the name of the Configuration Manager site.</param>  
        private bool TestConnection(string _siteServerName)
        {
            bool results = false;
            Validator.CheckForNull(_siteServerName, "_siteServerName");

            // Initialize impersonation
            System.Security.Principal.WindowsImpersonationContext impersonationContext;

            try
            {
                SmsProvider smsProvider = new SmsProvider();
                // Impersonate
                impersonationContext =
                    ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                WqlConnectionManager connectionManager = smsProvider.Connect(_siteServerName);
                impersonationContext.Undo();
                if (connectionManager !=null)
                {
                    connectionManager.Dispose();
                    results = true;
                }
            }
            catch (Exception ex)
            {
                alertError.Visible = true;
                contentdiv.Visible = false;
                spanError.InnerText = String.Format("An error occured when attempting to connect to SMS Provider. Error message: {0}", ex.Message);
            }
            return results;
        }
        private void QueryCollections()
        {
            SortedDictionary<string, string> collections = GetCollectionsbyType(1, _collectionPrefix);

            if (collections != null)
            {
                foreach (KeyValuePair<string, string> kvp in collections)
                {
                    dropdowncollections.Items.Add(kvp.Value);
                }
            }
        }
        private SortedDictionary<string, string> GetCollectionsbyType(int CollectionType, string _collectionPrefix)
        {
            bool erroroccurred = false;
            SortedDictionary<string, string> collectiondictionary = new SortedDictionary<string, string>();

            // Initialize impersonation
            System.Security.Principal.WindowsImpersonationContext impersonationContext;

            //' Query for device relationship instances
            SelectQuery relationQuery = new SelectQuery(string.Format("select * from SMS_Collection where CollectionType='{0}' and Name like '{1}%'", CollectionType, _collectionPrefix));
            ManagementScope managementScope = new ManagementScope("\\\\" + _siteServer + "\\root\\SMS\\site_" + _siteCode);

            try
            {
                // Impersonate
                impersonationContext =
                    ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                managementScope.Connect();
                impersonationContext.Undo();
            }
            catch (Exception ex)
            {
                alertError.Visible = true;
                contentdiv.Visible = false;
                spanError.InnerText = String.Format("An error occured when attempting to connect to SMS Provider. Error message: {0}", ex.Message);
                erroroccurred = true;
            }

            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(managementScope, relationQuery);
            managementObjectSearcher.Dispose();
            if (managementObjectSearcher != null && !erroroccurred)
            {
                try
                {
                    impersonationContext =
                    ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                    foreach (var appreference in managementObjectSearcher.Get())
                    {
                        //parameters.Add("ObjectPath", (string)appreference.GetPropertyValue("ObjectPath"));
                        //parameters.Add("ModelName", (string)appreference.GetPropertyValue("ModelName"));
                        collectiondictionary.Add((string)appreference.GetPropertyValue("Name"), (string)appreference.GetPropertyValue("Name"));
                        //return parameters;
                    }
                    impersonationContext.Undo();
                }
                catch (Exception ex)
                {
                    Log(String.Format("An error occured when attempting to connect to SMS Provider. Error message: {0}", ex.Message));
                }
                return collectiondictionary;
            }
            else
            {
                return null;
            }
        }

        private void QueryUserCategories()
        {
            SortedDictionary<string, string> categories = null;
            try
            {
                categories = DictGetUserCategories();
            }
            catch (Exception ex)
            {
                alertError.Visible = true;
                contentdiv.Visible = false;
                spanError.InnerText = String.Format("An error occured when attempting to connect to SMS Provider. Error message: {0}", ex.Message);
            }

            if (categories.Count > 0)
            {
                foreach (KeyValuePair<string, string> kvp in categories)
                {
                    dropdownusercategories.Items.Add(new ListItem(kvp.Key, kvp.Value));
                }
                dropdownusercategories.Rows = categories.Count;
            }
        }
        public SortedDictionary<string, string> DictGetUserCategories()
        {
            SortedDictionary<string, string> CategoriesDictionary = new SortedDictionary<string, string>();

            bool erroroccurred = false;

            // Initialize impersonationcontext
            System.Security.Principal.WindowsImpersonationContext impersonationContext;

            //' Query for user categories. Ref: https://docs.microsoft.com/en-us/mem/configmgr/develop/reference/compliance/sms_categoryinstance-server-wmi-class
            SelectQuery relationQuery = new SelectQuery("select LocalizedCategoryInstanceName,CategoryInstance_UniqueID from SMS_CategoryInstance where CategoryTypeName='CatalogCategories'");
            ManagementScope managementScope = new ManagementScope("\\\\" + _siteServer + "\\root\\SMS\\site_" + _siteCode);
            try
            {
                // Impersonate
                impersonationContext =
                    ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                managementScope.Connect();
                impersonationContext.Undo();

            }
            catch (Exception ex)
            {
                alertError.Visible = true;
                contentdiv.Visible = false;
                spanError.InnerText = String.Format("An error occured when attempting to connect to SMS Provider. Error message: {0}", ex.Message);
                erroroccurred = true;
            }

            // Impersonate
            impersonationContext = ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(managementScope, relationQuery);
            impersonationContext.Undo();

            if (managementObjectSearcher != null && !erroroccurred)
            {
                // Impersonate
                impersonationContext = ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                foreach (var appreference in managementObjectSearcher.Get())
                {
                    CategoriesDictionary.Add((string)appreference.GetPropertyValue("LocalizedCategoryInstanceName"), (string)appreference.GetPropertyValue("CategoryInstance_UniqueID"));
                }
                impersonationContext.Undo();
                managementObjectSearcher.Dispose();
                return CategoriesDictionary;
            }
            else
            {
                managementObjectSearcher.Dispose();
                return null;
            }
        }

        private void QueryAppCategories()
        {
            SortedDictionary<string, string> categories = DictGetAppCategories();

            if (categories.Count > 0)
            {
                foreach (KeyValuePair<string, string> kvp in categories)
                {
                    dropdownappcategories.Items.Add(new ListItem(kvp.Key, kvp.Value));
                }
                dropdownappcategories.Rows = categories.Count;
            }
        }

        public SortedDictionary<string, string> DictGetAppCategories()
        {
            SortedDictionary<string, string> CategoriesDictionary = new SortedDictionary<string, string>();

            bool erroroccurred = false;
            // Initialize impersonationcontext
            System.Security.Principal.WindowsImpersonationContext impersonationContext;

            //' Query for user categories. Ref: https://docs.microsoft.com/en-us/mem/configmgr/develop/reference/compliance/sms_categoryinstance-server-wmi-class
            SelectQuery relationQuery = new SelectQuery("select LocalizedCategoryInstanceName,CategoryInstance_UniqueID from SMS_CategoryInstance where CategoryTypeName='AppCategories'");
            ManagementScope managementScope = new ManagementScope("\\\\" + _siteServer + "\\root\\SMS\\site_" + _siteCode);
            try
            {
                // Impersonate
                impersonationContext = ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                managementScope.Connect();
                impersonationContext.Undo();

            }
            catch (Exception ex)
            {
                alertError.Visible = true;
                contentdiv.Visible = false;
                spanError.InnerText = String.Format("An error occured when attempting to connect to SMS Provider. Error message: {0}", ex.Message);
                erroroccurred = true;
            }

            // Impersonate
            impersonationContext = ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(managementScope, relationQuery);
            impersonationContext.Undo();

            if (managementObjectSearcher != null && !erroroccurred)
            {
                // Impersonate
                impersonationContext = ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                foreach (var appreference in managementObjectSearcher.Get())
                {
                    CategoriesDictionary.Add((string)appreference.GetPropertyValue("LocalizedCategoryInstanceName"), (string)appreference.GetPropertyValue("CategoryInstance_UniqueID"));
                }
                impersonationContext.Undo();

                managementObjectSearcher.Dispose();
                return CategoriesDictionary;
            }
            else
            {
                managementObjectSearcher.Dispose();
                return null;
            }
        }

        private void ShowFilesIn(string dir)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            var rootDir = new DirectoryInfo(_startingDir).Name;

            ListFiles.Items.Clear();

            // only list files if not in root UNC directory
            if (!dirInfo.Name.Equals(rootDir))
            {
                int icoextensioncount = 0;
                int pngextensioncount = 0;

                foreach (FileInfo fileItem in dirInfo.GetFiles().OrderBy(fi => fi.FullName))
                {
                    string FileExtension = fileItem.Extension.ToLower();

                    if (FileExtension.Equals(".exe") | FileExtension.Equals(".msi") | FileExtension.Equals(".ico") | FileExtension.Equals(".png"))
                    {
                        ListFiles.Items.Add(fileItem.Name);
                    }
                    if (FileExtension.Equals(".ico"))
                    {
                        // automatically extract file data from first .ico file if several are found. User can still click manually on another file to import.
                        if (icoextensioncount.Equals(0))
                        {
                            GetFileData(fileItem.FullName);
                        }
                        ++icoextensioncount;
                    }
                    // we prefer .ico over .png if both are found in same directory (user can still select a .png file manually to import icon)
                    if (icoextensioncount < 1 && FileExtension.Equals(".png"))
                    {
                        // automatically extract file data from first .png file if several are found. User can still click manually on another file to import.
                        if (pngextensioncount.Equals(0))
                        {
                            GetFileData(fileItem.FullName);
                        }
                        ++pngextensioncount;
                    }

                }
            }

        }

        private static long GetDirectorySize(string folderPath)
        {
            DirectoryInfo di = new DirectoryInfo(folderPath);
            return di.EnumerateFiles("*", SearchOption.AllDirectories).Sum(fi => fi.Length);
        }
        private bool ShowDirectoriesInOld(string dir)
        {
            List<string> myList = new List<string> {};

            bool results = false;
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            try
            {
                var dirs = dirInfo.EnumerateDirectories().OrderBy(d => d.Name);
                ListDirs.Items.Clear();
                foreach (DirectoryInfo dirItem in dirs)
                {
                    string d = dirItem.Name;
                    if (!(d.StartsWith(".") | d.StartsWith("_")))
                    {
                        //lstDirs.Items.Add(dirItem.Name);
                        myList.Add(dirItem.Name);
                    }
                }

                ListDirs.DataSource = myList;
                ListDirs.DataBind();
                results = true;

            }
            catch (Exception ex)
            {
                alertError.Visible = true;
                contentdiv.Visible = false;
                spanError.InnerHtml = String.Format("An error occured when attempting to access UNC Path {1}.<br>Error message: {0}", ex.Message, dir);
            }
            return results;
        }

        private List<string> ShowDirectoriesIn(string dir)
        {
            List<string> myList = new List<string> { };

            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            try
            {
                var dirs = dirInfo.EnumerateDirectories().OrderBy(d => d.Name);
                ListDirs.Items.Clear();
                foreach (DirectoryInfo dirItem in dirs)
                {
                    string d = dirItem.Name;
                    if (!(d.StartsWith(".") | d.StartsWith("_")))
                    {
                        //lstDirs.Items.Add(dirItem.Name);
                        myList.Add(dirItem.Name);
                    }
                }

                ListDirs.DataSource = myList;
                ListDirs.DataBind();

            }
            catch (Exception ex)
            {
                alertError.Visible = true;
                contentdiv.Visible = false;
                spanError.InnerHtml = String.Format("An error occured when attempting to access UNC Path {1}.<br>Error message: {0}", ex.Message, dir);
            }
            return myList;
        }

        protected void ShowInfo_Click(object sender, EventArgs e)
        {
            if (ListFiles.SelectedIndex != -1)
            {
                fileinfo.Visible = true;

                string fileName = Path.Combine(lblCurrentDir.InnerText, ListFiles.SelectedItem.Text);
                FileInfo selFile = new FileInfo(fileName);
                FileVersionInfo selFileVersionInfo =
                    FileVersionInfo.GetVersionInfo(fileName);

                string textData = "<b>" + selFile.Name + "</b><br>";
                textData += "Size: " + selFile.Length + "<br>";
                textData += "Created: ";
                textData += selFile.CreationTime.ToString();
                textData += "<br>Last Accessed: ";
                textData += selFile.LastAccessTime.ToString();

                lblFileInfo.InnerHtml = textData;
            }

        }

        protected void FolderParent_Click(object sender, EventArgs e)
        {
            if (Directory.GetParent(lblCurrentDir.InnerText) != null)
            {
                string newDir = Directory.GetParent(lblCurrentDir.InnerText).FullName;
                string newDirLCase = newDir.ToLower();
                if (newDir.Equals(_startingDir))
                {
                    FolderParent.Visible = false;
                    SetContentFolder.Visible = false;
                }
                if (!string.IsNullOrEmpty(_folderNameDetection) && newDirLCase.Contains(_folderNameDetection))
                {
                    SetContentFolder.Visible = true;
                }
                else if (newDirLCase == _startingDir)
                {
                    FolderParent.Visible = false;
                    SetContentFolder.Visible = false;
                }
                else
                {
                    FolderParent.Visible = true;
                    SetContentFolder.Visible = false;
                }
                lblCurrentDir.InnerText = newDir;
                //bool listFolder = ShowDirectoriesIn(newDir);
                //if (listFolder)
                //{
                //    ShowFilesIn(newDir);
                //}
                List<string> folders = ShowDirectoriesIn(newDir);
                ListDirs.DataSource = folders;
                ListDirs.DataBind();
                if (folders != null)
                {
                    ShowFilesIn(newDir);
                }
                msiinfo.Visible = false;
                fileinfo.Visible = false;
            }
        }

        protected void ListDirs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListDirs.SelectedIndex != -1)
            {
                string newDir = Path.Combine(lblCurrentDir.InnerText, ListDirs.SelectedItem.Text);
                string newDirLCase = newDir.ToLower();
                if (newDir.Equals(_startingDir))
                {
                    FolderParent.Visible = false;
                    SetContentFolder.Visible = false;
                }
                if (!string.IsNullOrEmpty(_folderNameDetection) && newDirLCase.Contains(_folderNameDetection))
                {
                    SetContentFolder.Visible = true;
                }
                else if (string.IsNullOrEmpty(_folderNameDetection))
                {
                    FolderParent.Visible = true;
                    SetContentFolder.Visible = true;
                }
                else
                {
                    FolderParent.Visible = true;
                    SetContentFolder.Visible = false;
                }
                lblCurrentDir.InnerText = newDir;
                //bool listFolder = ShowDirectoriesIn(newDir);
                //if (listFolder)
                //{
                //    ShowFilesIn(newDir);
                //}
                List<string> folders = ShowDirectoriesIn(newDir);
                ListDirs.DataSource = folders;
                ListDirs.DataBind();
                if (folders != null)
                {
                    ShowFilesIn(newDir);
                }
            }
        }

        protected void ListFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListFiles.SelectedIndex != -1)
            {
                msiinfo.Visible = false;
                fileinfo.Visible = false;
                string fileName = Path.Combine(lblCurrentDir.InnerText, ListFiles.SelectedItem.Text);
                GetFileData(fileName);
            }
        }
        private void CmdBrowse_Clickk(object sender, EventArgs e)
        {
            if (ListDirs.SelectedIndex != -1)
            {
                string newDir = Path.Combine(lblCurrentDir.InnerText, ListDirs.SelectedItem.Text);
                string newDirLCase = newDir.ToLower();
                if (newDir.Equals(_startingDir))
                {
                    FolderParent.Visible = false;
                    SetContentFolder.Visible = false;
                }
                if (!string.IsNullOrEmpty(_folderNameDetection) && newDirLCase.Contains(_folderNameDetection))
                {
                    SetContentFolder.Visible = true;
                }
                else if (string.IsNullOrEmpty(_folderNameDetection))
                {
                    FolderParent.Visible = true;
                    SetContentFolder.Visible = true;
                }
                else
                {
                    FolderParent.Visible = true;
                    SetContentFolder.Visible = false;
                }
                lblCurrentDir.InnerText = newDir;
                //bool listFolder = ShowDirectoriesIn(newDir);
                //if (listFolder)
                //{
                //    ShowFilesIn(newDir);
                //}
                List<string> folders = ShowDirectoriesIn(newDir);
                ListDirs.DataSource = folders;
                ListDirs.DataBind();
            }
        }

        private string GetMSIProductCode(string filename)
        {
            string returnvalue = null;
            using (var database = new QDatabase(filename, DatabaseOpenMode.ReadOnly))
            {
                var properties = from p in database.Properties
                                 select p;

                foreach (var property in properties)
                {
                    if (property.Property.Equals("ProductCode"))
                    {
                        returnvalue = property.Value;
                    }
                }
            }
            return returnvalue;
        }
        private void GetFileData(string filename)
        {
            //string fileName = Path.Combine(lblCurrentDir.InnerText, lstFiles.SelectedItem.Text);
            FileInfo selFile = new FileInfo(filename);
            IDictionary<string, string> d = new Dictionary<string, string>();
            var sdict = new SortedDictionary<string, string>();

            string FileExtension = selFile.Extension.ToLower();

            if (FileExtension.Equals(".msi"))
            {
                string ProductVersion = string.Empty;
                string ProductName = string.Empty;

                // open MSI using the WiX toolset, ref https://wixtoolset.org/
                using var database = new QDatabase(filename, DatabaseOpenMode.ReadOnly);
                var properties = from p in database.Properties
                                 select p;

                foreach (var property in properties)
                {
                    if (property.Property.Equals("ProductCode"))
                    {
                        detectioncode.Value = property.Value;
                        dropdownDetectionMethods.SelectedIndex = 0;
                    }
                    if (property.Property.Equals("Manufacturer"))
                    {
                        manufacturer.Value = property.Value;
                    }
                    if (property.Property.Equals("ProductName"))
                    {
                        //appname.Value = property.Value;
                        ProductName = property.Value;
                    }
                    if (property.Property.Equals("ProductVersion"))
                    {
                        //appversion.Value = property.Value;
                        ProductVersion = property.Value;
                    }

                    appversion.Value = ProductVersion;
                    appname.Value = ProductName + " " + ProductVersion;
                    appnamehidden.Value = ProductName;

                    sdict.Add(property.Property, property.Value);
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("Key");
                dt.Columns.Add("Value");

                foreach (KeyValuePair<string, string> kvp in sdict)
                {
                    dt.Rows.Add(kvp.Key, kvp.Value);
                }
                dt.DefaultView.Sort = "Key ASC";

                Session["MSIDatatable"] = dt;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                msiinfo.Visible = true;
                btntable.InnerText = "> Show All MSI properties (" + sdict.Count.ToString() + ")";

                //using (var database = new Database(fileName, DatabaseOpenMode.ReadOnly))
                //{
                //    using (var view = database.OpenView(database.Tables["Property"].SqlSelectString))
                //    {
                //        view.Execute();
                //        foreach (var rec in view) using (rec)
                //            {
                //                Log("{0} = {1}", rec.GetString("Property"), rec.GetString("Value"));
                //            }
                //    }
                //}
            }
            else if (FileExtension.Equals(".ico") | FileExtension.Equals(".png"))
            {
                //imgappicon.ImageUrl = this.PhotoBase64ImgSrc(filename);
                //imgappicon.ToolTip = filename;
                Bitmap orig = new Bitmap(filename);

                Bitmap newfile = new Bitmap(orig, 110, 110);
                ImageConverter converter = new ImageConverter();
                icon2bytes = (byte[])converter.ConvertTo(newfile, typeof(byte[]));

                //icon2bytes = File.ReadAllBytes(filename);

                imgCtrl.Src = this.PhotoBase64ImgSrc(filename);
                imgCtrl.Attributes.Add("title", filename);
                imgCtrl.Alt = filename;
                lblappikon.InnerText = "AppIcon: " + selFile.Name;
            }
            else if (FileExtension.Equals(".exe"))
            {
                if (!(selFile.Name.ToLower().Equals("deploy-application.exe")))
                {
                    try
                    {
                        FileVersionInfo selFileVersionInfo =
                        FileVersionInfo.GetVersionInfo(filename);
                        var versInfo = FileVersionInfo.GetVersionInfo(filename);
                        string fileVersionFull = versInfo.FileVersion;
                        string fileVersionSemantic = $"{versInfo.FileMajorPart}.{versInfo.FileMinorPart}.{versInfo.FileBuildPart}";
                        manufacturer.Value = selFileVersionInfo.CompanyName;
                        appversion.Value = fileVersionSemantic;
                        appname.Value = selFileVersionInfo.ProductName + " " + fileVersionSemantic;
                    }
                    catch (Exception ex)
                    {
                        Log(String.Format("An error occured when attempting to get file version info for file {1}. Error message: {0}", ex.Message, filename));
                    }
                    if (chkboxIcon2Exe.Checked)
                    {
                        icon2bytes = ExtractIcon(filename); // Extract picture from file, turn it into byte-array
                        if (icon2bytes != null)
                        {
                            var base64Data = Convert.ToBase64String(icon2bytes); // Convert byte-array to base64 to display in img control
                            imgCtrl.Src = "data:image/png;base64," + base64Data;
                            imgCtrl.Attributes.Remove("title");
                            imgCtrl.Alt = "";
                            lblappikon.InnerText = "AppIcon: " + selFile.Name;
                        }
                    }
                }
            }
        }

        Byte[] ExtractIcon(string filename)
        {
            byte[] bytes = null;
            // Använder library IconExtractor och IconUtil från https://www.codeproject.com/Articles/26824/Extract-icons-from-EXE-or-DLL-files
            // Copyright 2014 Tsuda Kageyu
            IconExtractor ie = new IconExtractor(filename);
            int iconCount = ie.Count;
            if (iconCount > 0)
            {
                bool no256icon = true;
                Bitmap bitmap = null;
                System.Drawing.Icon icon0 = ie.GetIcon(0);
                System.Drawing.Icon[] splitIcons = IconUtil.Split(icon0);
                try
                {
                    bitmap = ExtractVistaIcon(icon0);
                    if (!(bitmap == null))
                    {
                        using MemoryStream ms = new MemoryStream();
                        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        var base64Data = Convert.ToBase64String(ms.ToArray());
                        bytes = Convert.FromBase64String(base64Data);

                        //bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                    }
                    else
                    {
                        //CM_ShowModal(appexists, "", "Misslyckades läsa in ikon!", "Ingen Bitmap inläst (från funktionen)");
                        foreach (System.Drawing.Icon ico in splitIcons)
                        {
                            if (ico.Height == 256)
                            {
                                bitmap = IconUtil.ToBitmap(ico);
                                no256icon = false;
                                break;
                            }
                        }
                        if (no256icon)
                        {
                            bitmap = IconUtil.ToBitmap(splitIcons[1]);
                        }
                        bitmap = IconUtil.ToBitmap(splitIcons[1]);
                        using MemoryStream ms = new MemoryStream();
                        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        var base64Data = Convert.ToBase64String(ms.ToArray());
                        bytes = Convert.FromBase64String(base64Data);

                    }
                    bitmap.Dispose();
                }
                catch (Exception ex)
                {
                    CM_ShowModal(null, "", "Failed to extract Icon!", "ErrorMessage:<br>" + ex.Message);
                }
            }
            return bytes;
        }

        // Source code happily re-used from https://stackoverflow.com/questions/220465/using-a-256-x-256-windows-vista-icon-in-an-application/1945764
        Bitmap ExtractVistaIcon(System.Drawing.Icon icoIcon)
        {
            Bitmap bmpPngExtracted = null;
            try
            {
                byte[] srcBuf = null;
                using (MemoryStream stream = new MemoryStream())
                { icoIcon.Save(stream); srcBuf = stream.ToArray(); }
                const int SizeICONDIR = 6;
                const int SizeICONDIRENTRY = 16;
                int iCount = BitConverter.ToInt16(srcBuf, 4);
                for (int iIndex = 0; iIndex < iCount; iIndex++)
                {
                    int iWidth = srcBuf[SizeICONDIR + SizeICONDIRENTRY * iIndex];
                    int iHeight = srcBuf[SizeICONDIR + SizeICONDIRENTRY * iIndex + 1];
                    int iBitCount = BitConverter.ToInt16(srcBuf, SizeICONDIR + SizeICONDIRENTRY * iIndex + 6);
                    if (iWidth == 0 && iHeight == 0 && iBitCount == 32)
                    {
                        int iImageSize = BitConverter.ToInt32(srcBuf, SizeICONDIR + SizeICONDIRENTRY * iIndex + 8);
                        int iImageOffset = BitConverter.ToInt32(srcBuf, SizeICONDIR + SizeICONDIRENTRY * iIndex + 12);
                        MemoryStream destStream = new MemoryStream();
                        BinaryWriter writer = new BinaryWriter(destStream);
                        writer.Write(srcBuf, iImageOffset, iImageSize);
                        destStream.Seek(0, SeekOrigin.Begin);
                        bmpPngExtracted = new Bitmap(destStream); // This is PNG! :)
                        break;
                    }
                }
            }
            catch { return null; }
            return bmpPngExtracted;
        }

        // Supporting function that converts an image to base64.
        protected string PhotoBase64ImgSrc(string fileNameandPath)
        {
            byte[] byteArray = File.ReadAllBytes(fileNameandPath);
            string base64 = Convert.ToBase64String(byteArray);

            return string.Format("data:image/gif;base64,{0}", base64);
        }
        protected void SetContentFolder_Click(object sender, EventArgs e)
        {
            appcontentpath.Value = lblCurrentDir.InnerText;
            appcontentpath.Attributes.Add("title", string.Format("UNC Path: {0}", lblCurrentDir.InnerText));

            long foldersize = GetDirectorySize(appcontentpath.Value);

            Constant = new ConstantValue(Convert.ToString((long)foldersize / 1024 / 1024 * 2) /* 5000MB to KB to bytes */, DataType.Int64); // Convert content size to MB and double it for disk requirement rule

            contenthelpBlock.InnerHtml = "Disk Requirement rule will be set to " + Constant.Value + " MB";

            int EstimatedExecTimeBasedOnDiskRequirementRule = int.Parse(Constant.Value) / 100 + 10; // This is just something made up, adjust as necessary..
            estimatedexecutiontime.Value = EstimatedExecTimeBasedOnDiskRequirementRule.ToString();
            helpblockestimatedexectime.InnerHtml = "Estimated execution time has been calculated based on disk requirement rule";

            DirectoryInfo dirInfo = new DirectoryInfo(lblCurrentDir.InnerText);
            foreach (FileInfo fileItem in dirInfo.GetFiles())
            {
                if (fileItem.Name.ToLower().Equals("deploy-application.exe"))
                {
                    appinstallcmd.Value = fileItem.Name;
                    appuninstallcmd.Value = fileItem.Name + " Uninstall";
                    apprepaircmd.Value = fileItem.Name + " Repair";

                }
                if (fileItem.Extension.ToLower().Equals(".msi"))
                {
                    string msifile = fileItem.Name;
                    bool msifileHasSpace = msifile.Contains(" ");
                    if (msifileHasSpace)
                    {
                        // add double quoutes to filename
                        msifile = "\"" + fileItem.Name + "\"";
                    }
                    appinstallcmd.Value = "msiexec /i " + msifile + " /qn";
                    appuninstallcmd.Value = "msiexec /x" + GetMSIProductCode(fileItem.FullName) + " /qn";
                    apprepaircmd.Value = "msiexec /fa " + msifile + " /qn";
                }
            }
        }

        protected void CreateCMApp_Click(object sender, EventArgs e)
        {
            bool testwrite = Log(String.Format("CreateApp button clicked, start app creation flow - appname {0}", appname.Value));

            if (testwrite)
            {
                WqlResultObject appexists = GetAppWQLFromName(appname.Value);

                if (appexists == null)
                {
                    if ((!(string.IsNullOrEmpty(imgCtrl.Src))) | icon2bytes == null)
                    {
                        string installcmd = appinstallcmd.Value.ToLower();

                        if (installcmd.Contains(".exe") || installcmd.Contains(".ps1") || installcmd.Contains(".bat") || installcmd.Contains(".cmd"))
                        {
                            Main("script", chkboxInteractive.Checked);
                        }
                        else if (installcmd.Contains(".msi"))
                        {
                            Main("msi", chkboxInteractive.Checked);
                        }
                        else
                        {
                            CM_ShowModal(appexists, "Stop! Existing App found with same name: " + appname.Value, "Could not create App", "Not supported for the filetype" + appinstallcmd.Value.ToLower());
                        }
                    }
                    else
                    {
                        CM_ShowModal(appexists, "Stop! Existing App found with same name: " + appname.Value, "AppIcon missing!", "Fileformats that are accepted for Icons are .ico, .png or .exe. Ikon will be extraced and imported automatically if the app encounters a file of these types.");
                    }

                }
                else
                {
                    CM_ShowModal(appexists, "Stop! Existing App found with same name: " + appname.Value, "Stop! Existing App found with same name: " + appname.Value, "");
                }
            }
            else
            {
                CM_ShowModal(null, "Error! Cannot write to logfile", "Error! Cannot write to logfile", "Check access rights");
            }

        }
        // ref: https://www.scrapingbee.com/blog/web-scraping-csharp/
        private async Task GetDataFromServicesAsync()
        {
            string keyword = "about";
            string url = string.Format("https://www.google.com/search?q={0}+{1}+{2}&hl=en&gl=en", keyword, appname.Value, manufacturer.Value);
            //string response = null;
            //RegisterAsyncTask(new PageAsyncTask(CallUrl(url)));

            //var response = await CallUrl(url).Result;
            var task = CallUrlAsync(url);

            await Task.WhenAll(task);
            var parsedhtml = ParseHtml(task.Result);
            string resulthtml;
            if (string.IsNullOrEmpty(parsedhtml))
            {
                resulthtml = task.Result;
            }
            else
            {
                resulthtml = parsedhtml;
            }
            //var response = CallUrl(url).Result;
            CM_ShowModal2("Online Search results", "Copy relevant text below to software description field", resulthtml);
        }

        private string ParseHtml(string html)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var sb = new StringBuilder();
            foreach (var node in htmlDoc.DocumentNode.ChildNodes)
            {
                if (node.Name != "a")
                {
                    sb.Append(node.InnerHtml);
                }
            }

            var programmerLinks = htmlDoc.DocumentNode.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "").Contains("xpc")).ToList(); // div classxpc for new Google UI, div class kp-wholepage for old google UI card... // ref https://dev.to/samzhangjy/crawling-google-search-results-part-3-fetching-wiki-192f

            string myresults = null;
            foreach (HtmlNode div in programmerLinks)
            {
                if (div.Name != "a")
                {
                    myresults += div.InnerHtml;
                }

            }
            return myresults;
        }

        public Task<string> CallUrlAsync(string URL)
        {
            return Task.Run(() => CallUrl(URL));
        }

        private string CallUrl(string fullUrl)
        {
            HtmlWeb web = new HtmlWeb();
            var doc = web.Load(fullUrl);
            foreach (var item in doc.DocumentNode.SelectNodes("//style"))
            {
                item.Remove();
            }
            //var node = doc.DocumentNode.SelectSingleNode("//removeme");
            //node.ParentNode.RemoveChild(node, true);
            //var node = htmlDoc.DocumentNode.SelectSingleNode("//head/title");
            return doc.DocumentNode.InnerHtml;
        }
        private static async Task<string> CallUrlTask(string fullUrl)
        {
            HttpClient client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Trident/7.0; rv:11.0) like Gecko"); // IE 11 W10 header
            var response = await client.GetStringAsync(fullUrl);
            client.Dispose();
            return response;
        }

        protected async void SearchOnline_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(appname.Value))
            {
                await GetDataFromServicesAsync();
            }
        }

        protected void SearchCMApp_Click(object sender, EventArgs e)
        {
            //WqlResultObject application = SearchAppWQLFromName(appname.Value); //GetAppWQLFromName(appname.Value);
            string AppToFind = appname.Value;
            if (!string.IsNullOrEmpty(AppToFind))
            {
                IList apps = GetCMApplication(AppToFind);

                if (apps != null)
                {
                    foreach (CMApplication app in apps)
                    {
                        string _securityScopes = string.Join(", ", app.SecurityScopes.OfType<string>());
                        HtmlTableRow tRow = new HtmlTableRow();
                        HtmlTableCell tb = new HtmlTableCell
                        {
                            InnerHtml = string.Format("<b>{0}</b> - <i>ObjectPath: {1}</i> - _securityScope(s): {2}", app.ApplicationName, app.ObjectPath, _securityScopes)
                        };
                        tRow.Controls.Add(tb);
                        tblmodalbodytxt.Rows.Add(tRow);
                    }

                    modaldialog.Attributes.Add("class", "modal-dialog modal-lg");
                    modaltitle.InnerHtml = string.Format("Searchresult: Found {0} application(s)", apps.Count);
                    modaltblheader.InnerHtml = "Details";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
            }
        }

        public void CM_ShowModal(WqlResultObject application, string titlefound, string titlenotfound, string textnotfound)
        {
            if (application != null)
            {
                // Order by values.
                // ... Use LINQ to specify sorting by value.
                var items = from pair in application.PropertyList
                            where pair.Key == "CreatedBy" || pair.Key == "DateCreated" || pair.Key == "IsDeployed" || pair.Key == "LocalizedDisplayName" || pair.Key == "Manufacturer" || pair.Key == "SecuredScopeNames" || pair.Key == "SoftwareVersion"
                            orderby pair.Key ascending
                            select pair;

                foreach (KeyValuePair<string, string> kvp in items)
                {
                    //Console.WriteLine("Key = {0}, Value = {1}",kvp.Key, kvp.Value);
                    //lblFileInfo.InnerHtml += string.Format("{0} = {1}", kvp.Key, kvp.Value) + "<br>";
                    HtmlTableRow tRow = new HtmlTableRow();
                    HtmlTableCell tb = new HtmlTableCell
                    {
                        InnerText = string.Format("{0} = {1}", kvp.Key, kvp.Value)
                    };
                    tb.Attributes.Add("class", "p-1");
                    tRow.Controls.Add(tb);
                    tblmodalbodytxt.Rows.Add(tRow);
                }
                modaldialog.Attributes.Add("class", "modal-dialog modal-lg");
                modaltitle.InnerHtml = titlefound;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
            else
            {
                if (string.IsNullOrEmpty(textnotfound))
                {
                    textnotfound = "No data found..";
                }
                modaltitle.InnerHtml = titlenotfound;

                HtmlTableRow tRow = new HtmlTableRow();
                HtmlTableCell tb = new HtmlTableCell
                {
                    InnerHtml = textnotfound
                };
                tRow.Controls.Add(tb);
                tblmodalbodytxt.Rows.Add(tRow);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }
        }

        public void CM_ShowModal2(string title, string tabletitle, string tablemessage)
        {
            modaltitle.InnerHtml = title;

            HtmlTableRow tRow = new HtmlTableRow();
            HtmlTableCell tb = new HtmlTableCell
            {
                InnerHtml = tablemessage
            };
            tRow.Attributes.Add("class", "row");
            tRow.Controls.Add(tb);
            tblmodalbodytxt.Rows.Add(tRow);
            modaldialog.Attributes.Add("class", "modal-dialog modal-lg modal-dialog-scrollable");
            modaltblheader.InnerHtml = tabletitle;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }

        public Dictionary<string, string> GetApplicationWQLFromName(string applicationName)
        {
            //' Construct relation list
            var relations = new List<string>();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            //' Query for device relationship instances
            SelectQuery relationQuery = new SelectQuery(string.Format("SELECT * FROM SMS_ApplicationLatest WHERE (IsHidden = 0) AND LocalizedDisplayName='{0}'", applicationName.Trim()));
            ManagementScope managementScope = new ManagementScope("\\\\" + _siteServer + "\\root\\SMS\\site_" + _siteCode);
            managementScope.Connect();

            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(managementScope, relationQuery);
            if (managementObjectSearcher != null)
            {
                foreach (var appreference in managementObjectSearcher.Get())
                {
                    //parameters.Add("ObjectPath", (string)appreference.GetPropertyValue("ObjectPath"));
                    //parameters.Add("ModelName", (string)appreference.GetPropertyValue("ModelName"));
                    parameters.Add("Manufacturer", (string)appreference.GetPropertyValue("Manufacturer"));
                    parameters.Add("LocalizedDisplayName", (string)appreference.GetPropertyValue("LocalizedDisplayName"));

                    return parameters;

                }

                return null;
            }
            else
            {
                return null;
            }
        }

        private WqlResultObject GetAppWQLFromName(string applicationName)
        {
            System.Security.Principal.WindowsImpersonationContext impersonationContext;

            SmsProvider smsProvider = new SmsProvider();
            impersonationContext =
                ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
            WqlConnectionManager connection = smsProvider.Connect(_siteServer);
            impersonationContext.Undo();

            //Log(String.Format("Searching for app created with name {0} in order to continue", applicationName));
            ////get the application based on the display name
            string wmiQuery = string.Format("SELECT * FROM SMS_Application WHERE SMS_APPLICATION.IsLatest = 1 AND LocalizedDisplayName='{0}'", applicationName.Trim());
            //Log(String.Format("Searching with WMI query {0} in order to continue", wmiQuery));

            // Impersonate
            impersonationContext =
                ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
            WqlQueryResultsObject applicationResults = connection.QueryProcessor.ExecuteQuery(wmiQuery) as WqlQueryResultsObject;
            impersonationContext.Undo();

            connection.Dispose();
            ////return the first instance of the application found based on the query - note this assumes the name is unique!
            foreach (WqlResultObject appReference in applicationResults)
            {
                //Log(String.Format("{0} was found", applicationName));
                return appReference;
            }

            ////didn't find anything with the name
            //Log(String.Format("{0} was NOT found", applicationName));
            return null;
        }

        public List<CMApplication> GetCMApplication(string filter)
        {
            //' Variable for return value
            List<CMApplication> appList = new List<CMApplication>();

            // Initialize impersonation
            System.Security.Principal.WindowsImpersonationContext impersonationContext;

            //' Connect to SMS Provider
            SmsProvider smsProvider = new SmsProvider();
            impersonationContext =
                ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
            WqlConnectionManager connection = smsProvider.Connect(_siteServer);
            impersonationContext.Undo();

            //' Determine query for application objects
            string appQuery;
            if (!String.IsNullOrEmpty(filter))
            {
                // source wmi class https://docs.microsoft.com/en-us/mem/configmgr/develop/reference/apps/sms_applicationlatest-server-wmi-class
                appQuery = String.Format("SELECT * FROM SMS_ApplicationLatest WHERE (IsHidden = 0) AND (LocalizedDisplayName like '{0}%')", filter);

                // Impersonate

                impersonationContext =
                    ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                //' Get applications objects
                IResultObject appInstances = connection.QueryProcessor.ExecuteQuery(appQuery);
                impersonationContext.Undo();

                if (appInstances != null)
                {
                    foreach (IResultObject app in appInstances)
                    {
                        //' Construct a new CMApplication object
                        CMApplication application = new CMApplication
                        {

                            //' Assign properties to CMApplication object from query result and add object to list
                            ApplicationName = app["LocalizedDisplayName"].StringValue,
                            ApplicationDescription = app["LocalizedDescription"].StringValue,
                            ApplicationManufacturer = app["Manufacturer"].StringValue,
                            ApplicationVersion = app["SoftwareVersion"].StringValue,
                            //ApplicationCreated = app["DateCreated"].DateTimeValue,
                            SecurityScopes = app["SecuredScopeNames"].StringArrayValue,
                            ApplicationCreatedBy = app["CreatedBy"].StringValue,
                            ObjectPath = app["ObjectPath"].StringValue
                        };
                        appList.Add(application);
                    }
                }
            }
            connection.Dispose();

            return appList;
        }

        private void CM_DistContent(WqlResultObject application)
        {
            //WqlResultObject application = GetAppWQLFromName(appname);
            if (application != null)
            {
                System.Security.Principal.WindowsImpersonationContext impersonationContext;

                SmsProvider smsProvider = new SmsProvider();
                impersonationContext =
                    ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                WqlConnectionManager connection = smsProvider.Connect(_siteServer);
                impersonationContext.Undo();

                ////get the package ids associated with the application
                string packageQuery = "SELECT PackageID FROM SMS_ObjectContentInfo WHERE ObjectID='" + application.PropertyList["ModelName"] + "'";

                // Impersonate
                impersonationContext =
                    ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                IResultObject packages = connection.QueryProcessor.ExecuteQuery(packageQuery);
                impersonationContext.Undo();

                List<string> idList = new List<string>();
                if (packages != null)
                {
                    foreach (IResultObject package in packages)
                    {
                        idList.Add(package.PropertyList["PackageID"]);
                    }
                }

                ////return the selected distribution point group
                string dpgQuery = string.Format("Select * From SMS_DistributionPointGroup Where Name = '{0}'", _dpGroupName);

                // Impersonate
                impersonationContext =
                    ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                WqlQueryResultsObject dpgresults = connection.QueryProcessor.ExecuteQuery(dpgQuery) as WqlQueryResultsObject;
                impersonationContext.Undo();

                WqlResultObject result = null;
                foreach (WqlResultObject dpgresult in dpgresults)
                {
                    result = dpgresult;
                }

                if (result != null)
                {
                    ////send them to the distribution point group
                    Dictionary<string, object> methodParams = new Dictionary<string, object>
                    {
                        ["PackageIDs"] = idList.ToArray()
                    };

                    try
                    {
                        // Impersonate
                        impersonationContext =
                            ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                        result.ExecuteMethod("AddPackages", methodParams);
                        impersonationContext.Undo();

                        Log(String.Format("Successfully distributed application to DP Group: {0}", _dpGroupName));
                    }
                    catch (Exception ex)
                    {
                        Log(String.Format("An error occured when attempting to distribute to DP Group. Error message: {0}", ex.Message));
                    }
                }
            }
        }

        // ref https://docs.microsoft.com/en-us/mem/configmgr/develop/reference/core/servers/console/movemembers-method-in-class-sms_objectcontaineritem
        private void CM_MoveAppToFolder(WqlResultObject application, string TargetFolder)
        {
            //WqlResultObject application = GetAppWQLFromName(appname);
            if (application != null)
            {
                Log(String.Format("Try to move app to target folder: {0}", TargetFolder));
                System.Security.Principal.WindowsImpersonationContext impersonationContext;

                SmsProvider smsProvider = new SmsProvider();
                impersonationContext =
                    ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                WqlConnectionManager connection = smsProvider.Connect(_siteServer);
                impersonationContext.Undo();

                //get the ContainerNodeID associated with targetfolder name
                string folderQuery = "select ContainerNodeID from sms_objectcontainernode where ObjectType=6000 and Name='" + TargetFolder + "'";

                // Impersonate
                impersonationContext =
                    ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                IResultObject folderIDResult = connection.QueryProcessor.ExecuteQuery(folderQuery);
                impersonationContext.Undo();

                List<int> idList = new List<int>();
                if (folderIDResult != null)
                {
                    foreach (IResultObject folder in folderIDResult)
                    {
                        idList.Add(int.Parse(folder.PropertyList["ContainerNodeID"]));
                        //idList.Add(Convert.ToUInt32(folder.PropertyList["ContainerNodeID"], 16));
                    }
                }

                if (idList != null)
                {
                    string[] strarrinstancekey = new string[] { application.PropertyList["ModelName"] };

                    int ContainerNodeID = 0; //usually 0 when newly created Application.. so we'll assume that.. yay
                    //int TargetContainerNodeID = 16777970; // hardcoded during testing.. commented out for prod.
                    int TargetContainerNodeID = idList[0];
                    int ObjectType = 6000;


                    //' Construct in params for execution
                    Dictionary<string, object> execParams = new Dictionary<string, object>
                {
                    { "InstanceKeys", strarrinstancekey },
                    { "ContainerNodeID", ContainerNodeID },
                    { "TargetContainerNodeID", TargetContainerNodeID },
                    { "ObjectType", ObjectType }
                };

                    try
                    {
                        // Impersonate
                        impersonationContext =
                            ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                        IResultObject execute = connection.ExecuteMethod("SMS_ObjectContainerItem", "MoveMembers", execParams);
                        impersonationContext.Undo();

                        if (execute["ReturnValue"].IntegerValue == 0)
                        {
                            // returnValue = true;
                            Log(String.Format("Successfully moved application to folder: {0}", TargetFolder));
                        }
                    }
                    catch (Exception ex)
                    {
                        Log(String.Format("An error occured when attempting to move object. Error message: {0}", ex.Message));
                    }
                    finally
                    {
                        connection.Dispose();
                    }
                }
                else
                {
                    Log(String.Format("ERROR - Could not find folder {0} to move object to.", TargetFolder));
                }


            }
        }

        private void CM_DeployApp(WqlResultObject application, string collectionname, bool allowrepair, bool approvalrequired)
        {
            System.Security.Principal.WindowsImpersonationContext impersonationContext;

            SmsProvider smsProvider = new SmsProvider();
            impersonationContext =
                ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
            WqlConnectionManager connection = smsProvider.Connect(_siteServer);
            impersonationContext.Undo();

            ////retrieve the application
            //WqlResultObject application = GetAppWQLFromName(appname);

            if (application != null)
            {
                ////get the collection we want to apply to the deployment
                string collectionQuery = string.Format("SELECT * FROM SMS_Collection Where Name = '{0}'", collectionname);

                // Impersonate
                impersonationContext =
                    ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                WqlQueryResultsObject collections = connection.QueryProcessor.ExecuteQuery(collectionQuery) as WqlQueryResultsObject;
                impersonationContext.Undo();

                WqlResultObject result = null;
                foreach (WqlResultObject collection in collections)
                {
                    result = collection;
                }

                ////create an assignment (deployment) using the application and collection details
                //// ref: https://docs.microsoft.com/en-us/mem/configmgr/develop/reference/apps/sms_applicationassignment-server-wmi-class
                if (result != null)
                {
                    // Impersonate
                    impersonationContext = ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                    IResultObject applicationAssignment = connection.CreateInstance("SMS_ApplicationAssignment");
                    impersonationContext.Undo();

                    DateTime time = DateTime.Now;
                    //// assign the application information to the assignment
                    applicationAssignment["ApplicationName"].StringValue = application.PropertyList["LocalizedDisplayName"];
                    applicationAssignment["AssignedCI_UniqueID"].StringValue = application.PropertyList["CI_UniqueID"];
                    applicationAssignment["AssignedCIs"].IntegerArrayValue = new int[] { int.Parse(application.PropertyList["CI_ID"]) };
                    applicationAssignment["AssignmentName"].StringValue = application.PropertyList["LocalizedDisplayName"] + "_Deployment";
                    applicationAssignment["CollectionName"].StringValue = result.PropertyList["Name"];                                              ////use the collection name
                    applicationAssignment["DisableMomAlerts"].BooleanValue = true;
                    if (_AddBranding)
                    {
                        applicationAssignment["AssignmentDescription"].StringValue = string.Format(_BrandingText, HttpContext.Current.User.Identity.Name);
                    }
                    //applicationAssignment["EnforcementDeadline"].DateTimeValue = time;
                    applicationAssignment["NotifyUser"].BooleanValue = false;
                    //applicationAssignment["OfferFlags"].LongValue = 1;
                    if (allowrepair)
                    {
                        applicationAssignment["OfferFlags"].LongValue = 8;
                    }
                    applicationAssignment["OfferTypeID"].LongValue = 2; // 0 is required, 2 is available
                    applicationAssignment["DesiredConfigType"].LongValue = 1; // DesiredConfigType 1 is Install, 2 is Uninstall
                    applicationAssignment["OverrideServiceWindows"].BooleanValue = false;
                    applicationAssignment["RebootOutsideOfServiceWindows"].BooleanValue = false;
                    applicationAssignment["RequireApproval"].BooleanValue = approvalrequired;
                    applicationAssignment["StartTime"].DateTimeValue = time.AddHours(-2);
                    applicationAssignment["SuppressReboot"].LongValue = 0;
                    applicationAssignment["TargetCollectionID"].StringValue = result.PropertyList["CollectionID"]; ////use the collection id
                    applicationAssignment["UseGMTTimes"].BooleanValue = false;
                    applicationAssignment["UserUIExperience"].BooleanValue = true; // user notification
                    applicationAssignment["WoLEnabled"].BooleanValue = false;
                    applicationAssignment["LocaleID"].LongValue = 1033;

                    // Impersonate
                    impersonationContext = ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                    applicationAssignment.Put();
                    impersonationContext.Undo();

                    Log(String.Format("Successfully deployed application to collection: {0}", collectionname));
                }
            }
        }

        public void RemoveObjectScope(WqlResultObject application, string scopeId)
        {
            try
            {
                System.Security.Principal.WindowsImpersonationContext impersonationContext;

                SmsProvider smsProvider = new SmsProvider();
                impersonationContext =
                    ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                WqlConnectionManager connection = smsProvider.Connect(_siteServer);
                impersonationContext.Undo();

                UInt32 objectTypeId = 31;

                // Impersonate
                impersonationContext =
                    ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                IResultObject assignment = connection.GetInstance("SMS_SecuredCategoryMembership.CategoryID='" + scopeId + "',ObjectKey='" + application.PropertyList["ModelName"] + "',ObjectTypeID=" + objectTypeId.ToString());
                impersonationContext.Undo();

                if (assignment != null)
                {
                    // Impersonate
                    impersonationContext =
                        ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                    assignment.Delete();
                    impersonationContext.Undo();
                    Log(String.Format("Successfully removed security scopeid: {0}", scopeId));
                }

                connection.Dispose();
            }
            catch (Exception ex)
            {
                Log(String.Format("An error occured when attempting to remove scope {1}. Error message: {0}", ex.Message, scopeId));
            }

        }

        public bool AddObjectScope(WqlResultObject application, string scopeId)
        {
            try
            {
                System.Security.Principal.WindowsImpersonationContext impersonationContext;

                SmsProvider smsProvider = new SmsProvider();
                impersonationContext =
                    ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                WqlConnectionManager connection = smsProvider.Connect(_siteServer);
                impersonationContext.Undo();

                impersonationContext =
                    ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                IResultObject assignment = connection.CreateInstance("SMS_SecuredCategoryMembership");
                impersonationContext.Undo();

                // Configure the assignment  
                assignment.Properties["CategoryID"].StringValue = scopeId;
                assignment.Properties["ObjectKey"].StringValue = application.PropertyList["ModelName"];
                assignment.Properties["ObjectTypeID"].IntegerValue = 31;

                // Commit the assignment  
                // Impersonate
                impersonationContext =
                    ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                assignment.Put();
                impersonationContext.Undo();
                Log(String.Format("Successfully added security scopeid: {0}", scopeId));
                connection.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                Log(String.Format("An error occured when attempting to add scope {1}. Error message: {0}", ex.Message, scopeId));
                return false;
            }

        }

        public bool AddAdministrativeCategories(WqlResultObject application, IList<string> appcategories)
        {
            bool results = false;
            System.Security.Principal.WindowsImpersonationContext impersonationContext;

            application.Properties["CategoryInstance_UniqueIDs"].StringArrayValue = appcategories.ToArray();
            try
            {
                // Impersonate
                impersonationContext = ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                // Commit the assignment  
                application.Put();
                impersonationContext.Undo();
                results = true;
                Log("Successfully added administrative categories");
            }
            catch (Exception ex)
            {
                Log(String.Format("An error occured when attempting to add administrative categories. Error message: {0}", ex.Message));
            }
            return results;
        }


        //static void Main(string[] args)
        private void Main(string DtType, bool isinteractive)
        {
            //System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));

            Log("Running Main, initializing");
            //Initialize(Environment.MachineName);

            // impersonate!!
            System.Security.Principal.WindowsImpersonationContext impersonationContext;
            impersonationContext =
                ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
            Initialize(_siteServer);
            impersonationContext.Undo();


            Log("Initializing succeeded, now create app");

            // Create the usercategories list and populate
            IList<string> usercategories = new List<string> {};
            foreach (ListItem item in dropdownusercategories.Items)
            {
                if (item.Selected)
                {
                    usercategories.Add(item.Value);
                };
            }

            // Create the appcategories (administrative categories) list and populate
            IList<string> appcategories = new List<string> { };
            foreach (ListItem item in dropdownappcategories.Items)
            {
                if (item.Selected)
                {
                    appcategories.Add(item.Value);
                };
            }

            // System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName <-- use this instead of _defaultLanguage to go for culture for calling user
            Application application = CreateApplication(appname.Value, manufacturer.Value, appversion.Value, appdescription.InnerText, softwarecenterdescription.InnerText, _defaultLanguage, icon2bytes, appowner.Value, appcontact.Value, usercategories, appcategories, _AddBranding, _BrandingText);
            long foldersize = GetDirectorySize(appcontentpath.Value);

            IList<string> dtlanguages = new List<string> {};
            // Hack to make sure language for deploymenttype has the correct syntas as it is case-sensitive...
            if (_defaultLanguage.Contains("-"))
            {
                string[] languagesplit = _defaultLanguage.Split('-');
                string leftpart = languagesplit[0].ToLower();
                string rightpart = languagesplit[1].ToUpper();
                string LanguageToAdd = leftpart + "-" + rightpart;
                dtlanguages.Add(LanguageToAdd);
            }
            if (DtType == "script")
            {
                application.DeploymentTypes.Add(CreateScriptDt(application.Title, application.Description, appinstallcmd.Value, appuninstallcmd.Value, apprepaircmd.Value, detectioncode.Value, dropdownDetectionMethods.Value, appcontentpath.Value, foldersize, int.Parse(maxexecutiontime.Value), int.Parse(estimatedexecutiontime.Value), application, isinteractive, dtlanguages));
            }
            if (DtType == "msi")
            {
                application.DeploymentTypes.Add(CreateMsiDt(application.Title, application.Description, appinstallcmd.Value, appuninstallcmd.Value, apprepaircmd.Value, detectioncode.Value, dropdownDetectionMethods.Value, appcontentpath.Value, foldersize, int.Parse(maxexecutiontime.Value), int.Parse(estimatedexecutiontime.Value), application, isinteractive, dtlanguages));
            }

            Log("Store app..");
            //Store(application);
            if (Store(application))
            {
                WqlResultObject isappcreated = GetAppWQLFromName(appname.Value);

                if (isappcreated != null)
                {
                    if (appcategories.Count > 0)
                    {
                        AddAdministrativeCategories(isappcreated, appcategories);
                    }
                    
                    if (chkboxDistribute.Checked)
                    {
                        CM_DistContent(isappcreated);
                    }

                    if (chkboxDeploy.Checked)
                    {
                        CM_DeployApp(isappcreated, dropdowncollections.Value, chkboxDeployRepair.Checked, chkboxDeployApproval.Checked);
                    }

                    if (!String.IsNullOrEmpty(_CMAppFolder))
                    {
                        CM_MoveAppToFolder(isappcreated, _CMAppFolder);
                    }

                    if (!string.IsNullOrEmpty(_securityScope))
                    {
                        bool ScopeResult = AddObjectScope(isappcreated, _securityScope); // custom scope, see wmi class SMS_SecuredCategory on _siteServer
                        if (ScopeResult)
                        {
                            RemoveObjectScope(isappcreated, "SMS00UNA"); // SMS00UNA = default scope
                        }
                    }

                    isappcreated = GetAppWQLFromName(appname.Value);
                }
                CM_ShowModal(isappcreated, "App Created (" + appname.Value + ")", "Failed to Create App (" + appname.Value + ")", "");
            }

        }

        // Initializes the default authoring scope and establishes connection to the SMS Provider.  
        // <param name="_siteServerName">A string containing the name of the Configuration Manager site.</param>  
        public static void Initialize(string _siteServerName)
        {
            Validator.CheckForNull(_siteServerName, "_siteServerName");
            //Log("Connecting to the SMS Provider on computer [{0}].", _siteServerName);
            // Creates a connection to the SMS Provider.  
            //WqlConnectionManager connectionManager = new WqlConnectionManager();
            //connectionManager.Connect(_siteServerName);

            SmsProvider smsProvider = new SmsProvider();
            WqlConnectionManager connectionManager = smsProvider.Connect(_siteServerName);
            //Log("Initializing the ApplicationFactory.");
            // Initialize application wrapper and factory for creating the SMS Provider application object.  
            factory = new ApplicationFactory();
            wrapper = AppManWrapper.Create(connectionManager, factory) as AppManWrapper;

        }

        // Inserts the provided application to the provided connected Configuration Manager site.  
        // <param name="application">An application object that will be inserted into the Configuration Manager site.</param>  
        public bool Store(Application application)
        {
            //using (HostingEnvironment.Impersonate())
            //{
            bool returnvalue = false;

            Validator.CheckForNull(application, "application");
            Validator.CheckForNull(wrapper, "wrapper");
            Exception ex = null;
            try
            {
                // Set the application into the provider object.  
                wrapper.InnerAppManObject = application;
                Log("Initializing the SMS_Application object with the model.");
                factory.PrepareResultObject(wrapper);
                Log("Saving application, Title: [{0}], Scope: [{1}].", application.Title, application.Scope);
                // impersonate!!
                System.Security.Principal.WindowsImpersonationContext impersonationContext;
                impersonationContext =
                    ((System.Security.Principal.WindowsIdentity)User.Identity).Impersonate();
                // Save to the database.  
                wrapper.InnerResultObject.Put();
                impersonationContext.Undo();
            }
            catch (SmsException exception)
            {
                ex = exception;
                Log("ERROR saving application [{0}].", exception.Message);
                var ExDataAsString = string.Join(Environment.NewLine, exception.Data.ToString());

                CM_ShowModal2("Failed to save app!", "Message", string.Format("Errormessage:<br>{0}<br><br>InnerException:<br>{1}<br><br>StackTrace:<br>{2}<br><br>ExceptionData:<br>{3}", exception.Message, exception.InnerException, exception.StackTrace, ExDataAsString));
            }
            catch (Exception exception)
            {
                ex = exception;
                Log("ERROR saving application [{0}].", exception.Message);
                var ExDataAsString = string.Join(Environment.NewLine, exception.Data.ToString());

                CM_ShowModal2("Failed to save app!", "Message", string.Format("Errormessage:<br>{0}<br><br>InnerException:<br>{1}<br><br>StackTrace:<br>{2}<br><br>ExceptionData:<br>{3}", exception.Message, exception.InnerException, exception.StackTrace, ExDataAsString));
            }
            if (ex != null)
            {
                //Log("ERROR saving application [{0}].", ex.Message);
                //Log("STACK saving application [{0}].", ex.StackTrace);
                //CM_ShowModal2("Misslyckades spara app!", "Felmeddelande:<br>" + ex.Message);
                //throw new System.Exception(String.Format("An error occured when attempting to connect to SMS Provider, message {0}. stackTrace: {1}",ex.Message, ex.StackTrace));
                //Log(ex);
            }
            else
            {
                returnvalue = true;
                Log("Successfully saved application.");
            }

            return returnvalue;
            //}
        }

        // Creates an Application object.  
        // <param name="title">The title of the application that will be visible in the admin console and in the Software Center.</param>  
        // <param name="description">The description for the application.</param>  
        // <param name="language">The language of the resources supplied.</param>  
        public static Application CreateApplication(string title, string publisher, string softversion, string admindescription, string description, string language, byte[] iconbytes, string owner, string contact, IList<string> usercategories, IList<string> appcategories, bool AddBranding, string _BrandingText)
        {
            Validator.CheckForNull(title, "title");
            Validator.CheckForNull(language, "language");
            Log("Creating application [{0}].", title);
            Application app = new Application { Title = title };


            // Input icon as byte array (Seems icondata with size 256*256 is not working.. or didn't during testing anyway)
            Microsoft.ConfigurationManagement.ApplicationManagement.Icon myicon = new Microsoft.ConfigurationManagement.ApplicationManagement.Icon
            {
                Data = iconbytes
            };

            app.Publisher = publisher;
            app.SoftwareVersion = softversion;
            if (AddBranding)
            {
                app.Description = admindescription + string.Format(" (" + _BrandingText + ")", HttpContext.Current.User.Identity.Name);
            }
            app.DisplayInfo.DefaultLanguage = language;

            AppDisplayInfo DisplayInfo = new AppDisplayInfo
            {
                Title = title,
                Description = description,
                Language = language,
                Icon = myicon
            };
            if (usercategories.Count > 0)
            {
                //app.DisplayInfo.Add(new AppDisplayInfo { Title = title, Description = description, Language = language, Icon = myicon, UserCategories = usercategories });
                DisplayInfo.UserCategories = usercategories;
            }
            if (appcategories.Count > 0)
            {
                //DisplayInfo.Tags = appcategories; // Note-to-self: Tags are keywords,not administrative categories..
                
            }

            app.DisplayInfo.DefaultLanguage = language;
            app.DisplayInfo.Add(DisplayInfo);

            User myowner = new User
            {
                Qualifier = "LogonName",
                Id = owner
            };

            User mysupportcontact = new User
            {
                Qualifier = "LogonName",
                Id = contact
            };
            
            app.Owners.Add(myowner);
            app.Contacts.Add(mysupportcontact);

            //app.DisplayInfo.Add(new AppDisplayInfo { Title = title, Description = description, Language = language });
            return app;
        }

        // Creates a Deployment Type with a Script Installer.  
        // <param name="title">A string containing the title for the Deployment Type (required).</param>  
        // <param name="description"> A string containing the description for the Deployment Type (optional).</param>  
        // <param name="installCommandLine">A string containing the installation command line for the installer (required).</param>  
        // <param name="detectionScript">A string containing the script for detection, this would most likely be separated out.  
        // to a different method to support creating different detection method types such as Windows Installer, EHD, and script. Additionally, in the case  
        // of script, the more likely scenario would be to load the script from a file, read the file, and then set the value.</param>  
        // <param name="contentFolder">The folder that will contain the set of files that will represent the content for this Deployment Type. Validation  
        // should verify that this is a UNC path, otherwise the Configuration Manager system will fail to create the content package correctly.</param>  
        // <returns>A deployment type object.</returns>  

        public static DeploymentType CreateScriptDt(string title, string description, string installCommandLine, string uninstallCommandLine, string repairCommandLine, string productcode, string detectionType, string contentFolder, long foldersize, int maxruntime, int estimatedruntime, Application application, bool interactive, IList<string> dtLanguages)
        {
            Validator.CheckForNull(installCommandLine, "installCommandLine");
            Validator.CheckForNull(title, "title");
            Validator.CheckForNull(detectionType, "detectionType");
            Log("Creating Script DeploymentType.");

            ScriptInstaller installer = new ScriptInstaller();

            if (detectionType.StartsWith("powershell"))
            {
                installer.InstallCommandLine = installCommandLine;
                installer.UninstallCommandLine = uninstallCommandLine;
                installer.RepairCommandLine = repairCommandLine;
                installer.DetectionMethod = DetectionMethod.Script;
                installer.DetectionScript = new Script { Text = productcode, Language = ScriptLanguage.PowerShell.ToString() };
            }
            else
            {
                string msipcode;
                if (!string.IsNullOrEmpty(productcode))
                {
                    msipcode = productcode;
                }
                else
                {
                    msipcode = Guid.NewGuid().ToString(); // Mockup product code will be added if none found in the input field (so user creating the app will have to adjust detection method afterwards)
                }

                installer.InstallCommandLine = installCommandLine;
                installer.UninstallCommandLine = uninstallCommandLine;
                installer.RepairCommandLine = repairCommandLine;
                installer.DetectionMethod = DetectionMethod.ProductCode;
                installer.ProductCode = msipcode;
                installer.MaxExecuteTime = maxruntime;
                installer.ExecuteTime = estimatedruntime;
            }

            EnhancedDetectionMethod ehd = new EnhancedDetectionMethod();

            string pcode;
            if (productcode != "")
            {
                pcode = productcode;
            }
            else
            {
                pcode = Guid.NewGuid().ToString();
            }
            MSISettingInstance msiSetting = new MSISettingInstance(pcode, null);
            ehd.Settings.Add(msiSetting);
            ConstantValue msiConstValue = new ConstantValue("0", DataType.Int64);
            SettingReference msiSettingRef = new SettingReference(
                application.Scope,
                application.Name,
                application.Version.GetValueOrDefault(),
                msiSetting.LogicalName,
                DataType.Int64,
                ConfigurationItemSettingSourceType.MSI,
                false)
            {
                MethodType = ConfigurationItemSettingMethodType.Count
            };
            CustomCollection<ExpressionBase> msiOperands = new CustomCollection<ExpressionBase>
            {
                msiSettingRef,
                msiConstValue
            };
            Expression msiExp = new Expression(ExpressionOperator.NotEquals, msiOperands);
            // Create a root collection to combine the registry and MSI EHDs
            CustomCollection<ExpressionBase> rootOperands = new CustomCollection<ExpressionBase>
            {
                //rootOperands.Add(regExp);
                msiExp
            };
            // Create a root expression
            Expression rootExp = new Expression(ExpressionOperator.And, rootOperands);
            // Create the rule
            Rule detectrule = new Rule("MyRuleId", NoncomplianceSeverity.None, null, msiExp);

            // Only add content if specified and exists.  
            if (Directory.Exists(contentFolder) == true)
            {
                Microsoft.ConfigurationManagement.ApplicationManagement.Content content = ContentImporter.CreateContentFromFolder(contentFolder);
                if (content != null)
                {
                    installer.Contents.Add(content);
                    installer.ExecutionContext = ExecutionContext.System;
                    if (interactive)
                    {
                        installer.RequiresLogOn = true;
                        installer.RequiresUserInteraction = true;
                    }
                    content.OnSlowNetwork = ContentHandlingMode.Download;
                    content.OnFastNetwork = ContentHandlingMode.Download;
                }
                // Fix to actually set content path through reference
                ContentRef contentReference = new ContentRef(content);
                installer.InstallContent = contentReference;
            }

            // Expression for the properties around the drive (in this case, must be system drive)
            CustomCollection<ExpressionBase> settingsOperands = new CustomCollection<ExpressionBase>();
            GlobalSettingReference driveIdSettingRef = new GlobalSettingReference("GLOBAL", "FreeDiskSpace", DataType.String, "DriveID", ConfigurationItemSettingSourceType.CIM);
            GlobalSettingReference systemDriveSettingRef = new GlobalSettingReference("GLOBAL", "FreeDiskSpace", DataType.String, "SystemDrive", ConfigurationItemSettingSourceType.CIM);
            settingsOperands.Add(driveIdSettingRef);
            settingsOperands.Add(systemDriveSettingRef);
            Expression driveSettingsExp = new Expression(ExpressionOperator.IsEquals, settingsOperands);

            // Expression for the drive space, must be greater than n
            CustomCollection<ExpressionBase> freeSpaceOperands = new CustomCollection<ExpressionBase>();
            GlobalSettingReference freeSpaceSettingRef = new GlobalSettingReference("GLOBAL", "FreeDiskSpace", DataType.Int64, "FreeSpace", ConfigurationItemSettingSourceType.CIM);
            //ConstantValue myconstant = new ConstantValue(Convert.ToString((long)5000 * 1024 * 1024) /* 5000MB to KB to bytes */, DataType.Int64);
            ConstantValue myconstant = new ConstantValue(Convert.ToString(foldersize * 2) /* MB to KB to bytes */, DataType.Int64);
            freeSpaceOperands.Add(freeSpaceSettingRef);
            freeSpaceOperands.Add(myconstant);
            Expression freeSpaceExp = new Expression(ExpressionOperator.GreaterEquals, freeSpaceOperands);

            // Outer expression that combines the previous expressions
            CustomCollection<ExpressionBase> outerOperands = new CustomCollection<ExpressionBase>
            {
                freeSpaceExp,
                driveSettingsExp
            };

            Expression fullExp = new Expression(ExpressionOperator.And, outerOperands);

            // Now build a rule and add it to the DT

            Rule rule = new Rule("MyRule_" + Guid.NewGuid().ToString(), NoncomplianceSeverity.Critical, null, fullExp);

            // Expression for primary device rule
            CustomCollection<ExpressionBase> primarydeviceOperands = new CustomCollection<ExpressionBase>();
            GlobalSettingReference primarydeviceSettingRef = new GlobalSettingReference("GLOBAL", "PrimaryDevice", DataType.Boolean, "PrimaryDevice_Setting_LogicalName", ConfigurationItemSettingSourceType.CIM);
            ConstantValue primarydeviceconstant = new ConstantValue(Convert.ToString(true) /* MB to KB to bytes */, DataType.Boolean);
            primarydeviceOperands.Add(primarydeviceSettingRef);
            primarydeviceOperands.Add(primarydeviceconstant);
            Expression primarydevice = new Expression(ExpressionOperator.IsEquals, primarydeviceOperands);

            Rule primarydevicerule = new Rule("MyRule_" + Guid.NewGuid().ToString(), NoncomplianceSeverity.Critical, null, primarydevice);

            DeploymentType dt = new DeploymentType(installer, ScriptInstaller.TechnologyId, NativeHostingTechnology.TechnologyId)
            {
                Title = title
            };
            dt.Requirements.Add(rule);
            dt.Requirements.Add(primarydevicerule);
            dt.Description = description;
            if (dtLanguages.Count > 0)
            {
                dt.Languages = dtLanguages;
            }

            // enable below for enhanced detection rule
            //ehd.Rule = detectrule;
            // Now add the rule to the detection method.
            //installer.EnhancedDetectionMethod = ehd;

            //{
            //Title = title,
            //dt.Requirements.Add(rule)
            //Requirements.Add(rule)

            //};
            return dt;
        }

        public static DeploymentType CreateMsiDt(string title, string description, string installCommandLine, string uninstallCommandLine, string repairCommandLine, string productcode, string detectionScript, string contentFolder, long foldersize, int maxruntime, int estimatedruntime, Application application, bool interactive, IList<string> dtLanguages)
        {
            Log("Creating MSI DeploymentType.");
            Validator.CheckForNull(installCommandLine, "installCommandLine");
            Validator.CheckForNull(title, "title");
            //Validator.CheckForNull(detectionScript, "detectionScript");

            string pcode;
            if (productcode != "")
            {
                pcode = productcode;
            }
            else
            {
                pcode = Guid.NewGuid().ToString();
            }

            //Log("Sätter productcode till ",pcode);

            MsiInstaller installer = new MsiInstaller
            {
                InstallCommandLine = installCommandLine,
                UninstallCommandLine = uninstallCommandLine,
                RepairCommandLine = repairCommandLine,
                //DetectionMethod = DetectionMethod.Enhanced
                DetectionMethod = DetectionMethod.ProductCode,
                ProductCode = pcode,
                MaxExecuteTime = maxruntime,
                ExecuteTime = estimatedruntime
        };

            Log("Creating MSI deploymenttype content.");
            // Only add content if specified and exists.  
            if (Directory.Exists(contentFolder) == true)
            {
                Microsoft.ConfigurationManagement.ApplicationManagement.Content content = ContentImporter.CreateContentFromFolder(contentFolder);
                if (content != null)
                {
                    installer.Contents.Add(content);
                    installer.ExecutionContext = ExecutionContext.System;
                    installer.PackageCode = pcode;
                    if (interactive)
                    {
                        installer.RequiresLogOn = true;
                        installer.RequiresUserInteraction = true;
                    }

                    content.OnSlowNetwork = ContentHandlingMode.Download;
                    content.OnFastNetwork = ContentHandlingMode.Download;
                }

                // Fix to actually set contentpath via reference
                ContentRef contentReferenece = new ContentRef(content);
                installer.InstallContent = contentReferenece;
            }

            // Expression for the properties around the drive (in this case, must be system drive)
            CustomCollection<ExpressionBase> settingsOperands = new CustomCollection<ExpressionBase>();
            GlobalSettingReference driveIdSettingRef = new GlobalSettingReference("GLOBAL", "FreeDiskSpace", DataType.String, "DriveID", ConfigurationItemSettingSourceType.CIM);
            GlobalSettingReference systemDriveSettingRef = new GlobalSettingReference("GLOBAL", "FreeDiskSpace", DataType.String, "SystemDrive", ConfigurationItemSettingSourceType.CIM);
            settingsOperands.Add(driveIdSettingRef);
            settingsOperands.Add(systemDriveSettingRef);
            Expression driveSettingsExp = new Expression(ExpressionOperator.IsEquals, settingsOperands);

            // Expression for the drive space, must be greater than n
            CustomCollection<ExpressionBase> freeSpaceOperands = new CustomCollection<ExpressionBase>();
            GlobalSettingReference freeSpaceSettingRef = new GlobalSettingReference("GLOBAL", "FreeDiskSpace", DataType.Int64, "FreeSpace", ConfigurationItemSettingSourceType.CIM);
            //ConstantValue myconstant = new ConstantValue(Convert.ToString((long)5000 * 1024 * 1024) /* 5000MB to KB to bytes */, DataType.Int64);
            ConstantValue myconstant = new ConstantValue(Convert.ToString(foldersize * 2) /* MB to KB to bytes */, DataType.Int64);
            freeSpaceOperands.Add(freeSpaceSettingRef);
            freeSpaceOperands.Add(myconstant);
            Expression freeSpaceExp = new Expression(ExpressionOperator.GreaterEquals, freeSpaceOperands);

            // Outer expression that combines the previous expressions
            CustomCollection<ExpressionBase> outerOperands = new CustomCollection<ExpressionBase>
            {
                freeSpaceExp,
                driveSettingsExp
            };

            Expression fullExp = new Expression(ExpressionOperator.And, outerOperands);

            // Now build a rule and add it to the DT

            Rule rule = new Rule("MyRule_" + Guid.NewGuid().ToString(), NoncomplianceSeverity.Critical, null, fullExp);

            // Expression for primary device rule
            CustomCollection<ExpressionBase> primarydeviceOperands = new CustomCollection<ExpressionBase>();
            GlobalSettingReference primarydeviceSettingRef = new GlobalSettingReference("GLOBAL", "PrimaryDevice", DataType.Boolean, "PrimaryDevice_Setting_LogicalName", ConfigurationItemSettingSourceType.CIM);
            ConstantValue primarydeviceconstant = new ConstantValue(Convert.ToString(true), DataType.Boolean);
            primarydeviceOperands.Add(primarydeviceSettingRef);
            primarydeviceOperands.Add(primarydeviceconstant);
            Expression primarydevice = new Expression(ExpressionOperator.IsEquals, primarydeviceOperands);

            Rule primarydevicerule = new Rule("MyRule_" + Guid.NewGuid().ToString(), NoncomplianceSeverity.Critical, null, primarydevice);

            DeploymentType dt = new DeploymentType(installer, MsiInstaller.TechnologyId, NativeHostingTechnology.TechnologyId)
            {
                Title = title
            };
            dt.Requirements.Add(rule);
            dt.Requirements.Add(primarydevicerule);
            dt.Description = description;
            if (dtLanguages.Count > 0)
            {
                dt.Languages = dtLanguages;
            }

            return dt;
        }

        public static void Log(Exception exception)
        {
            Log("ERROR: [{0}] ", exception.Message);
            Log("Stack: [{0}]", exception.StackTrace);
            if (exception.InnerException != null)
            {
                //Log(exception.InnerException);
            }
        }

        public static bool Log(string message, params object[] args)
        {
            bool results;

            results = SiteMaster.Log(message, args);
            return results;
        }
    }

    public class IconExtractorSmall
    {

        public static System.Drawing.Icon Extract(string file, int number, bool largeIcon)
        {
            ExtractIconEx(file, number, out IntPtr large, out IntPtr small, 1);
            try
            {
                return System.Drawing.Icon.FromHandle(largeIcon ? large : small);
            }
            catch
            {
                return null;
            }

        }
        [System.Runtime.InteropServices.DllImport("Shell32.dll", EntryPoint = "ExtractIconExW", CharSet = System.Runtime.InteropServices.CharSet.Unicode, ExactSpelling = true, CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        private static extern int ExtractIconEx(string sFile, int iIndex, out IntPtr piLargeVersion, out IntPtr piSmallVersion, int amountIcons);

    }

    public static class IconUtil
    {
        private delegate byte[] GetIconDataDelegate(System.Drawing.Icon icon);

        static GetIconDataDelegate getIconData;

        static IconUtil()
        {
            // Create a dynamic method to access Icon.iconData private field.

            var dm = new System.Reflection.Emit.DynamicMethod(
                "GetIconData", typeof(byte[]), new Type[] { typeof(System.Drawing.Icon) }, typeof(System.Drawing.Icon));
            var fi = typeof(System.Drawing.Icon).GetField(
                "iconData", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var gen = dm.GetILGenerator();
            gen.Emit(System.Reflection.Emit.OpCodes.Ldarg_0);
            gen.Emit(System.Reflection.Emit.OpCodes.Ldfld, fi);
            gen.Emit(System.Reflection.Emit.OpCodes.Ret);

            getIconData = (GetIconDataDelegate)dm.CreateDelegate(typeof(GetIconDataDelegate));
        }

        /// <summary>
        /// Split an Icon consists of multiple icons into an array of Icon each
        /// consists of single icons.
        /// </summary>
        /// <param name="icon">A System.Drawing.Icon to be split.</param>
        /// <returns>An array of System.Drawing.Icon.</returns>
        public static System.Drawing.Icon[] Split(System.Drawing.Icon icon)
        {
            if (icon == null)
                throw new ArgumentNullException("icon");

            // Create an .ico file in memory, then split it into separate icons.

            var src = GetIconData(icon);

            var splitIcons = new List<System.Drawing.Icon>();
            {
                int count = BitConverter.ToUInt16(src, 4);

                for (int i = 0; i < count; i++)
                {
                    int length = BitConverter.ToInt32(src, 6 + 16 * i + 8);    // ICONDIRENTRY.dwBytesInRes
                    int offset = BitConverter.ToInt32(src, 6 + 16 * i + 12);   // ICONDIRENTRY.dwImageOffset

                    using var dst = new BinaryWriter(new MemoryStream(6 + 16 + length));
                    // Copy ICONDIR and set idCount to 1.

                    dst.Write(src, 0, 4);
                    dst.Write((short)1);

                    // Copy ICONDIRENTRY and set dwImageOffset to 22.

                    dst.Write(src, 6 + 16 * i, 12); // ICONDIRENTRY except dwImageOffset
                    dst.Write(22);                   // ICONDIRENTRY.dwImageOffset

                    // Copy a picture.

                    dst.Write(src, offset, length);

                    // Create an icon from the in-memory file.

                    dst.BaseStream.Seek(0, SeekOrigin.Begin);
                    splitIcons.Add(new System.Drawing.Icon(dst.BaseStream));
                }
            }

            return splitIcons.ToArray();
        }

        /// <summary>
        /// Converts an Icon to a GDI+ Bitmap preserving the transparent area.
        /// </summary>
        /// <param name="icon">An System.Drawing.Icon to be converted.</param>
        /// <returns>A System.Drawing.Bitmap Object.</returns>
        public static Bitmap ToBitmap(System.Drawing.Icon icon)
        {
            if (icon == null)
                throw new ArgumentNullException("icon");

            // Quick workaround: Create an .ico file in memory, then load it as a Bitmap.

            using var ms = new MemoryStream();
            icon.Save(ms);
            return (Bitmap)System.Drawing.Image.FromStream(ms);
        }

        /// <summary>
        /// Gets the bit depth of an Icon.
        /// </summary>
        /// <param name="icon">An System.Drawing.Icon object.</param>
        /// <returns>The biggest bit depth of the icons.</returns>
        public static int GetBitCount(System.Drawing.Icon icon)
        {
            if (icon == null)
                throw new ArgumentNullException("icon");

            // Create an .ico file in memory, then read the header.

            var data = GetIconData(icon);

            int count = BitConverter.ToInt16(data, 4);
            int bitDepth = 0;
            for (int i = 0; i < count; ++i)
            {
                int depth = BitConverter.ToUInt16(data, 6 + 16 * i + 6);    // ICONDIRENTRY.wBitCount
                if (depth > bitDepth)
                    bitDepth = depth;
            }

            return bitDepth;
        }

        private static byte[] GetIconData(System.Drawing.Icon icon)
        {
            var data = getIconData(icon);
            if (data != null)
            {
                return data;
            }
            else
            {
                using var ms = new MemoryStream();
                icon.Save(ms);
                return ms.ToArray();
            }
        }
    }
}