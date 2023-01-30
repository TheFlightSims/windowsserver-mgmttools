using DeploymentToolkit.Modals.Actions;
using Microsoft.Win32;
using static DeploymentToolkit.Actions.RegistryActions;

namespace DeploymentToolkit.Actions.Modals.Actions
{
    public class RegistrySetValueForAllUsers : IExecutableAction
    {
        public Architecture Architecture { get; set; }
        public string Path { get; set; }
        public string ValueName { get; set; }
        public string Value { get; set; }
        public RegistryValueKind Type { get; set; }

        public bool IncludeDefaultProfile { get; set; }
        public bool IncludeSpecialProfiles { get; set; }

        public bool Execute()
        {
            return SetValueForAllUsers(Architecture, Path, ValueName, Value, Type, IncludeDefaultProfile, IncludeSpecialProfiles);
        }
    }
}