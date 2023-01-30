using DeploymentToolkit.Actions;
using DeploymentToolkit.Scripting.Extensions;
using NLog;
using System;
using static DeploymentToolkit.Actions.RegistryActions;

namespace DeploymentToolkit.Scripting.PreProcessorFunctions
{
    public static class RegistryFunctions
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static string HasKey(string[] parameters)
        {
            if(parameters.Length <= 2)
            {
                return false.ToIntString();
            }

            var architectureString = parameters[0];
            if(string.IsNullOrEmpty(architectureString) || !Enum.TryParse<Architecture>(architectureString, out var architecture))
            {
                return false.ToIntString();
            }

            var path = parameters[1];
            if(string.IsNullOrEmpty(path))
            {
                return false.ToIntString();
            }

            var subKeyName = parameters[2];
            if(string.IsNullOrEmpty(subKeyName))
            {
                return false.ToIntString();
            }

            return RegistryActions.KeyExists(architecture, path, subKeyName).ToIntString();
        }

        public static string CreateKey(string[] parameters)
        {
            if(parameters.Length <= 2)
            {
                return false.ToIntString();
            }

            var architectureString = parameters[0];
            if(string.IsNullOrEmpty(architectureString) || !Enum.TryParse<Architecture>(architectureString, out var architecture))
            {
                return false.ToIntString();
            }

            var path = parameters[1];
            if(string.IsNullOrEmpty(path))
            {
                return false.ToIntString();
            }

            var subKeyName = parameters[2];
            if(string.IsNullOrEmpty(subKeyName))
            {
                return false.ToIntString();
            }

            return RegistryActions.CreateKey(architecture, path, subKeyName).ToIntString();
        }

        public static string CreateKeyForAllUsers(string[] parameters)
        {
            if(parameters.Length <= 2)
            {
                return false.ToIntString();
            }

            var architectureString = parameters[0];
            if(string.IsNullOrEmpty(architectureString) || !Enum.TryParse<Architecture>(architectureString, out var architecture))
            {
                return false.ToIntString();
            }

            var path = parameters[1];
            if(string.IsNullOrEmpty(path))
            {
                return false.ToIntString();
            }

            var subKeyName = parameters[2];
            if(string.IsNullOrEmpty(subKeyName))
            {
                return false.ToIntString();
            }

            var includeDefaultProfile = false;
            var includePublicProfile = false;

            if(parameters.Length > 3 && !string.IsNullOrEmpty(parameters[3]))
            {
                bool.TryParse(parameters[3], out includeDefaultProfile);
            }
            if(parameters.Length > 4 && !string.IsNullOrEmpty(parameters[4]))
            {
                bool.TryParse(parameters[4], out includePublicProfile);
            }

            return RegistryActions.CreateKeyForAllUsers(architecture, path, subKeyName, includeDefaultProfile, includePublicProfile).ToIntString();
        }

        public static string DeleteKey(string[] parameters)
        {
            if(parameters.Length <= 2)
            {
                return false.ToIntString();
            }

            var architectureString = parameters[0];
            if(string.IsNullOrEmpty(architectureString) || !Enum.TryParse<Architecture>(architectureString, out var architecture))
            {
                return false.ToIntString();
            }

            var path = parameters[1];
            if(string.IsNullOrEmpty(path))
            {
                return false.ToIntString();
            }

            var subKeyName = parameters[2];
            if(string.IsNullOrEmpty(subKeyName))
            {
                return false.ToIntString();
            }

            return RegistryActions.DeleteKey(architecture, path, subKeyName).ToIntString();
        }

        public static string DeleteKeyForAllUsers(string[] parameters)
        {
            if(parameters.Length <= 2)
            {
                return false.ToIntString();
            }

            var architectureString = parameters[0];
            if(string.IsNullOrEmpty(architectureString) || !Enum.TryParse<Architecture>(architectureString, out var architecture))
            {
                return false.ToIntString();
            }

            var path = parameters[1];
            if(string.IsNullOrEmpty(path))
            {
                return false.ToIntString();
            }

            var subKeyName = parameters[2];
            if(string.IsNullOrEmpty(subKeyName))
            {
                return false.ToIntString();
            }

            var includeDefaultProfile = false;
            var includePublicProfile = false;

            if(parameters.Length > 3 && !string.IsNullOrEmpty(parameters[3]))
            {
                bool.TryParse(parameters[3], out includeDefaultProfile);
            }
            if(parameters.Length > 4 && !string.IsNullOrEmpty(parameters[4]))
            {
                bool.TryParse(parameters[4], out includePublicProfile);
            }

            return RegistryActions.DeleteKeyForAllUsers(architecture, path, subKeyName, includeDefaultProfile, includePublicProfile).ToIntString();
        }

        public static string HasValue(string[] parameters)
        {
            if(parameters.Length <= 2)
            {
                return false.ToIntString();
            }

            var architectureString = parameters[0];
            if(string.IsNullOrEmpty(architectureString) || !Enum.TryParse<Architecture>(architectureString, out var architecture))
            {
                return false.ToIntString();
            }

            var path = parameters[1];
            if(string.IsNullOrEmpty(path))
            {
                return false.ToIntString();
            }

            var valueName = parameters[2];
            if(string.IsNullOrEmpty(valueName))
            {
                return false.ToIntString();
            }

            return RegistryActions.ValueExists(architecture, path, valueName).ToIntString();
        }

        public static string SetValue(string[] parameters)
        {
            if(parameters.Length <= 4)
            {
                return false.ToIntString();
            }

            var architectureString = parameters[0];
            if(string.IsNullOrEmpty(architectureString) || !Enum.TryParse<Architecture>(architectureString, out var architecture))
            {
                return false.ToIntString();
            }

            var path = parameters[1];
            if(string.IsNullOrEmpty(path))
            {
                return false.ToIntString();
            }

            var valueName = parameters[2];
            if(string.IsNullOrEmpty(valueName))
            {
                return false.ToIntString();
            }

            // Value can be an empty string
            var value = parameters[3];

            var valueType = parameters[4];
            if(string.IsNullOrEmpty(valueType) || !Enum.TryParse<Microsoft.Win32.RegistryValueKind>(valueType, out var valueKind))
            {
                return false.ToIntString();
            }

            return RegistryActions.SetValue(architecture, path, valueName, value, valueKind).ToIntString();
        }

        public static string SetValueForAllUsers(string[] parameters)
        {
            if(parameters.Length <= 4)
            {
                return false.ToIntString();
            }

            var architectureString = parameters[0];
            if(string.IsNullOrEmpty(architectureString) || !Enum.TryParse<Architecture>(architectureString, out var architecture))
            {
                return false.ToIntString();
            }

            var path = parameters[1];
            if(string.IsNullOrEmpty(path))
            {
                return false.ToIntString();
            }

            var valueName = parameters[2];
            if(string.IsNullOrEmpty(valueName))
            {
                return false.ToIntString();
            }

            // Value can be an empty string
            var value = parameters[3];

            var valueType = parameters[4];
            if(string.IsNullOrEmpty(valueType) || !Enum.TryParse<Microsoft.Win32.RegistryValueKind>(valueType, out var valueKind))
            {
                return false.ToIntString();
            }

            var includeDefaultProfile = false;
            var includePublicProfile = false;

            if(parameters.Length > 5 && !string.IsNullOrEmpty(parameters[5]))
            {
                bool.TryParse(parameters[5], out includeDefaultProfile);
            }
            if(parameters.Length > 6 && !string.IsNullOrEmpty(parameters[6]))
            {
                bool.TryParse(parameters[6], out includePublicProfile);
            }

            return RegistryActions.SetValueForAllUsers(architecture, path, valueName, value, valueKind, includeDefaultProfile, includePublicProfile).ToIntString();
        }

        public static string GetValue(string[] parameters)
        {
            if(parameters.Length <= 2)
            {
                return false.ToIntString();
            }

            var architectureString = parameters[0];
            if(string.IsNullOrEmpty(architectureString) || !Enum.TryParse<Architecture>(architectureString, out var architecture))
            {
                return false.ToIntString();
            }

            var path = parameters[1];
            if(string.IsNullOrEmpty(path))
            {
                return false.ToIntString();
            }

            var valueName = parameters[2];
            if(string.IsNullOrEmpty(valueName))
            {
                return false.ToIntString();
            }

            return RegistryActions.GetValue(architecture, path, valueName);
        }

        public static string DeleteValue(string[] parameters)
        {
            if(parameters.Length <= 2)
            {
                return false.ToIntString();
            }

            var architectureString = parameters[0];
            if(string.IsNullOrEmpty(architectureString) || !Enum.TryParse<Architecture>(architectureString, out var architecture))
            {
                return false.ToIntString();
            }

            var path = parameters[1];
            if(string.IsNullOrEmpty(path))
            {
                return false.ToIntString();
            }

            var valueName = parameters[2];
            if(string.IsNullOrEmpty(valueName))
            {
                return false.ToIntString();
            }

            return RegistryActions.DeleteValue(architecture, path, valueName).ToIntString();
        }

        public static string DeleteValueForAllUsers(string[] parameters)
        {
            if(parameters.Length <= 2)
            {
                return false.ToIntString();
            }

            var architectureString = parameters[0];
            if(string.IsNullOrEmpty(architectureString) || !Enum.TryParse<Architecture>(architectureString, out var architecture))
            {
                return false.ToIntString();
            }

            var path = parameters[1];
            if(string.IsNullOrEmpty(path))
            {
                return false.ToIntString();
            }

            var valueName = parameters[2];
            if(string.IsNullOrEmpty(valueName))
            {
                return false.ToIntString();
            }

            var includeDefaultProfile = false;
            var includePublicProfile = false;

            if(parameters.Length > 3 && !string.IsNullOrEmpty(parameters[3]))
            {
                bool.TryParse(parameters[3], out includeDefaultProfile);
            }
            if(parameters.Length > 4 && !string.IsNullOrEmpty(parameters[4]))
            {
                bool.TryParse(parameters[4], out includePublicProfile);
            }

            return RegistryActions.DeleteValueForAllUsers(architecture, path, valueName, includeDefaultProfile, includePublicProfile).ToIntString();
        }
    }
}
