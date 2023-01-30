namespace DeploymentToolkit.Registry
{
    public class Win64Registry : WinRegistryBase
    {
        protected override RegAccess RegAccess => RegAccess.KEY_WOW64_64KEY;
    }
}
