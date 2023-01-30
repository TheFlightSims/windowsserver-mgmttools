namespace DeploymentToolkit.Registry
{
    public class Win32Registry : WinRegistryBase
    {
        protected override RegAccess RegAccess => RegAccess.KEY_WOW64_32KEY;
    }
}
