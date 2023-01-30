using DeploymentToolkit.Modals.Actions;
using static DeploymentToolkit.Actions.RegistryActions;

namespace DeploymentToolkit.Actions.Modals.Actions
{
    public class RegistryDeleteValue : IExecutableAction
    {
        public Architecture Architecture { get; set; }
        public string Path { get; set; }
        public string ValueName { get; set; }

        public bool Execute()
        {
            return DeleteValue(Architecture, Path, ValueName);
        }
    }
}