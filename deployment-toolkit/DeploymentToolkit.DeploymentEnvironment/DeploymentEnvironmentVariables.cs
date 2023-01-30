using System.IO;
using System.Reflection;

namespace DeploymentToolkit.DeploymentEnvironment
{
    public static class DeploymentEnvironmentVariables
    {
        private static string _rootDirectory;
        public static string RootDirectory
        {
            get
            {
                if(string.IsNullOrEmpty(_rootDirectory))
                {
                    _rootDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                }

                return _rootDirectory;
            }
            set
            {
                _rootDirectory = value;
            }
        }

        private static string _filesDirectory;
        public static string FilesDirectory
        {
            get
            {
                if(string.IsNullOrEmpty(_filesDirectory))
                {
                    _filesDirectory = Path.Combine(RootDirectory, "Files");
                }

                return _filesDirectory;
            }
        }

        private static string _extensionsEnvironment;
        public static string ExtensionsEnvironment
        {
            get
            {
                if(string.IsNullOrEmpty(_extensionsEnvironment))
                {
                    _extensionsEnvironment = Path.Combine(RootDirectory, "Extensions");
                }

                return _extensionsEnvironment;
            }
        }
    }
}
