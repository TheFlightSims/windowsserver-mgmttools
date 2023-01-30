using DeploymentToolkit.Modals.Actions;

namespace DeploymentToolkit.Actions.Modals.Actions
{
    public class DirectoryDelete : IExecutableAction
    {
        public string Target { get; set; }
        public bool Recursive { get; set; }

        public bool Execute()
        {
            return DirectoryActions.DeleteDirectory(Target, Recursive);
        }
    }
}
