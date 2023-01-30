using DeploymentToolkit.Modals.Actions;

namespace DeploymentToolkit.Actions.Modals.Actions
{
    public class DirectoryCreateForAllUsers : IExecutableAction
    {
        public string Target { get; set; }

        public bool IncludeDefaultProfile { get; set; }
        public bool IncludePublicProfile { get; set; }

        public bool Execute()
        {
            return DirectoryActions.CreateDirectoryForAllUsers(Target, IncludeDefaultProfile, IncludePublicProfile);
        }
    }
}
