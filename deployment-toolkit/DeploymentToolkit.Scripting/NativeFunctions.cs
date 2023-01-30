using System.Runtime.InteropServices;

namespace DeploymentToolkit.Scripting
{
    internal static class NativeFunctions
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern short GetUserDefaultLangID();

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern short GetSystemDefaultLangID();
    }
}
