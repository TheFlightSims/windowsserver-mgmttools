using DeploymentToolkit.Modals.Actions;
using static DeploymentToolkit.Actions.RegistryActions;

namespace DeploymentToolkit.Actions.Modals.Actions
{
    public class RegistryCreateKeyForAllUsers : IExecutableAction
    {
        public Architecture Architecture { get; set; }
        public string Path { get; set; }
        public string KeyName { get; set; }

        public bool IncludeDefaultProfile { get; set; }
        public bool IncludeSpecialProfiles { get; set; }

        public bool Execute()
        {
            return CreateKeyForAllUsers(Architecture, Path, KeyName, IncludeDefaultProfile, IncludeSpecialProfiles);
        }
    }
}
