using System.IO;

namespace DeploymentToolkit.ToolkitEnvironment.Exceptions
{
    public class DeploymentToolkitInstallPathNotFoundException : FileNotFoundException
    {
        public DeploymentToolkitInstallPathNotFoundException(string message, string fileName) : base(message, fileName)
        {
        }
    }
}
