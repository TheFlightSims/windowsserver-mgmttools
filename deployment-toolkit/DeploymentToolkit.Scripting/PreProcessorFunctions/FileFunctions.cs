using DeploymentToolkit.Actions;
using DeploymentToolkit.Scripting.Extensions;

namespace DeploymentToolkit.Scripting.PreProcessorFunctions
{
    public static class FileFunctions
    {
        public static string Exists(string[] parameters)
        {
            if(parameters.Length == 0)
            {
                return false.ToIntString();
            }

            return FileActions.FileExists(parameters[0]).ToIntString();
        }

        public static string MoveFile(string[] parameters)
        {
            if(parameters.Length <= 1)
            {
                return false.ToIntString();
            }

            var source = parameters[0];
            if(string.IsNullOrEmpty(source))
            {
                return false.ToIntString();
            }

            var target = parameters[1];
            if(string.IsNullOrEmpty(target))
            {
                return false.ToIntString();
            }

            var overwrite = false;

            if(parameters.Length > 2 && !string.IsNullOrEmpty(parameters[2]))
            {
                bool.TryParse(parameters[2], out overwrite);
            }

            return FileActions.MoveFile(source, target, overwrite).ToIntString();
        }

        public static string CopyFile(string[] parameters)
        {
            if(parameters.Length <= 1)
            {
                return false.ToIntString();
            }

            var source = parameters[0];
            if(string.IsNullOrEmpty(source))
            {
                return false.ToIntString();
            }

            var target = parameters[1];
            if(string.IsNullOrEmpty(target))
            {
                return false.ToIntString();
            }

            var overwrite = false;

            if(parameters.Length > 2 && !string.IsNullOrEmpty(parameters[2]))
            {
                bool.TryParse(parameters[2], out overwrite);
            }

            return FileActions.CopyFile(source, target, overwrite).ToIntString();
        }

        public static string CopyFileForAllUsers(string[] parameters)
        {
            if(parameters.Length <= 1)
            {
                return false.ToIntString();
            }

            var source = parameters[0];
            if(string.IsNullOrEmpty(source))
            {
                return false.ToIntString();
            }

            var target = parameters[1];
            if(string.IsNullOrEmpty(target))
            {
                return false.ToIntString();
            }

            var overwrite = false;
            var includeDefaultProfile = false;
            var includePublicProfile = false;

            if(parameters.Length > 2 && !string.IsNullOrEmpty(parameters[2]))
            {
                bool.TryParse(parameters[2], out overwrite);
            }
            if(parameters.Length > 3 && !string.IsNullOrEmpty(parameters[3]))
            {
                bool.TryParse(parameters[3], out includeDefaultProfile);
            }
            if(parameters.Length > 4 && !string.IsNullOrEmpty(parameters[4]))
            {
                bool.TryParse(parameters[4], out includePublicProfile);
            }

            return FileActions.CopyFileForAllUsers(source, target, overwrite, includeDefaultProfile, includePublicProfile).ToIntString();
        }

        public static string DeleteFile(string[] parameters)
        {
            if(parameters.Length < 1)
            {
                return false.ToIntString();
            }

            var target = parameters[0];
            if(string.IsNullOrEmpty(target))
            {
                return false.ToIntString();
            }

            return FileActions.DeleteFile(target).ToIntString();
        }

        public static string DeleteFileForAllUsers(string[] parameters)
        {
            if(parameters.Length < 1)
            {
                return false.ToIntString();
            }

            var target = parameters[0];
            if(string.IsNullOrEmpty(target))
            {
                return false.ToIntString();
            }

            var includeDefaultProfile = false;
            var includePublicProfile = false;

            if(parameters.Length > 1 && !string.IsNullOrEmpty(parameters[1]))
            {
                bool.TryParse(parameters[1], out includeDefaultProfile);
            }
            if(parameters.Length > 2 && !string.IsNullOrEmpty(parameters[2]))
            {
                bool.TryParse(parameters[2], out includePublicProfile);
            }

            return FileActions.DeleteFileForAllUsers(target, includeDefaultProfile, includePublicProfile).ToIntString();
        }
    }
}
