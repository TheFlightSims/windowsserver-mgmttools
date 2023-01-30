using DeploymentToolkit.Modals.Actions;

namespace DeploymentToolkit.Actions.Modals.Actions
{
    public class DirectoryDeleteForAllUsers : IExecutableAction
    {
        public string Target { get; set; }
        public bool Recursive { get; set; }

        public bool IncludeDefaultProfile { get; set; }
        public bool IncludePublicProfile { get; set; }

        public bool Execute()
        {
            return DirectoryActions.DeleteDirectoryForAllUsers(Target, Recursive, IncludeDefaultProfile, IncludePublicProfile);
        }
    }
}
