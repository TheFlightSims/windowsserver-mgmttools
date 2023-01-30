using System;

namespace DeploymentToolkit.Scripting.Extensions
{
    public static class BoolExtension
    {
        public static string ToIntString(this bool boolean)
        {
            return Convert.ToInt32(boolean).ToString();
        }
    }
}
