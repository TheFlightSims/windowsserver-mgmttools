using DeploymentToolkit.Modals.Actions;
using static DeploymentToolkit.Actions.RegistryActions;

namespace DeploymentToolkit.Actions.Modals.Actions
{
    public class RegistryCreateKey : IExecutableAction
    {
        public Architecture Architecture { get; set; }
        public string Path { get; set; }
        public string KeyName { get; set; }

        public bool Execute()
        {
            return CreateKey(Architecture, Path, KeyName);
        }
    }
}
