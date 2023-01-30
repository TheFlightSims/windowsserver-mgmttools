using System.Runtime.InteropServices;

namespace DeploymentToolkit.Util.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct TOKEN_PRIVILEGES
    {
        internal int PrivilegeCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        internal int[] Privileges;
    }
}
