using DeploymentToolkit.Modals.Actions;

namespace DeploymentToolkit.Actions.Modals.Actions
{
    public class DirectoryCopyForAllUsers : IExecutableAction
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public bool Overwrite { get; set; }
        public bool Recursive { get; set; }

        public bool IncludeDefaultProfile { get; set; }
        public bool IncludePublicProfile { get; set; }

        public bool Execute()
        {
            return DirectoryActions.CopyDirectoryForAllUsers(Source, Target, Overwrite, Recursive, IncludeDefaultProfile, IncludePublicProfile);
        }
    }
}
