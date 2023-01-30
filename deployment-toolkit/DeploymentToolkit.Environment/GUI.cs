namespace DeploymentToolkit.ToolkitEnvironment
{
    public static partial class EnvironmentVariables
    {
        public static bool ForceDisableGUI = false;
        public static bool IsRunningInTaskSequence = false;
        public static bool IsGUIEnabled
        {
            get
            {
                if(ForceDisableGUI)
                {
                    return false;
                }

                if(IsRunningInTaskSequence)
                {
                    return false;
                }

                return true;
            }
        }
    }
}
