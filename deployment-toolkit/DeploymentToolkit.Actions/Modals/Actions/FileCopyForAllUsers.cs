using DeploymentToolkit.Modals.Actions;

namespace DeploymentToolkit.Actions.Modals.Actions
{
    public class FileCopyForAllUsers : IExecutableAction
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public bool Overwrite { get; set; }

        public bool IncludeDefaultProfile { get; set; }
        public bool IncludePublicProfile { get; set; }

        public bool Execute()
        {
            return FileActions.CopyFileForAllUsers(Source, Target, Overwrite, IncludeDefaultProfile, IncludePublicProfile);
        }
    }
}
