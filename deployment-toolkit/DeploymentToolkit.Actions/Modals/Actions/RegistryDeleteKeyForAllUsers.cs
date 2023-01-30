using DeploymentToolkit.Modals.Actions;
using static DeploymentToolkit.Actions.RegistryActions;

namespace DeploymentToolkit.Actions.Modals.Actions
{
    public class RegistryDeleteKeyForAllUsers : IExecutableAction
    {
        public Architecture Architecture { get; set; }
        public string Path { get; set; }
        public string KeyName { get; set; }

        public bool IncludeDefaultProfile { get; set; }
        public bool IncludeSpecialProfiles { get; set; }

        public bool Execute()
        {
            return DeleteKeyForAllUsers(Architecture, Path, KeyName, IncludeDefaultProfile, IncludeSpecialProfiles);
        }
    }
}