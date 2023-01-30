using Microsoft.Win32;

namespace DeploymentToolkit.RegistryWrapper
{
    public class Win64Registry : WinRegistryBase
    {
        public override RegistryView View => RegistryView.Registry64;
    }
}
