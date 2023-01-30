using DeploymentToolkit.DeploymentEnvironment;
using DeploymentToolkit.Scripting.Extensions;
using DeploymentToolkit.ToolkitEnvironment;
using DeploymentToolkit.ToolkitEnvironment.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;

namespace DeploymentToolkit.Scripting
{
    public static partial class PreProcessor
    {
        private static readonly Dictionary<string, Func<string>> _variables = new Dictionary<string, Func<string>>()
        {
            {
                "Is64Bit",
                delegate()
                {
                    return Environment.Is64BitOperatingSystem.ToIntString();
                }
            },
            {
                "Is64BitProcess",
                delegate()
                {
                    return Environment.Is64BitProcess.ToIntString();
                }
            },
            {
                "Is32Bit",
                delegate()
                {
                    return (!Environment.Is64BitOperatingSystem).ToIntString();
                }
            },
            {
                "Is32BitProcess",
                delegate()
                {
                    return (!Environment.Is64BitProcess).ToIntString();
                }
            },
            {
                "DT_InstallPath",
                delegate()
                {
                    try
                    {
                        return EnvironmentVariables.DeploymentToolkitInstallPath;
                    }
                    catch(DeploymentToolkitInstallPathNotFoundException)
                    {
                        _logger.Warn("Failed to get DeploymentToolkitInstallation Path. Invalid installation?");
                        return "";
                    }
                }
            },
            {
                "DT_FilesPath",
                delegate()
                {
                    return DeploymentEnvironmentVariables.FilesDirectory;
                }
            },
            {
                "DT_DeploymentUniqueName",
                delegate()
                {
                    return EnvironmentVariables.ActiveSequence.UniqueName;
                }
            },
            {
                "DT_IsTaskSequence",
                delegate()
                {
                    return EnvironmentVariables.IsRunningInTaskSequence.ToIntString();
                }
            },

            {
                "LogonUser",
                delegate()
                {
                    return Environment.UserName;
                }
            },
            {
                "LogonDomain",
                delegate()
                {
                    return Environment.UserDomainName;
                }
            },
            {
                "WindowsBuild",
                delegate()
                {
                    return Environment.OSVersion.Version.Build.ToString();
                }
            },
            {
                "ComputerName",
                delegate()
                {
                    return Environment.MachineName;
                }
            },
            {
                "Date",
                delegate()
                {
                    return DateTime.Now.ToShortDateString();
                }
            },
            {
                "Time",
                delegate()
                {
                    return DateTime.Now.ToShortTimeString();
                }
            }
        };

        private static void InitializeEnvironment()
        {
            AddMsiVariables();
            AddEnvironmentVariables();
        }

        private static void AddMsiVariables()
        {
            _logger.Trace($"Adding MsiVariables ...");

            try
            {
                {
                    var culture = NativeFunctions.GetUserDefaultLangID();
                    _variables.Add("UserLanguageID", () => culture.ToString());
                }

                {
                    var culture = NativeFunctions.GetSystemDefaultLangID();
                    _variables.Add("SystemLanguageID", () => culture.ToString());
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Failed to add UserVariables");
            }
        }

        private static void AddEnvironmentVariables()
        {
            _logger.Trace($"Adding EnvironmentVariables ...");

            try
            {
                var variables = Environment.GetEnvironmentVariables();
                if(variables == null || variables.Count == 0)
                {
                    _logger.Trace("No EnvironmentVariables found");
                    return;
                }

                foreach(DictionaryEntry environmentVariable in variables)
                {
                    var name = (string)environmentVariable.Key;

                    if(name.Contains("("))
                    {
                        name = name.Replace("(", "[");
                    }

                    if(name.Contains(")"))
                    {
                        name = name.Replace(")", "]");
                    }

                    var value = (string)environmentVariable.Value;
                    if(_variables.ContainsKey(name))
                    {
                        _logger.Warn($"Cannot add environmentvariable {name} as it already exists");
                        continue;
                    }

                    _variables.Add(name, delegate () {
                        return value;
                    });
                    _logger.Trace($"Added '{name}' with value of '{value}'");
                }

            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Failed to add EnvironmentVariables");
            }
        }

        private static int _uniqueEnvironmentCounter = 0;
        private static readonly Dictionary<string, PowerShell> _powershellEnvironments = new Dictionary<string, PowerShell>();

        public static bool DisposePowerShellEnvironments()
        {
            if(_powershellEnvironments.Count == 0)
            {
                return true;
            }

            var hadException = false;
            foreach(var powerShell in _powershellEnvironments)
            {
                _logger.Trace($"Disposing {powerShell.Key}");
                try
                {
                    powerShell.Value?.Dispose();
                }
                catch(ObjectDisposedException) { }
                catch(Exception ex)
                {
                    _logger.Warn(ex, $"Failed to dispose powershell environment ({powerShell.Key})");
                    hadException = true;
                }
            }

            return !hadException;
        }

        public static bool AddVariable(string name, string script, string environment)
        {
            if(string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if(string.IsNullOrEmpty(script))
            {
                throw new ArgumentNullException(nameof(script));
            }

            if(string.IsNullOrEmpty(environment))
            {
                environment = "UNIQUE";
            }

            if(name.Contains("("))
            {
                name = name.Replace("(", "[");
            }

            if(name.Contains(")"))
            {
                name = name.Replace(")", "]");
            }

            if(_variables.ContainsKey(name))
            {
                _logger.Warn($"Tried to overwrite an already existing variable! ({name})");
                return false;
            }

            if(environment == "UNIQUE")
            {
                environment = GetUniqueEnvironmentName();
            }

            try
            {
#if DEBUG && PREPROCESSOR_TRACE
                Debug.WriteLine($"Running in PowerShell environment '{environment}'");
#endif
                PowerShell powershell;
                if(!_powershellEnvironments.ContainsKey(environment))
                {
                    powershell = PowerShell.Create();
                    _powershellEnvironments.Add(environment, powershell);
                }
                else
                {
                    powershell = _powershellEnvironments[environment];
                }

                powershell.AddScript(script, false);
                powershell.Invoke();
                powershell.Commands.Clear();
                powershell.AddCommand(name);
                var results = powershell.Invoke();
                var result = GetResultFromPSObject(
                    results.Count >= 1 ?
                    results[0] :
                    string.Empty
                );

                _variables.Add(name, delegate () {
                    return result;
                });

                return true;
            }
            catch(Exception ex)
            {
                _logger.Error(ex, $"Error while trying to add CustomVariable {name}");
                return false;
            }
        }

        private static string GetUniqueEnvironmentName()
        {
            var environmentName = string.Empty;
            do
            {
                environmentName = $"UNIQUE_{_uniqueEnvironmentCounter++}";
            }
            while(_powershellEnvironments.ContainsKey(environmentName));

            return environmentName;
        }

        private static string GetResultFromPSObject(PSObject input)
        {
            var type = input.BaseObject.GetType();

            if(type == typeof(bool))
            {
                return ((bool)input.BaseObject).ToIntString();
            }
            else
            {
                return (string)input.BaseObject;
            }
        }
    }
}
