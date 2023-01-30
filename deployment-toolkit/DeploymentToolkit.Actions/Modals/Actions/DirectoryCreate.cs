using DeploymentToolkit.Modals.Actions;

namespace DeploymentToolkit.Actions.Modals.Actions
{
    public class DirectoryCreate : IExecutableAction
    {
        public string Target { get; set; }

        public bool Execute()
        {
            return DirectoryActions.CreateDirectory(Target);
        }
    }
}
