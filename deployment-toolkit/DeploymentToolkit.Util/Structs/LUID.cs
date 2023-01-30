using System.Runtime.InteropServices;

namespace DeploymentToolkit.Util.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct LUID
    {
        internal int LowPart;
        internal int HighPart;
    }
}
