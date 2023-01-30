using DeploymentToolkit.Modals.Actions;

namespace DeploymentToolkit.Actions.Modals.Actions
{
    public class FileMove : IExecutableAction
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public bool Overwrite { get; set; }

        public bool Execute()
        {
            return FileActions.MoveFile(Source, Target, Overwrite);
        }
    }
}
