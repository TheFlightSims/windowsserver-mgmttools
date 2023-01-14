using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Microsoft.UpdateServices.Administration;
using Microsoft.UpdateServices.Administration.Internal;
using Microsoft.Win32;

namespace WSUSAdminAssistant
{
    // Define a basic exception for configuration issues
    public class ConfigurationException : System.Exception
    {
        public string Summary;

        public ConfigurationException(string message, string problemsummary) : base(message)
        {
            Summary = problemsummary;
        }
    }

    public class clsConfig
    {
        public clsConfig()
        {
            // Open the configuration storage registry
            reg = Registry.CurrentUser.OpenSubKey(regPath, true);

            if (reg == null)
                // If the registry key doesn't already exist, create it.
                reg = Registry.CurrentUser.CreateSubKey(regPath);

            wsus = new clsWSUS(this);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // WSUS related classes
        public clsWSUS wsus;

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Useful private variables
        private string regPath = @"Software\\WSUSAdminAssistant";
        private RegistryKey reg;

        #region WSUSConfig
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // WSUS Configuration methods
        public string WSUSServer
        {
            get 
            {
                object o = reg.GetValue("WSUSServer");

                if (o == null)
                {
                    return "localhost";
                }
                else
                {
                    return (string)o;
                }
            }

            set { reg.SetValue("WSUSServer", value, RegistryValueKind.String); }
        }

        public bool WSUSSecureConnection
        {
            get
            {
                object o = reg.GetValue("WSUSSecureConnection");

                if (o == null)
                {
                    return false;
                }
                else
                {
                    return Convert.ToBoolean((int)reg.GetValue("WSUSSecureConnection"));
                }
            }

            set { reg.SetValue("WSUSSecureConnection", Convert.ToInt32(value), RegistryValueKind.DWord); }
        }
        
        #endregion

        #region DatabaseConfig
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Database Configuration methods
        public string DBServer
        {
           get 
            {
                object o = reg.GetValue("DBServer");

                if (o == null)
                {
                    return "localhost";
                }
                else
                {
                    return (string)o;
                }
            }

            set { reg.SetValue("DBServer", value, RegistryValueKind.String); }
        }

        public string DBDatabase
        {
            get
            {
                object o = reg.GetValue("DBDatabase");

                if (o == null)
                {
                    return "SUSDB";
                }
                else
                {
                    return (string)o;
                }
            }

            set { reg.SetValue("DBDatabase", value, RegistryValueKind.String); }
        }

        public bool DBIntegratedAuth
        {
            get
            {
                object o = reg.GetValue("DBIntegratedAuth");

                if (o == null)
                {
                    return false;
                }
                else
                {
                    return Convert.ToBoolean((int)reg.GetValue("DBIntegratedAuth"));
                }
            }

            set { reg.SetValue("DBIntegratedAuth", Convert.ToInt32(value), RegistryValueKind.DWord); }
        }

        public string DBUsername
        {
            get
            {
                object o = reg.GetValue("DBUsername");

                if (o == null)
                {
                    return "";
                }
                else
                {
                    return (string)o;
                }
            }

            set { reg.SetValue("DBUsername", value, RegistryValueKind.String); }
        }

        public string DBPassword
        {
            get
            {
                // Try to retreive and decrypt encyrpted password
                string pwd;
                object o = reg.GetValue("DBPasswordEncrypted");

                if (o != null)
                {
                    // Try to decrypt password
                    try
                    {
                        pwd = Decrypt(o.ToString(), "dbpwd");
                    }
                    catch
                    {
                        // Decryption failed, return empty password
                        pwd = "";
                    }
                }
                else
                {
                    // If retrieving the encrypted password fails, try to use the old unencrypted password
                    pwd = (string)reg.GetValue("DBPassword");

                    if (pwd == null)
                    {
                        // Unencrypted password not available - use default
                        pwd = "";
                    }
                    else
                    {
                        // Encrypt password, save to registry and delete old key
                        try
                        {
                            reg.SetValue("DBPasswordEncrypted", Encrypt(pwd, "dbpwd"), RegistryValueKind.String);
                            reg.DeleteValue("DBPassword");
                        }
                        catch (Exception e)
                        {
                            // Converting password failed, do not delete old key
                            Console.WriteLine("Failed to encrypt password: " + e.Message);
                        }
                    }
                }

                return pwd;
            }

            set { reg.SetValue("DBPasswordEncrypted", Encrypt(value, "dbpwd"), RegistryValueKind.String); }
        }

        public string SQLConnectionString()
        {
            if (this.DBIntegratedAuth)
                return "database=" + this.DBDatabase + "; server=" + this.DBServer + ";Trusted_Connection=true";
            else
                return "database=" + this.DBDatabase + "; server=" + this.DBServer + ";" + "User ID=" + this.DBUsername + ";Password=" + this.DBPassword;
        }
        
        #endregion

        #region UISettings
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // UI Settings methods
        public System.Drawing.Point WindowLocation
        {
            get
            {
                object x = reg.GetValue("WindowLocationX");
                object y = reg.GetValue("WindowLocationY");

                if (x == null || y == null)
                {
                    return new System.Drawing.Point(0, 0);
                }
                else
                {
                    return new System.Drawing.Point((int)x, (int)y);
                }
            }

            set
            {
                reg.SetValue("WindowLocationX", value.X, RegistryValueKind.DWord);
                reg.SetValue("WindowLocationY", value.Y, RegistryValueKind.DWord);
            }
        }

        public System.Drawing.Size WindowSize
        {
            get
            {
                object x = reg.GetValue("WindowSizeWidth");
                object y = reg.GetValue("WindowSizeHeight");

                if (x == null || y == null)
                {
                    return new System.Drawing.Size(800, 600);
                }
                else
                {
                    return new System.Drawing.Size((int)x, (int)y);
                }
            }

            set
            {
                reg.SetValue("WindowSizeWidth", value.Width, RegistryValueKind.DWord);
                reg.SetValue("WindowSizeHeight", value.Height, RegistryValueKind.DWord);
            }
        }

        public string WindowState
        {
            get
            {
                object o = reg.GetValue("WindowState");

                if (o == null)
                {
                    return "";
                }
                else
                {
                    return (string)o;
                }
            }

            set { reg.SetValue("WindowState", value, RegistryValueKind.String); }
        }

        public int EndpointSelections
        {
            get
            {
                object o = reg.GetValue("EndpointSelections");

                if (o == null)
                {
                    return 255;
                }
                else
                {
                    return int.Parse(o.ToString());
                }
            }

            set { reg.SetValue("EndpointSelections", value, RegistryValueKind.DWord); }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // String encryption and decryption

        // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private const string initVector = "WqFoWd5tnVSPjo37";

        // This constant is used to determine the keysize of the encryption algorithm.
        private const int keysize = 256;

        private static string Encrypt(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }

        private static string Decrypt(string cipherText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }

        #endregion

        #region HelperMethods
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Helper application methods

        private string FindOnPath(string executable)
        {
            var pathvar = Environment.GetEnvironmentVariable("PATH");

            foreach (var d in pathvar.Split(';'))
            {
                var fullpath = Path.Combine(d, executable);

                FileInfo fi = new FileInfo(fullpath.ToString());
                
                if (fi.Exists)
                    return fullpath;
            }

            // Not found - return nothing
            return "";
        }

        public string PSExecPath
        {
            get
            {
                object o = reg.GetValue("PSExecPath");
                string path;

                if (o == null)
                {
                    path = FindOnPath("psexec.exe");
                }
                else
                {
                    path = (string)o;
                }

                if (File.Exists(path))
                    // PSExec found - return it
                    return path;
                else
                    // PSExec not found - return a blank
                    return "";
            }

            set { reg.SetValue("PSExecPath", value, RegistryValueKind.String); }
        }

        public string DefaultConfigDirectory
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WSUSAdminAssistant"); }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Miscellaneous preferences

        public bool RunWithLocalCreds
        {
            get
            {
                object o = reg.GetValue("RunWithLocalCreds");

                if (o == null)
                {
                    return false;
                }
                else
                {
                    return Convert.ToBoolean((int)reg.GetValue("RunWithLocalCreds"));
                }
            }

            set { reg.SetValue("RunWithLocalCreds", Convert.ToInt32(value), RegistryValueKind.DWord); }
        }

        private List<string> _hidegroups = null;
        private DateTime _hgupdated = DateTime.MinValue;

        public List<string> HideGroups
        {
            get
            {
                // If we've already retrieved a copy of the hidden groups in the last 10 seconds, just return that object
                if (_hidegroups != null && DateTime.Now.Subtract(_hgupdated).TotalSeconds < 10)
                    return _hidegroups;

                // Retrieve a copy of the hidden groups from the registry
                object o = reg.GetValue("HideGroups");

                if (o == null)
                    // Nothing stored in the registry - return an empty collection
                    _hidegroups = new List<string>();
                else
                    // Convert returned object to a List<string>
                    _hidegroups = new List<string>((string[])o);

                // Note time and return hidden groups
                _hgupdated = DateTime.Now;
                return _hidegroups;
            }

            set
            {
                // Set registry and update cached values
                reg.SetValue("HideGroups", value.ToArray(), RegistryValueKind.MultiString);
                _hidegroups = value;
                _hgupdated = DateTime.Now;
            }
        }

        #endregion

        #region DefaultSUSID
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Default SUD ID methods
        private string[] _susids = null;
        private DateTime _susupdated = DateTime.MinValue;

        public string[] DefaultSusIDCollection
        {
            get
            {
                // Check modified date of XML file
                DateTime lastmod = File.GetLastWriteTime(SusIdXmlFile);

                // Have the group update rules been updated since we last loaded it, or has it never been loaded?
                if (_susids != null && _susupdated == lastmod)
                    // Yes they have - return them.
                    return _susids;

                _susids = ReadSusIdXML(this.SusIdXmlFile);
                _susupdated = DateTime.Now;

                return _susids;
            }

            set
            {
                WriteSusIdXML(value, this.SusIdXmlFile);
                _susids = value;
                _susupdated = DateTime.Now;
            }
        }

        public string SusIdXmlFile
        {
            get
            {
                object o = reg.GetValue("SusIdXmlFile");
                string xmlpath ;

                if (o == null)
                {
                    xmlpath = Path.Combine(this.DefaultConfigDirectory, "DefaultSusIds.xml");
                }
                else
                {
                    xmlpath = (string)o;
                }

                // Check if XML file exists
                if (File.Exists(xmlpath))
                {
                    // It exists - return path
                    return xmlpath;
                }
                else
                {
                    // It doesn't exist - try to create it
                    try
                    {
                        // Does the directory exist?
                        if (!Directory.Exists(Path.GetDirectoryName(xmlpath)))
                            // No it doesn't - try and create it
                            Directory.CreateDirectory(Path.GetDirectoryName(xmlpath));

                        WriteSusIdXML(new string[0], xmlpath);

                        // If we get here, it wrote successfully, so return the file
                        return xmlpath;
                    }
                    catch
                    {
                        // Create failed - return no path
                        return "";
                    }
                }
            }

            set { reg.SetValue("SusIdXmlFile", value, RegistryValueKind.String); }
        }

        private void WriteSusIdXML(string[] s, string file)
        {
            // Create XML file (overwriting it if it already exists)
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                XmlSerializer xs = new XmlSerializer(typeof(string[]));
                xs.Serialize(fs, s);
            }
        }

        private string[] ReadSusIdXML(string file)
        {
            // Open the XML file and read the config.  Let the exceptions fly for other procedures to handle
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                string[] s;
                XmlSerializer xs = new XmlSerializer(typeof(string[]));
                s = (string[])xs.Deserialize(fs);
                return s;
            }
        }

        #endregion

        #region ComputerGroupRegex
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Computer Group Regex matching methods
        public class ComputerGroupRegEx
        {
            public int Priority = 0;
            public string ComputerNameRegex;
            public string IPRegex;
            public string ComputerGroup;
            public string Comment;
            public bool Enabled;
        }

        public class ComputerGroupRegexCollection : System.Collections.CollectionBase
        {
            public ComputerGroupRegEx this[int index]
            {
                get { return (ComputerGroupRegEx) this.List[index]; }
                set { this.List[index] = value;}
            }

            public int Add(ComputerGroupRegEx add)
            {
                // Add item to list
                return this.List.Add(add);
            }

            public void Remove(int index)
            {
                // Ensure the index number falls within the correct range
                if (index > this.Count - 1 || index < 0)
                    // Out of bounds
                    throw new IndexOutOfRangeException();
                else
                    // Remove item from list
                    this.List.RemoveAt(index);
            }

            public void SortByPriority()
            {
                IComparer sorter = new PrioritySortHelper();
                InnerList.Sort(sorter);
            }

            public class PrioritySortHelper : System.Collections.IComparer
            {
                public int Compare(object x, object y)
                {
                    ComputerGroupRegEx r1 = (ComputerGroupRegEx)x;
                    ComputerGroupRegEx r2 = (ComputerGroupRegEx)y;

                    if (r1.Priority > r2.Priority) return 1;
                    if (r1.Priority < r2.Priority) return -1;

                    // Items are identical
                    return 0;
                }
            }

            public ComputerGroupRegEx MatchPC(string fulldomainname, string ip)
            {
                ComputerGroupRegEx rx = null;

                foreach (clsConfig.ComputerGroupRegEx r in this)
                {
                    // Assume the rules match unless one of the two regex expressions is non-empty and does not match or the rule is not enabled
                    bool matched = true;

                    if (!r.Enabled) matched = false;

                    if (r.ComputerNameRegex != null && r.ComputerNameRegex != "")
                    {
                        Match m = Regex.Match(fulldomainname, r.ComputerNameRegex);
                        if (!m.Success) matched = false;
                    }

                    if (r.IPRegex != null && r.IPRegex != "")
                    {
                        Match m = Regex.Match(ip, r.IPRegex);
                        if (!m.Success) matched = false;
                    }

                    if (matched)
                    {
                        // If we have a match, note it and break out
                        rx = r;
                        break;
                    }
                }

                // Return the rule that was matched (or null if no rule was matched)
                return rx;
            }
        }

        public string ComputerRegExXMLFile
        {
            get
            {
                object o = reg.GetValue("ComputerRegExXmlFile");
                string xmlpath;

                if (o == null)
                    xmlpath = Path.Combine(this.DefaultConfigDirectory, "ComputerRegEx.xml");
                else
                    xmlpath = (string)o;

                // Check if XML file exists
                if (File.Exists(xmlpath))
                    // It exists - return path
                    return xmlpath;
                else
                {
                    // It doesn't exist - try to create it
                    try
                    {
                        // Does the directory exist?
                        if (!Directory.Exists(Path.GetDirectoryName(xmlpath)))
                            // No it doesn't - try and create it
                            Directory.CreateDirectory(Path.GetDirectoryName(xmlpath));

                        WriteComputerRegExXML(new ComputerGroupRegexCollection(), xmlpath);

                        // If we get here, it wrote successfully, so return the file
                        return xmlpath;
                    }
                    catch
                    {
                        // Create failed - return no path
                        return "";
                    }
                }
            }

            set { reg.SetValue("ComputerRegExXMLFile", value, RegistryValueKind.String); }
        }

        private void WriteComputerRegExXML(ComputerGroupRegexCollection c, string file)
        {
            // Create XML file (overwriting it if it already exists)
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                XmlSerializer xs = new XmlSerializer(typeof(ComputerGroupRegexCollection));
                xs.Serialize(fs, c);
            }
        }

        private ComputerGroupRegexCollection ReadComputerRegExXML(string file)
        {
            // Open the XML file and read the config.  Let the exceptions fly for other procedures to handle
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                ComputerGroupRegexCollection c = new ComputerGroupRegexCollection();
                XmlSerializer xs = new XmlSerializer(typeof(ComputerGroupRegexCollection));
                c = (ComputerGroupRegexCollection)xs.Deserialize(fs);
                return c;
            }
        }

        ComputerGroupRegexCollection _regexlist = null;
        DateTime _upregex = DateTime.MinValue;

        public ComputerGroupRegexCollection ComputerRegExList
        {
            get
            {
                // Check modified date of XML file
                DateTime lastmod = File.GetLastWriteTime(ComputerRegExXMLFile);

                // Have the group update rules been updated since we last loaded it, or has it never been loaded?
                if (_regexlist != null && _upregex == lastmod)
                    // Yes they have - return them.
                    return _regexlist;

                // Try and read the file
                try
                {
                    _regexlist = ReadComputerRegExXML(this.ComputerRegExXMLFile);

                    // If we got here, it read correctly - return the collection
                    _upregex = DateTime.Now;
                    return _regexlist;
                }
                catch
                {
                    // Something went wrong - return a null collection
                    return null;
                }
            }

            set
            {
                WriteComputerRegExXML(value, this.ComputerRegExXMLFile);
                _regexlist = value;
                _upregex = DateTime.Now;
            }
        }

        #endregion

        #region SecurityCredentials
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Security credential methods and classes

        public class SecurityCredential
        {
            [XmlIgnore] public IPAddress ipaddress;

            public string ip
            {
                get { return ipaddress.ToString(); }
                set { ipaddress = IPAddress.Parse(value); }
            }

            public byte netmask;
            public string description;

            public string domain;
            public string username;
            public string encryptedpassword;

            [XmlIgnore] public string password
            {
                get
                {
                    string pwd;

                    // Try to decrypt password
                    try
                    {
                        pwd = Decrypt(encryptedpassword, "cred");
                    }
                    catch
                    {
                        // Decryption failed - return empty password
                        pwd = "";
                    }

                    return pwd;
                }

                set { encryptedpassword = Encrypt(value, "cred"); }
            }
        }

        public class CredentialCollection : System.Collections.CollectionBase
        {
            public SecurityCredential this[int index]
            {
                get { return (SecurityCredential)this.List[index]; }
                set { this.List[index] = value; }
            }

            private class clsIPSubnet
            {
                private byte[] _address;
                private byte _prefixlen;

                public clsIPSubnet(IPAddress network, byte cidr)
                {
                    _address = network.GetAddressBytes();
                    _prefixlen = cidr;
                }

                public bool Contains(IPAddress ip)
                {
                    byte[] ipbytes = ip.GetAddressBytes();

                    // Are the two address types the same?
                    if (_address.Length != ipbytes.Length)
                        // IPv4 and IPv6 addresses never match
                        return false;

                    int index = 0;
                    int bits = _prefixlen;

                    // Loop though each byte fully covered by the CIDR length
                    for (; bits >=8; bits -=8)
                    {
                        if (ipbytes[index] != _address[index])
                            // If any byte fully covered does not match, the address does not match
                            return false;

                        ++index;
                    }

                    // Are there any leftover bits?
                    if (bits > 0)
                    {
                        // Yes - calculate the leftover bits
                        int mask = (byte)~(255 >> bits);

                        // Do the bytes (appropriately masked) match?
                        if ((ipbytes[index] & mask) != (_address[index] & mask))
                            // No - no match.
                            return false;
                    }

                    // Yes, they match
                    return true;
                }
            }

            public SecurityCredential this[IPAddress ip]
            {
                get
                {
                    // Loop through each credential in our list, returning the first one that matches
                    foreach (SecurityCredential sc in this.List)
                    {
                        // Do the addresses match?
                        if (new clsIPSubnet(sc.ipaddress, sc.netmask).Contains(ip))
                            // Yes, return this credential
                            return sc;
                    }

                    // Couldn't find a matching subnet, return null
                    return null;
                }
            }

            public int Add(SecurityCredential add)
            {
                return this.List.Add(add);
            }

            public void Remove(int index)
            {
                // Check index is within boundaries
                if (index > this.Count - 1 || index < 0)
                    // Out of bounds
                    throw new IndexOutOfRangeException();
                else
                    this.List.RemoveAt(index);
            }

            public void SortByMaskedIP()
            {
                IComparer sorter = new MaskSortHelper();
                InnerList.Sort(sorter);
            }

            public class MaskSortHelper : System.Collections.IComparer
            {
                private byte[] IPMask(IPAddress ip, byte netmask)
                {
                    byte[] i = ip.GetAddressBytes();

                    int bits = netmask;

                    for (int b = 0; b < i.Length; b++)
                    {
                        if (bits > 8)
                            // No action required, move on to the next load of bits
                            bits -= 8;
                        else if (bits == 0)
                            // Mask out all bytes in this byte
                            i[b] = 255;
                        else
                        {
                            // Calculate what's left of the bitmask
                            int msk = (byte)(255 >> bits);

                            // Mask this byte appropriately
                            i[b] = (byte)(i[b] | msk);

                            // No bits left to process
                            bits = 0;
                        }
                    }

                    return i;
                }

                public int Compare(object x, object y)
                {
                    SecurityCredential r1 = (SecurityCredential)x;
                    SecurityCredential r2 = (SecurityCredential)y;

                    byte[] i1 = IPMask(r1.ipaddress, r1.netmask);
                    byte[] i2 = IPMask(r2.ipaddress, r2.netmask);

                    // If IP address lengths are different, sort by IPv4 first
                    if (i1.Length > i2.Length) return -1;
                    if (i1.Length < i2.Length) return 1;

                    // Find first byte that differs and compare them
                    for (int i = 0; i < i1.Length; i++)
                    {
                        if (i1[i] > i2[i]) return 1;
                        if (i1[i] < i2[i]) return -1;
                    }

                    // Masked IPs are the same - sort by the most specific netmask
                    if (r1.netmask > r2.netmask) return -1;
                    if (r1.netmask < r2.netmask) return 1;

                    // IPs and masks are exactly the same.
                    return 0;
                }
            }
        }

        public string CredentialXmlFile
        {
            get
            {
                object o = reg.GetValue("CredentialXmlFile");
                string xmlpath;

                if (o == null)
                    xmlpath = Path.Combine(this.DefaultConfigDirectory, "CredentialXmlFile.xml");
                else
                    xmlpath = (string)o;

                // Check if XML file exists
                if (File.Exists(xmlpath))
                    // It exists - return path
                    return xmlpath;
                else
                {
                    // It doesn't exist - try to create it
                    try
                    {
                        // Does the directory exist?
                        if (!Directory.Exists(Path.GetDirectoryName(xmlpath)))
                            // No it doesn't - try and create it
                            Directory.CreateDirectory(Path.GetDirectoryName(xmlpath));

                        WriteComputerRegExXML(new ComputerGroupRegexCollection(), xmlpath);

                        // If we get here, it wrote successfully, so return the file
                        return xmlpath;
                    }
                    catch
                    {
                        // Create failed - return no path
                        return "";
                    }
                }
            }

            set { reg.SetValue("CredentialXmlFile", value, RegistryValueKind.String); }
        }

        private void WriteCredentialXML(CredentialCollection c, string file)
        {
            // Create XML file (overwriting it if it already exists)
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                XmlSerializer xs = new XmlSerializer(typeof(CredentialCollection));
                xs.Serialize(fs, c);
            }
        }

        private CredentialCollection ReadCredentialXML(string file)
        {
            // Open the XML file and read the config.  Let the exceptions fly for other procedures to handle
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                CredentialCollection c = new CredentialCollection();
                XmlSerializer xs = new XmlSerializer(typeof(CredentialCollection));
                c = (CredentialCollection)xs.Deserialize(fs);
                return c;
            }
        }

        private CredentialCollection _credentials = null;
        private DateTime _crupdated = DateTime.MinValue;

        public CredentialCollection CredentialList
        {
            get
            {
                // Check modified date of XML file
                DateTime lastmod = File.GetLastWriteTime(CredentialXmlFile);

                // Have the group update rules been updated since we last loaded it, or has it never been loaded?
                if (_credentials != null && _crupdated == lastmod)
                    // Yes they have - return them.
                    return _credentials;

                // Try and read the file
                try
                {
                    _credentials = ReadCredentialXML(this.CredentialXmlFile);
                    _crupdated = DateTime.Now;

                    // If we got here, it read correctly - return the collection
                    return _credentials;
                }
                catch
                {
                    // Something went wrong - return an empty collection
                    return new CredentialCollection();
                }
            }

            set
            {
                WriteCredentialXML(value, this.CredentialXmlFile);
                _credentials = value;
                _crupdated = DateTime.Now;
            }
        }

        //// Function to obtain a login token for remote servers using different credentials
        //private class LoginToken : IDisposable
        //{
        //    [System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError = true)]
        //    static extern bool LogonUser(string pszUsername, string pszDomain, string pszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
            
        //    [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        //    extern static bool CloseHandle(IntPtr handle);

        //    public IntPtr Token = IntPtr.Zero;

        //    public LoginToken(string username, string password, string domain)
        //    {
        //        const int LOGON32_PROVIDER_DEFAULT = 0;
        //        const int LOGON32_LOGON_INTERACTIVE = 2;

        //        bool loggedOn = LogonUser(username, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref Token);

        //        if (!loggedOn)
        //        {
        //            throw new UnauthorizedAccessException("Logon failed");
        //        }

        //        // Got our token
        //    }

        //    public void ReleaseLoginToken()
        //    {
        //        if (Token != IntPtr.Zero)
        //            CloseHandle(Token);

        //        Token = IntPtr.Zero;
        //    }

        //    public void Dispose()
        //    {
        //        Dispose(true);
        //    }

        //    public override void Dispose(bool disposing)
        //    {
        //        // Only clean up if we have a valid token and we're supposed to be doing it.
        //        if (Token != IntPtr.Zero && disposing)
        //        {
        //            ReleaseLoginToken();
        //            GC.SuppressFinalize(this);
        //        }
        //    }
        //}

        #endregion

        #region WSUSComputerGroupRules
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // WSUS Computer Group Update Rules

        public class GroupUpdateRule
        {
            private clsWSUS wsus;

            // This constructor is private so that we can serialize and and deserialize properly without exposing it for use within code
            private GroupUpdateRule()
            {
                // Since no wsus object has been passed, null the wsus object so that GUIDs will be stored and loaded later
                wsus = null;
            }

            public GroupUpdateRule(clsWSUS wsusobject)
            {
                wsus = wsusobject;
            }

            public void SetWSUSObject(clsWSUS wsusobject)
            {
                // Store the object for further use
                wsus = wsusobject;

                // Did we store any GUIDs for later update?
                if (_storedgroupguid != null)
                    // Re-set variable
                    computergroupid = _storedgroupguid;

                if (_storedparentguid != null)
                    // Re-set variable
                    parentcomputergroupid = _storedparentguid;
            }

            public int displayorder;                                        // Display order on Unapproved Updates grid
            [DefaultValueAttribute(-1)] public int sortweight;              // Sort weight

            [XmlIgnore] public IComputerTargetGroup computergroup;
            [DefaultValueAttribute("")] public string shortname;            // The short displayname for the group
            [XmlIgnore] public TimeSpan updateinterval;                     // The interval after the update is approved for the parent group (or after first being downloaded if no parent group) that updates should be approved for this group
            [XmlIgnore] public IComputerTargetGroup parentcomputergroup;    // Parent update group for update approval purposes.  Null if this is a first-release group
            [XmlIgnore] public TimeSpan childupdateinterval;                // The interval after which child computer groups may install updates if no computer within this group requires the update

            // Extra classes for serialization
            [XmlIgnore] private string _storedgroupguid = null;

            [XmlElement]
            public string computergroupid
            {
                get
                { return computergroup.Id.ToString(); }

                set
                {
                    // Do we have a wsus object to work with?
                    if (wsus == null)
                    {
                        // Store guid temporarily so we can load details later
                        _storedgroupguid = value;
                        return;
                    }

                    // Are we setting a null group?
                    if (value == "")
                    {
                        computergroup = null;
                        return;
                    }
                    else
                    {
                        // Loop through list of computer groups and find the one we reference
                        foreach (IComputerTargetGroup g in wsus.ComputerGroups)
                            if (g.Id.ToString() == value)
                            {
                                // Found the right group - return it
                                computergroup = g;
                                return;
                            }
                    }

                    // Didn't find the right computer group - return null
                    throw new WsusObjectNotFoundException("Computer group ID " + value + " does not correspond to any known WSUS computer group");
                }
            }

            [XmlElement]
            public long updateintervalticks
            {
                get { return updateinterval.Ticks; }
                set { updateinterval = new TimeSpan(value); }
            }

            [XmlIgnore] private string _storedparentguid = null;

            [XmlElement] public string parentcomputergroupid
            {
                get
                {
                    if (parentcomputergroup == null)
                        return "";
                    else
                        return parentcomputergroup.Id.ToString();
                }

                set
                {
                    // Do we have a wsus object to work with?
                    if (wsus == null)
                    {
                        // Store guid temporarily so we can load details later
                        _storedparentguid = value;
                        return;
                    }

                    // Are we setting a null group?
                    if (value == "")
                    {
                        // Yes.
                        parentcomputergroup = null;
                        return;
                    }
                    else
                    {
                        // Loop through list of computer groups and find the one we reference
                        foreach (IComputerTargetGroup g in wsus.ComputerGroups)
                            if (g.Id.ToString() == value)
                            {
                                // Found the right group - set it
                                parentcomputergroup = g;
                                return;
                            }
                    }

                    // Didn't find the right computer group - return null
                    parentcomputergroup = null;
                }
            }
            
            [XmlElement] public long childupdateintervalticks
            {
                get { return childupdateinterval.Ticks; }
                set { childupdateinterval = new TimeSpan(value); }
            }
        }

        public class GroupUpdateRuleCollection : System.Collections.CollectionBase
        {
            public GroupUpdateRule this[int index]
            {
                get { return (GroupUpdateRule)this.List[index]; }
                set { this.List[index] = value; }
            }

            public GroupUpdateRule this[IComputerTargetGroup index]
            {
                get
                {
                    // Loop through all groups in the collection, returning the first match
                    foreach (GroupUpdateRule cg in this.List)
                        if (cg.computergroup == index)
                            return cg;

                    // None found - return null
                    return null;
                }

                set
                {
                    // Loop through all groups in the collection, updating the first match
                    for (int i = 0; i < this.List.Count; i++)
                    {
                        GroupUpdateRule cg = (GroupUpdateRule)this.List[i];
                        if (cg.computergroup == index)
                        {
                            // Got one - update it
                            cg = value;
                            return;
                        }
                    }

                    // Couldn't find a group - throw exception
                    throw new KeyNotFoundException();
                }
            }

            public GroupUpdateRule this[string groupname]
            {
                get
                {
                    // Loop through all groups in the collection, returning the first match
                    foreach (GroupUpdateRule cg in this.List)
                        if (cg.computergroup.Name == groupname)
                            return cg;

                    // None found - return null
                    return null;
                }

                set
                {
                    // Loop through all groups in the collection, updating the first match
                    for (int i = 0; i < this.List.Count; i++)
                    {
                        GroupUpdateRule cg = (GroupUpdateRule)this.List[i];
                        if (cg.computergroup.Name == groupname)
                        {
                            // Got one - update it
                            cg = value;
                            return;
                        }
                    }

                    // Couldn't find a group - throw exception
                    throw new KeyNotFoundException();
                }
            }

            public int Add(GroupUpdateRule add)
            {
                return this.List.Add(add);
            }

            public void Add(GroupUpdateRuleCollection add)
            {
                // Add all the update rules provided
                foreach (GroupUpdateRule ur in add)
                    this.List.Add(ur);
            }

            public int AddEdit(GroupUpdateRule add)
            {
                // Loop through collection to see if we can match an existing rule
                for (int i = 0; i < this.List.Count; i++)
                {
                    GroupUpdateRule ur = (GroupUpdateRule) this.List[i];
                    
                    if (ur.computergroup.Id == add.computergroup.Id)
                    {
                        // Found a matching group - update it and exit
                        this.List[i] = add;
                        return i;
                    }
                }

                // We didn't find one, so add it.
                return this.List.Add(add);
            }

            public void Remove(int index)
            {
                // Check index falls within acceptable range
                if (index < 0 || index > this.List.Count - 1)
                    // Index out of range.
                    throw new ArgumentOutOfRangeException();

                // Remove list item
                this.List.RemoveAt(index);
            }

            public void Remove(string groupname)
            {
                // Loop through all items and remove the offending one
                for (int i = 0; i < this.List.Count; i++)
                {
                    GroupUpdateRule ur = (GroupUpdateRule)this.List[i];

                    if (ur.computergroup.Name == groupname)
                    {
                        // Found it - remove!
                        this.Remove(i);
                        return;
                    }
                }

                // Didn't find it - throw an exception
                throw new ArgumentOutOfRangeException("Computer group " + groupname + " not in collection");
            }

            public int MaxDisplayOrder
            {
                get
                {
                    int o = -1;

                    // Loop through each rule and find the highest display order
                    foreach (GroupUpdateRule ur in this.List)
                        if (ur.displayorder > o) o = ur.displayorder;

                    // Return the maximum displayorder found.  Default is -1 if array is empty
                    return o;
                }
            }

            public GroupUpdateRuleCollection ChildGroups(GroupUpdateRule rule)
            {
                // Default is not to recurse if no parameter is provided
                return ChildGroups(rule, false);
            }

            public GroupUpdateRuleCollection ChildGroups(GroupUpdateRule rule, bool recursive)
            {
                return AddChildrenOf(rule, recursive);
            }
        
            private GroupUpdateRuleCollection AddChildrenOf(GroupUpdateRule rule, bool recursive)
            {
                GroupUpdateRuleCollection uc = new GroupUpdateRuleCollection();

                // Loop through the collection and determine if the current rule is a child of the current rule
                foreach (GroupUpdateRule ur in this.List)
                {
                    if (ur.parentcomputergroup != null && rule.computergroup.Id == ur.parentcomputergroup.Id)
                    {
                        // Found one - add it to the collection.
                        uc.Add(ur);

                        // Are we recursively adding children?
                        if (recursive)
                            // Yes, also add all the children of this child node
                            uc.Add(AddChildrenOf(ur, true));
                    }
                }

                return uc;
            }

            public void SortByDisplayOrder()
            {
                IComparer sorter = new DisplayOrderHelper();
                InnerList.Sort(sorter);
            }

            public class DisplayOrderHelper : System.Collections.IComparer
            {
                public int Compare(object x, object y)
                {
                    GroupUpdateRule r1 = (GroupUpdateRule)x;
                    GroupUpdateRule r2 = (GroupUpdateRule)y;

                    if (r1.displayorder > r2.displayorder) return 1;
                    if (r1.displayorder < r2.displayorder) return -1;

                    // Items are identical
                    return 0;
                }
            }

            public bool Contains(string groupname)
            {
                // Loop through array looking for a matching group name
                foreach (GroupUpdateRule ur in this.List)
                {
                    if (ur.computergroup.Name == groupname)
                        // Got a match
                        return true;
                }

                // No match found
                return false;
            }
        }
        
        public string GroupUpdateRulesXMLFile
        {
            get
            {
                object o = reg.GetValue("GroupUpdateRulesXmlFile");
                string xmlpath;

                if (o == null)
                    xmlpath = Path.Combine(this.DefaultConfigDirectory, "GroupUpdateRules.xml");
                else
                    xmlpath = (string)o;

                // Check if XML file exists
                if (File.Exists(xmlpath))
                    // It exists - return path
                    return xmlpath;
                else
                {
                    // It doesn't exist - try to create it
                    try
                    {
                        // Does the directory exist?
                        if (!Directory.Exists(Path.GetDirectoryName(xmlpath)))
                            // No it doesn't - try and create it
                            Directory.CreateDirectory(Path.GetDirectoryName(xmlpath));

                        WriteComputerGroupUpdateRulesXML(new GroupUpdateRuleCollection(), xmlpath);

                        // If we get here, it wrote successfully, so return the file
                        return xmlpath;
                    }
                    catch
                    {
                        // Create failed - return no path
                        return "";
                    }
                }
            }

            set { reg.SetValue("GroupUpdateRulesXMLFile", value, RegistryValueKind.String); }
        }

        private void WriteComputerGroupUpdateRulesXML(GroupUpdateRuleCollection c, string file)
        {
            // Create XML file (overwriting it if it already exists)
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                XmlSerializer xs = new XmlSerializer(typeof(GroupUpdateRuleCollection));
                xs.Serialize(fs, c);
            }
        }

        private GroupUpdateRuleCollection ReadComputerGroupRuleXML(string file)
        {
            // Open the XML file and read the config.  Let the exceptions fly for other procedures to handle
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                GroupUpdateRuleCollection c = new GroupUpdateRuleCollection();
                XmlSerializer xs = new XmlSerializer(typeof(GroupUpdateRuleCollection));
                c = (GroupUpdateRuleCollection)xs.Deserialize(fs);

                // Apply appropriate defaults to any field that had no value in the XML file and apply WSUS object so group details can be loaded
                foreach (GroupUpdateRule ur in c)
                {
                    ur.SetWSUSObject(wsus);

                    if (ur.sortweight == -1) ur.sortweight = ur.displayorder;
                    if (ur.shortname == null) ur.shortname = ur.computergroup.Name;
                }

                return c;
            }
        }

        private GroupUpdateRuleCollection _groupupdaterules = null;
        private DateTime _gurupdated = DateTime.MinValue;

        public GroupUpdateRuleCollection GroupUpdateRules
        {
            get
            {
                DateTime lastmod;

                // Does XML file exist?
                if (File.Exists(GroupUpdateRulesXMLFile))
                {
                    // Yes, check modified date of XML file
                    lastmod = File.GetLastWriteTime(GroupUpdateRulesXMLFile);
                }
                else
                {
                    // No, return bogus date to force reload
                    lastmod = DateTime.MinValue;
                }

                // Have the group update rules been updated since we last loaded it, or has it never been loaded?
                if (_groupupdaterules != null && _gurupdated == lastmod)
                    // Yes they have - return them.
                    return _groupupdaterules;

                // No they haven't - read them.
                try
                {
                    _groupupdaterules = ReadComputerGroupRuleXML(GroupUpdateRulesXMLFile);
                    _gurupdated = lastmod;

                    return _groupupdaterules;
                }
                catch
                {
                    // Error reading ComputerGroupRules - return an empty collection
                    return new GroupUpdateRuleCollection();
                }
            }

            set
            {
                // Write XML file, update saved rules and last modified time
                WriteComputerGroupUpdateRulesXML(value, GroupUpdateRulesXMLFile);

                _gurupdated = File.GetLastWriteTime(GroupUpdateRulesXMLFile);
                _groupupdaterules = value;
            }
        }

        #endregion

        #region IgnoreComputerGroups
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Ignore Computer Groups methods
        private Dictionary<Guid, string> _ignoregroups = new Dictionary<Guid, string>();
        private DateTime _ignoregroupsupdated = DateTime.MinValue;

        public Dictionary<Guid, string> IgnoreComputerGroupCollection
        {
            get
            {
                // Check modified date of XML file
                DateTime lastmod = File.GetLastWriteTime(IgnoreGroupsXMLFile);

                // Have the group update rules been updated since we last loaded it, or has it never been loaded?
                if (_ignoregroups != null && _ignoregroupsupdated == lastmod)
                    // Yes they have - return them.
                    return _ignoregroups;

                _ignoregroups = ReadIgnoreGroupsXML(this.IgnoreGroupsXMLFile);
                _ignoregroupsupdated = DateTime.Now;

                return _ignoregroups;
            }

            set
            {
                WriteIgnoreGroupsXML(value, this.IgnoreGroupsXMLFile);
                _ignoregroups = value;
                _ignoregroupsupdated = DateTime.Now;
            }
        }

        public string IgnoreGroupsXMLFile
        {
            get
            {
                object o = reg.GetValue("IgnoreGroupsXMLFile");
                string xmlpath;

                if (o == null)
                {
                    xmlpath = Path.Combine(this.DefaultConfigDirectory, "IgnoreGroups.xml");
                }
                else
                {
                    xmlpath = (string)o;
                }

                // Check if XML file exists
                if (File.Exists(xmlpath))
                {
                    // It exists - return path
                    return xmlpath;
                }
                else
                {
                    // It doesn't exist - try to create it
                    try
                    {
                        // Does the directory exist?
                        if (!Directory.Exists(Path.GetDirectoryName(xmlpath)))
                            // No it doesn't - try and create it
                            Directory.CreateDirectory(Path.GetDirectoryName(xmlpath));

                        WriteIgnoreGroupsXML(new Dictionary<Guid, string>(), xmlpath);

                        // If we get here, it wrote successfully, so return the file
                        return xmlpath;
                    }
                    catch
                    {
                        // Create failed - return no path
                        return "";
                    }
                }
            }

            set { reg.SetValue("IgnoreGroupsXMLFile", value, RegistryValueKind.String); }
        }

        private void WriteIgnoreGroupsXML(Dictionary<Guid, string> s, string file)
        {
            // Create XML file (overwriting it if it already exists)
            string[] g = Array.ConvertAll(s.Keys.ToArray(), x => x.ToString());

            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                XmlSerializer xs = new XmlSerializer(typeof(string[]));
                xs.Serialize(fs, g);
            }
        }

        private Dictionary<Guid, string> ReadIgnoreGroupsXML(string file)
        {
            // Open the XML file and read the config.  Let the exceptions fly for other procedures to handle
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                string[] s;
                XmlSerializer xs = new XmlSerializer(typeof(string[]));
                s = (string[])xs.Deserialize(fs);

                // Loop through the resulting string array and create the dictionary
                Dictionary<Guid, string> g = new Dictionary<Guid, string>();

                foreach (string gs in s)
                {
                    Guid gn;

                    // Try to convert the string to a Guid
                    if (Guid.TryParse(gs, out gn))
                    {
                        // Got it.  Try and find the appropriate computer group
                        foreach (IComputerTargetGroup tg in wsus.ComputerGroups)
                        {
                            if (tg.Id == gn)
                            {
                                // Found it - add it to the dictionary.
                                g.Add(tg.Id, tg.Name);
                                break;
                            }
                        }
                    }
                }
                        
                return g;
            }
        }

        public bool ComputerInIgnoredGroup(IComputerTarget c)
        {
            foreach (IComputerTargetGroup g in c.GetComputerTargetGroups())
            {
                // Is this group in the list of ignored computer groups?
                if (IgnoreComputerGroupCollection.Keys.ToArray().Contains(g.Id))
                    // Yes
                    return true;
            }

            // Computer is not in any of the ignored computer groups.
            return false;
        }

        #endregion
    }
}
