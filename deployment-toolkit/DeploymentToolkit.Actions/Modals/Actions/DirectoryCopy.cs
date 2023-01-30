using DeploymentToolkit.Modals.Actions;

namespace DeploymentToolkit.Actions.Modals.Actions
{
    public class DirectoryCopy : IExecutableAction
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public bool Overwrite { get; set; }
        public bool Recursive { get; set; }

        public bool Execute()
        {
            return DirectoryActions.CopyDirectory(Source, Target, Overwrite, Recursive);
        }
    }
}
