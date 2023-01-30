using DeploymentToolkit.Modals.Actions;

namespace DeploymentToolkit.Actions.Modals.Actions
{
    public class FileDelete : IExecutableAction
    {
        public string Target { get; set; }

        public bool Execute()
        {
            return FileActions.DeleteFile(Target);
        }
    }
}
