using DeploymentToolkit.Modals.Actions;

namespace DeploymentToolkit.Actions.Modals.Actions
{
    public class FileDeleteForAllUsers : IExecutableAction
    {
        public string Target { get; set; }

        public bool IncludeDefaultProfile { get; set; }
        public bool IncludePublicProfile { get; set; }

        public bool Execute()
        {
            return FileActions.DeleteFileForAllUsers(Target, IncludeDefaultProfile, IncludePublicProfile);
        }
    }
}
