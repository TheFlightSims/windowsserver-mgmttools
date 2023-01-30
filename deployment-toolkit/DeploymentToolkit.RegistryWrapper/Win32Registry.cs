using Microsoft.Win32;

namespace DeploymentToolkit.RegistryWrapper
{
    public class Win32Registry : WinRegistryBase
    {
        public override RegistryView View => RegistryView.Registry32;
    }
}
