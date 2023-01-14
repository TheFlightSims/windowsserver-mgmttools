using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RemoteMsiManager
{
    public class MsiProduct
    {
        #region (Error codes definition)

        private static readonly Dictionary<uint, string> _errorCodes = new Dictionary<uint, string>
        {
            {0,"The action completed successfully."},
            {13, "The data is invalid."},
            {87, "One of the parameters was invalid."},
            {120, "This value is returned when a custom action attempts to call a function that cannot be called from custom actions. The function returns the value ERROR_CALL_NOT_IMPLEMENTED. Available beginning with Windows Installer version 3.0."},
            {1259, "If Windows Installer determines a product may be incompatible with the current operating system, it displays a dialog box informing the user and asking whether to try to install anyway. This error code is returned if the user chooses not to try the installation."},
            {1601, "The Windows Installer service could not be accessed. Contact your support personnel to verify that the Windows Installer service is properly registered."},
            {1602, "The user cancels installation."},
            {1603, "A fatal error occurred during installation."},
            {1604, "Installation suspended, incomplete."},
            {1605, "This action is only valid for products that are currently installed."},
            {1606, "The feature identifier is not registered."},
            {1607, "The component identifier is not registered."},
            {1608, "This is an unknown property."},
            {1609, "The handle is in an invalid state."},
            {1610, "The configuration data for this product is corrupt. Contact your support personnel."},
            {1611, "The component qualifier not present."},
            {1612, "The installation source for this product is not available. Verify that the source exists and that you can access it."},
            {1613, "This installation package cannot be installed by the Windows Installer service. You must install a Windows service pack that contains a newer version of the Windows Installer service."},
            {1614, "The product is uninstalled."},
            {1615, "The SQL query syntax is invalid or unsupported."},
            {1616, "The record field does not exist."},
            {1618, "Another installation is already in progress. Complete that installation before proceeding with this install."},
            {1619, "This installation package could not be opened. Verify that the package exists and is accessible, or contact the application vendor to verify that this is a valid Windows Installer package."},
            {1620, "This installation package could not be opened. Contact the application vendor to verify that this is a valid Windows Installer package."},
            {1621, "There was an error starting the Windows Installer service user interface. Contact your support personnel."},
            {1622, "There was an error opening installation log file. Verify that the specified log file location exists and is writable."},
            {1623, "This language of this installation package is not supported by your system."},
            {1624, "There was an error applying transforms. Verify that the specified transform paths are valid."},
            {1625, "This installation is forbidden by system policy. Contact your system administrator."},
            {1626, "The function could not be executed."},
            {1627, "The function failed during execution."},
            {1628, "An invalid or unknown table was specified."},
            {1629, "The data supplied is the wrong type."},
            {1630, "Data of this type is not supported."},
            {1631, "The Windows Installer service failed to start. Contact your support personnel."},
            {1632, "The Temp folder is either full or inaccessible. Verify that the Temp folder exists and that you can write to it."},
            {1633, "This installation package is not supported on this platform. Contact your application vendor."},
            {1634, "Component is not used on this machine."},
            {1635, "This patch package could not be opened. Verify that the patch package exists and is accessible, or contact the application vendor to verify that this is a valid Windows Installer patch package."},
            {1636, "This patch package could not be opened. Contact the application vendor to verify that this is a valid Windows Installer patch package."},
            {1637, "This patch package cannot be processed by the Windows Installer service. You must install a Windows service pack that contains a newer version of the Windows Installer service."},
            {1638, "Another version of this product is already installed. Installation of this version cannot continue. To configure or remove the existing version of this product, use Add/Remove Programs in Control Panel."},
            {1639, "Invalid command line argument. Consult the Windows Installer SDK for detailed command-line help."},
            {1640, "The current user is not permitted to perform installations from a client session of a server running the Terminal Server role service."},
            {1641, "The installer has initiated a restart. This message is indicative of a success."},
            {1642, "The installer cannot install the upgrade patch because the program being upgraded may be missing or the upgrade patch updates a different version of the program. Verify that the program to be upgraded exists on your computer and that you have the correct upgrade patch."},
            {1643, "The patch package is not permitted by system policy."},
            {1644, "One or more customizations are not permitted by system policy."},
            {1645, "Windows Installer does not permit installation from a Remote Desktop Connection."},
            {1646, "The patch package is not a removable patch package. Available beginning with Windows Installer version 3.0."},
            {1647, "The patch is not applied to this product. Available beginning with Windows Installer version 3.0."},
            {1648, "No valid sequence could be found for the set of patches. Available beginning with Windows Installer version 3.0."},
            {1649, "Patch removal was disallowed by policy. Available beginning with Windows Installer version 3.0."},
            {1650, "The XML patch data is invalid. Available beginning with Windows Installer version 3.0."},
            {1651, "Administrative user failed to apply patch for a per-user managed or a per-machine application that is in advertise state. Available beginning with Windows Installer version 3.0."},
            {1652, "Windows Installer is not accessible when the computer is in Safe Mode. Exit Safe Mode and try again or try using System Restore to return your computer to a previous state. Available beginning with Windows Installer version 4.0."},
            {1653, "Could not perform a multiple-package transaction because rollback has been disabled. Multiple-Package Installations cannot run if rollback is disabled. Available beginning with Windows Installer version 4.5."},
            {1654, "The app that you are trying to run is not supported on this version of Windows. A Windows Installer package, patch, or transform that has not been signed by Microsoft cannot be installed on an ARM computer."},
            {3010, "A restart is required to complete the install. This message is indicative of a success. This does not include installs where the ForceReboot action is run."}
        };

        #endregion (Error codes definition)

        public enum Assignment_Type
        {
            User = 0,
            Computer = 1,
            Unknown = 255
        }

        public enum Install_State
        {
            Bad_Configuration = -6,
            Invalid_Argument = -2,
            Unknown_Package = -1,
            Advertised = 1,
            Absent = 2,
            Installed = 5,
            Unknown = 255
        }

        private static readonly char[] versionSeparator = new char[] { '.' };

        /// <summary>
        /// Constructor for the MsiProduct class.
        /// </summary>
        /// <param name="identifyingNumber">A GUID without curly braces</param>
        /// <param name="name">Name of the product</param>
        /// <param name="version">Version of the product</param>
        public MsiProduct(String identifyingNumber, string name, string version)
        {
            IdentifyingNumber = !String.IsNullOrEmpty(identifyingNumber) ? identifyingNumber : String.Empty;
            Name = !String.IsNullOrEmpty(name) ? name : String.Empty;
            Version = !String.IsNullOrEmpty(version) ? version : String.Empty;
        }

        #region (Properties)

        public Assignment_Type AssignmentType { get; internal set; } = Assignment_Type.Unknown;

        public string Caption { get; internal set; } = String.Empty;

        public string Description { get; internal set; } = String.Empty;

        public string HelpLink { get; internal set; } = String.Empty;

        public string IdentifyingNumber { get; internal set; } = String.Empty;

        public string InstallDate { get; internal set; } = String.Empty;

        public string InstallLocation { get; internal set; } = String.Empty;

        public Install_State InstallState { get; internal set; } = Install_State.Unknown;

        public string InstallSource { get; internal set; } = String.Empty;

        public string Language { get; internal set; } = String.Empty;

        public string LocalPackage { get; internal set; } = String.Empty;

        public string Name { get; internal set; } = String.Empty;

        public string PackageCache { get; internal set; } = String.Empty;

        public string PackageCode { get; internal set; } = String.Empty;

        public string PackageName { get; internal set; } = String.Empty;

        public string ProductID { get; internal set; } = String.Empty;

        public string RegOwner { get; internal set; } = String.Empty;

        public string Transform { get; internal set; } = String.Empty;

        public string UrlInfoAbout { get; internal set; } = String.Empty;

        public string UrlUpdateInfo { get; internal set; } = String.Empty;

        public string Vendor { get; internal set; } = String.Empty;

        public string Version { get; set; } = String.Empty;

        #endregion (Properties)

        #region (Methods)

        /// <summary>
        /// Take a dotted version string (8.0.650.17) and returns a string where all version numbers have been concatened and padded with "0" to get a 5 digits number (00008000000065000017)
        /// </summary>
        /// <param name="dottedVersion">A string like "8.0.650.17"</param>
        /// <returns>A string like "00008000000065000017"</returns>
        public static string GetConcatenatedVersion(string dottedVersion)
        {
            string result = String.Empty;

            try
            {
                string[] splitedVersion = dottedVersion.Split(versionSeparator);
                foreach (string versionNumber in splitedVersion)
                {
                    result += String.Format("{0:00000}", int.Parse(versionNumber));
                }
            }
            catch (Exception) { }

            return result;
        }

        /// <summary>
        /// Returns the installation date formatted like "DD/MM/YYYY"
        /// </summary>
        /// <returns>A string representing the install date formatted as "DD/MM/YYYY"</returns>
        public static string GetFormattedInstallDate(string unformattedDate)
        {
            string date = String.Empty;
            try
            {
                date = String.Format("{2}/{1}/{0}", unformattedDate.Substring(0, 4), unformattedDate.Substring(4, 2), unformattedDate.Substring(6, 2));
            }
            catch (Exception) { }

            return date;
        }

        /// <summary>
        /// Replaces WQL like «%» and «_» characters by corresponding RegExp pattern
        /// </summary>
        /// <param name="pattern">A WQL like string that can contains «%» and «_» characters</param>
        /// <returns>A RegExp pattern where «%» and «_» characters have been replaced</returns>
        public static string GetRegExpPattern(string pattern)
        {
            return pattern.Replace("%", @"[ABCDEF\d\-]*").Replace("_", @"[ABCDEF\d\-]");
        }

        /// <summary>
        /// Determins whether a MsiCode match a RegExp pattern
        /// </summary>
        /// <param name="msiCode">MsiCode to test against</param>
        /// <param name="pattern">A RegExp pattern</param>
        /// <returns>True if the MsiCode match RegExp pattern</returns>
        public static bool PatternMatchMsiCode(string msiCode, string pattern)
        {
            bool result = false;

            if (pattern.Contains("%") || pattern.Contains("_"))
            {
                Regex regExpr = new Regex("^" + GetRegExpPattern(pattern) + "$", RegexOptions.IgnoreCase);
                if (regExpr.IsMatch(msiCode))
                {
                    result = true;
                }
            }
            else
            {
                if (pattern.Length == 36)
                {
                    result = String.Compare(msiCode, pattern, true) == 0;
                }
            }

            return result;
        }

        /// <summary>
        /// Removes all characters from a string that are not include in "ABCDEFabcdef0123456789-;%_"
        /// </summary>
        /// <param name="text">The string from where to remove unwanted characters</param>
        /// <returns>A string where all unwanted characters have been removed</returns>
        public static string RemoveUnvantedCharacters(string text)
        {
            try
            {
                string allowedCharacters = "ABCDEFabcdef0123456789-;%_";

                if (!String.IsNullOrEmpty(text))
                {
                    int index = 0;
                    do
                    {
                        if (!allowedCharacters.Contains(text[index]))
                        {
                            text = text.Remove(index, 1);
                        }
                        else
                            index++;
                    } while (text.Length > index);

                    return text;
                }
            }
            catch (Exception) { }

            return String.Empty;
        }

        /// <summary>
        /// Splits a string using ";" as a separator
        /// </summary>
        /// <param name="textToSplit">Text to split</param>
        /// <returns>A list of string</returns>
        public static List<string> SplitMsiProductCodes(string textToSplit)
        {
            List<string> msiProducts = new List<string>();

            string[] msiProductsArray = textToSplit.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string exception in msiProductsArray)
            {
                msiProducts.Add(exception);
            }

            return msiProducts;
        }

        /// <summary>
        /// Gets the error message for the provided error codes as defined on https://msdn.microsoft.com/en-us/library/windows/desktop/aa376931(v=vs.85).aspx
        /// </summary>
        /// <param name="errorCode">An MSI error code.</param>
        /// <returns>The error message for the provided error code.</returns>
        public static string GetErrorMessage(uint errorCode)
        {
            if (_errorCodes.ContainsKey(errorCode))
                return _errorCodes[errorCode];
            return String.Empty;
        }

        /// <summary>
        /// Allows to know if the error code is indicative of a success. 
        /// </summary>
        /// <param name="errorCode">An MSI error code.</param>
        /// <returns>True if the error code is indicative of a success.</returns>
        public static bool IsSuccess(uint errorCode)
        {
            return (errorCode == 0 || errorCode == 1641 || errorCode == 3010);
        }

        /// <summary>
        /// Allows to know if a successfull installation need a reboot.
        /// </summary>
        /// <param name="errorCode">An error code for a successfull installation.</param>
        /// <returns>True if the installation need a reboot to complete.</returns>
        public static bool IsRebootNeeded(uint errorCode)
        {
            return (errorCode == 1641 || errorCode == 3010);
        }

        #endregion (Methods)
    }
}
