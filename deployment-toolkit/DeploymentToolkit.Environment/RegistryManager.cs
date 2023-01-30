using DeploymentToolkit.Modals;
using DeploymentToolkit.RegistryWrapper;
using Microsoft.Win32;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DeploymentToolkit.ToolkitEnvironment
{
    public static class RegistryManager
    {
        private const string DeploymentToolkitRegistryPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\DeploymentToolkit";
        private const string DeploymentToolkitActiveSequence = "ActiveSequence";
        private const string DeploymentToolkitHistory = "History";
        private const string DeploymentToolkitDeployments = "Deployments";

        private const string ApplicationUninstallPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private static readonly Win64Registry _registry = new Win64Registry();

        private static string _lastDeploymentRegistryKeyPath;

        internal static void VerifyRegistry()
        {
            _logger.Trace("Verifying registry ...");

            var registry = new Win64Registry();
            if(!registry.CreateSubKey(Path.GetDirectoryName(DeploymentToolkitRegistryPath), Path.GetFileName(DeploymentToolkitRegistryPath)))
            {
                _logger.Fatal("Failed to validate registry");
                throw new Exception("Registry corrupt?");
            }

            _logger.Trace("Successfully verified registry");
        }

        private static bool GetDeploymentRegistryKey(string deploymentName, out RegistryKey deploymentRegistryKey)
        {
            _lastDeploymentRegistryKeyPath = Path.Combine(DeploymentToolkitRegistryPath, DeploymentToolkitDeployments, deploymentName);
            deploymentRegistryKey = _registry.OpenSubKey(_lastDeploymentRegistryKeyPath);

            if(deploymentRegistryKey == null)
            {
                return false;
            }

            return true;
        }

        public static List<DeferedDeployment> GetAllDeferedDeployments()
        {
            var result = new List<DeferedDeployment>();

            var path = Path.Combine(DeploymentToolkitRegistryPath, DeploymentToolkitDeployments);
            var subKeys = _registry.GetSubKeys(path);

            if(subKeys == null || subKeys.Length == 0)
            {
                _logger.Trace("No defered deployments available");
                return result;
            }

            foreach(var key in subKeys)
            {
                try
                {
                    var subKey = _registry.OpenSubKey(path, key, false);

                    var deploymentEndDate = (string)subKey.GetValue("DeploymentEndDate", string.Empty);
                    var deploymentDeadlineString = (string)subKey.GetValue("DeploymentDeadline", string.Empty);

                    var deferedDeployment = new DeferedDeployment
                    {
                        Name = key
                    };

                    if(!string.IsNullOrEmpty(deploymentEndDate) && DateTime.TryParse(deploymentEndDate, out var deploymentEndDay))
                    {
                        deferedDeployment.RemainingDays = (int)Math.Ceiling((deploymentEndDay - DateTime.Now).TotalDays);
                    }

                    if(!string.IsNullOrEmpty(deploymentDeadlineString) && DateTime.TryParse(deploymentDeadlineString, out var deploymentDeadline))
                    {
                        deferedDeployment.Deadline = deploymentDeadline;
                    }

                    result.Add(deferedDeployment);
                }
                catch(Exception ex)
                {
                    _logger.Error(ex, $"Failed to process '{key}'");
                }
            }

            return result;
        }

        public static int? GetDeploymentRemainingDays(string deploymentName)
        {
            if(string.IsNullOrEmpty(deploymentName))
            {
                throw new ArgumentException("Empty string not allowed", nameof(deploymentName));
            }

            if(GetDeploymentRegistryKey(deploymentName, out var registryKey))
            {
                // This isn't the first time we are working with this deployment
                _logger.Trace($"Getting 'DeploymentEndDate' for {deploymentName}");
                var deploymentEndDayString = (string)registryKey.GetValue("DeploymentEndDate", string.Empty);
                if(string.IsNullOrEmpty(deploymentEndDayString) || !DateTime.TryParse(deploymentEndDayString, out var deploymentEndDay))
                {
                    _logger.Warn($"Invalid 'DeploymentEndDate' in registry or 'DeploymentEndDate' not found under {_lastDeploymentRegistryKeyPath}");
                }
                else
                {
                    var remainingDays = (int)Math.Ceiling((deploymentEndDay - DateTime.Now).TotalDays);
                    _logger.Trace($"{remainingDays} remaining days for deployment {deploymentName}");
                    return remainingDays;
                }
            }

            // The current deployment is being run the first time
            if(deploymentName == EnvironmentVariables.ActiveSequence.UniqueName)
            {
                _logger.Trace($"Getting remaining days of current deployment {EnvironmentVariables.ActiveSequence.DeferSettings.Days}");

                return EnvironmentVariables.ActiveSequence.DeferSettings.Days;
            }

            _logger.Warn($"Can't get remaining days for {deploymentName} as it was not found in the registry and isn't the current active deployment");
            return null;
        }

        public static DateTime? GetDeploymentDeadline(string deploymentName)
        {
            if(string.IsNullOrEmpty(deploymentName))
            {
                throw new ArgumentException("Empty string not allowed", nameof(deploymentName));
            }

            if(GetDeploymentRegistryKey(deploymentName, out var registryKey))
            {
                // This isn't the first time we are working with this deployment
                var deploymentDeadlineString = (string)registryKey.GetValue("DeploymentDeadline", string.Empty);
                if(string.IsNullOrEmpty(deploymentDeadlineString) || !DateTime.TryParse(deploymentDeadlineString, out var deploymentDeadline))
                {
                    _logger.Warn($"Invalid 'DeploymentDeadline' in registry or 'DeploymentDeadline' not found under {_lastDeploymentRegistryKeyPath}");
                }
                else
                {
                    return deploymentDeadline;
                }
            }

            // The current deployment is being run the first time
            if(deploymentName == EnvironmentVariables.Configuration.Name)
            {
                _logger.Trace($"Getting deadline of current deployment {EnvironmentVariables.ActiveSequence.DeferSettings.Days}");
                var deadline = EnvironmentVariables.ActiveSequence.DeferSettings.DeadlineAsDate;
                if(deadline == DateTime.MinValue)
                {
                    return null;
                }

                return deadline;
            }

            return null;
        }

        public static void SaveDeploymentDeferalSettings()
        {
            var uniqueName = EnvironmentVariables.ActiveSequence.UniqueName;
            var deferalSettings = EnvironmentVariables.ActiveSequence.DeferSettings;
            var deploymentExists = GetDeploymentRegistryKey(uniqueName, out _);
            var deploymentRegistryPath = _lastDeploymentRegistryKeyPath;

            if(deploymentExists)
            {
                _logger.Trace($"{deploymentRegistryPath} already exists. Deleting...");
                _registry.DeleteSubKey(deploymentRegistryPath);
            }

            if(deferalSettings.DeadlineAsDate == DateTime.MinValue && deferalSettings.Days <= 0)
            {
                _logger.Trace($"No need to save deferal settings for {uniqueName} as no settings are specified");
                return;
            }

            _logger.Trace($"Creating '{deploymentRegistryPath}' ...");
            var deploymentRegistryKey = _registry.CreateSubKey(deploymentRegistryPath);

            if(deferalSettings.Days > 0)
            {
                var endDate = DateTime.Now.AddDays(deferalSettings.Days).ToShortDateString();
                _logger.Trace($"Deployment can be defered for {deferalSettings.Days} days. Enddate: {endDate}");
                deploymentRegistryKey.SetValue("DeploymentEndDate", endDate);
            }

            if(deferalSettings.DeadlineAsDate != DateTime.MinValue)
            {
                var deadLine = deferalSettings.Deadline;
                _logger.Trace($"Deployment can be defered until {deadLine}.");
                deploymentRegistryKey.SetValue("Deadline", deadLine);
            }

            _logger.Trace("Successfully saved deferal settings to registry");
        }

        public static void RemoveDeploymentDeferalSettings()
        {
            var uniqueName = EnvironmentVariables.ActiveSequence.UniqueName;
            var deploymentExists = GetDeploymentRegistryKey(uniqueName, out _);
            var deploymentRegistryPath = _lastDeploymentRegistryKeyPath;

            if(deploymentExists)
            {
                _logger.Trace($"{deploymentRegistryPath} exists. Deleting...");
                try
                {
                    _registry.DeleteSubKey(deploymentRegistryPath);
                }
                catch(Exception ex)
                {
                    _logger.Error(ex, $"Error while trying to delete deferal settings for {uniqueName} -> {deploymentRegistryPath}");
                }
            }
            else
            {
                _logger.Trace($"No deferal settings found in registry for deployment {uniqueName}. Nothing to delete ...");
            }
        }

        internal static void SetActiveSequence(ISequence sequence)
        {
            _logger.Trace($"Updating ActiveSequence in registry ...");

            var registry = new Win64Registry();
            var path = Path.Combine(DeploymentToolkitRegistryPath, DeploymentToolkitActiveSequence);

            if(!registry.CreateSubKey(DeploymentToolkitRegistryPath, DeploymentToolkitActiveSequence))
            {
                _logger.Error($"Failed to create '{DeploymentToolkitActiveSequence}' key in '{DeploymentToolkitRegistryPath}'");
                return;
            }

            if(!registry.SetValue(path, "Name", sequence.UniqueName, RegistryValueKind.String))
            {
                _logger.Error($"Failed to set Name in '{path}'");
                return;
            }

            if(!registry.SetValue(path, "Type", EnvironmentVariables.ActiveSequenceType.ToString(), RegistryValueKind.String))
            {
                _logger.Error($"Failed to set Type in '{path}'");
                return;
            }

            if(!registry.SetValue(path, "StartTime", DateTime.Now.ToFileTime().ToString(), RegistryValueKind.String))
            {
                _logger.Error($"Failed to set StartTime in '{path}'");
                return;
            }

            _logger.Trace("ActiveSequence set");
        }

        public static void SetSequenceComplete(SequenceCompletedEventArgs sequenceCompletedEventArgs)
        {
            _logger.Trace("Deleting ActiveSequence keys ...");

            var registry = new Win64Registry();

            var startTime = registry.GetValue(Path.Combine(DeploymentToolkitRegistryPath, DeploymentToolkitActiveSequence), "StartTime");
            if(startTime == null)
            {
                _logger.Warn("Failed to get startTime from ActiveSequence");
                startTime = string.Empty;
            }

            if(!registry.DeleteSubKey(DeploymentToolkitRegistryPath, DeploymentToolkitActiveSequence))
            {
                _logger.Error($"Failed to delete '{DeploymentToolkitActiveSequence}' from '{DeploymentToolkitRegistryPath}'");
                return;
            }

            _logger.Trace($"Updating Sequence history ...");

            var subKeyName = DateTime.Now.ToFileTime().ToString();

            if(!registry.CreateSubKey(DeploymentToolkitRegistryPath, DeploymentToolkitHistory))
            {
                _logger.Error($"Failed to create '{DeploymentToolkitHistory}' in '{DeploymentToolkitRegistryPath}'");
                return;
            }

            var historyPath = Path.Combine(DeploymentToolkitRegistryPath, DeploymentToolkitHistory);
            if(!registry.CreateSubKey(historyPath, subKeyName))
            {
                _logger.Error($"Failed to create '{subKeyName}' in '{historyPath}'");
                return;
            }

            var path = Path.Combine(DeploymentToolkitRegistryPath, DeploymentToolkitHistory, subKeyName);

            var activeSequence = EnvironmentVariables.ActiveSequence;
            if(!registry.SetValue(path, "Name", activeSequence.UniqueName, RegistryValueKind.String))
            {
                _logger.Error($"Failed to set Name in '{path}'");
                return;
            }

            if(!registry.SetValue(path, "Type", EnvironmentVariables.ActiveSequenceType.ToString(), RegistryValueKind.String))
            {
                _logger.Error($"Failed to set Type in '{path}'");
                return;
            }

            if(!registry.SetValue(path, "StartTime", startTime.ToString(), RegistryValueKind.String))
            {
                _logger.Error($"Failed to set StartTime in '{path}'");
                return;
            }

            if(!registry.SetValue(path, "EndTime", DateTime.Now.ToFileTime().ToString(), RegistryValueKind.String))
            {
                _logger.Error($"Failed to set EndTime in '{path}'");
                return;
            }

            if(!registry.SetValue(path, "ExitCode", sequenceCompletedEventArgs.ReturnCode, RegistryValueKind.DWord))
            {
                _logger.Error($"Failed to set ExitCode in '{path}");
                return;
            }

            if(!registry.SetValue(path, "Successful", sequenceCompletedEventArgs.SequenceSuccessful, RegistryValueKind.DWord))
            {
                _logger.Error($"Failed to set Successful in '{path}");
                return;
            }

            if(sequenceCompletedEventArgs.CountErrors > 0)
            {
                if(!registry.CreateSubKey(path, "Errors"))
                {
                    _logger.Error($"Failed to create Errors in '{path}");
                    return;
                }

                var errorPath = Path.Combine(path, "Errors");
                for(var i = 0; i < sequenceCompletedEventArgs.CountErrors; i++)
                {
                    if(!registry.CreateSubKey(errorPath, i.ToString()))
                    {
                        _logger.Error($"Failed to create in '{errorPath}'");
                        continue;
                    }

                    var currentError = sequenceCompletedEventArgs.SequenceErrors[i];
                    var currentErrorPath = Path.Combine(errorPath, i.ToString());

                    if(!registry.SetValue(currentErrorPath, "Message", currentError.Message, RegistryValueKind.String))
                    {
                        _logger.Error($"Failed to set Message in '{currentErrorPath}");
                        return;
                    }
                }
            }

            if(sequenceCompletedEventArgs.CountWarnings > 0)
            {
                if(!registry.CreateSubKey(path, "Warnings"))
                {
                    _logger.Error($"Failed to create Warnings in '{path}");
                    return;
                }

                var warningPath = Path.Combine(path, "Warnings");
                for(var i = 0; i < sequenceCompletedEventArgs.CountWarnings; i++)
                {
                    if(!registry.CreateSubKey(warningPath, i.ToString()))
                    {
                        _logger.Error($"Failed to create in '{warningPath}'");
                        continue;
                    }

                    var currentWarning = sequenceCompletedEventArgs.SequenceWarnings[i];
                    var currentWarningPath = Path.Combine(warningPath, i.ToString());

                    if(!registry.SetValue(currentWarningPath, "Message", currentWarning.Message, RegistryValueKind.String))
                    {
                        _logger.Error($"Failed to set Message in '{currentWarningPath}");
                        return;
                    }
                }
            }
        }

        public static List<UninstallInfo> GetInstalledMSIProgramsByName(string name, bool exact = false)
        {
            var installedPrograms = GetInstalledMSIPrograms();

            if(!exact)
            {
                var result = new List<UninstallInfo>();
                var regex = new Regex(name);
                foreach(var program in installedPrograms)
                {
                    if(regex.Match(program.DisplayName).Success)
                    {
                        _logger.Trace($"Found match: {program.DisplayName}");
                        result.Add(program);
                    }
                }

                return result;
            }
            else
            {
                return installedPrograms.Where((p) => string.Compare(p.DisplayName, name, StringComparison.InvariantCulture) == 0).ToList();
            }
        }

        public static List<UninstallInfo> GetInstalledMSIPrograms()
        {
            var win32Registry = new Win32Registry();
            var keys = win32Registry.GetSubKeys(ApplicationUninstallPath);
            var msiPrograms = GetInstallMSIProgramsInHive(win32Registry, keys);

            if(Environment.Is64BitOperatingSystem)
            {
                var win64Registry = new Win64Registry();
                var win64Keys = win64Registry.GetSubKeys(ApplicationUninstallPath);
                msiPrograms = msiPrograms.Union(GetInstallMSIProgramsInHive(win64Registry, win64Keys)).ToList();
            }

            return msiPrograms;
        }

        private static List<UninstallInfo> GetInstallMSIProgramsInHive(WinRegistryBase registry, string[] keys)
        {
            var result = new List<UninstallInfo>();

            foreach(var key in keys)
            {
                _logger.Trace($"Processing {key}");

                if(!Guid.TryParse(key, out var productId))
                {
                    _logger.Debug($"{key} could not be parsed as guid. Assuming non MSI installation. Skipping");
                    continue;
                }

                var keyPath = Path.Combine(ApplicationUninstallPath, key);
                var program = new UninstallInfo()
                {
                    DisplayName = registry.GetValue(keyPath, "DisplayName")?.ToString() ?? string.Empty,
                    DisplayVersion = registry.GetValue(keyPath, "DisplayVersion")?.ToString() ?? string.Empty,
                    Publisher = registry.GetValue(keyPath, "Publisher")?.ToString() ?? string.Empty,
                    UninstallString = registry.GetValue(keyPath, "UninstallString")?.ToString() ?? string.Empty,
                    ProductId = $@"{{{productId.ToString()}}}"
                };

                if(!program.UninstallString.ToLower().Contains("msiexec"))
                {
                    _logger.Debug($"{key} does not contain 'msiexec' in UninstallString. Skipping");
                    continue;
                }

                result.Add(program);
            }

            return result;
        }
    }
}
