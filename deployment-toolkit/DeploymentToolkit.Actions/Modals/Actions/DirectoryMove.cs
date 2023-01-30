using DeploymentToolkit.Modals.Actions;

namespace DeploymentToolkit.Actions.Modals.Actions
{
    public class DirectoryMove : IExecutableAction
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public bool Overwrite { get; set; }
        public bool Recursive { get; set; }

        public bool Execute()
        {
            return DirectoryActions.MoveDirectory(Source, Target, Overwrite, Recursive);
        }
    }
}
