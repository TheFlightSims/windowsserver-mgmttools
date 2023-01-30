using System.Runtime.InteropServices;

namespace DeploymentToolkit.Util.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct _PRIVILEGE_SET
    {
        private int PrivilegeCount;
        private int Control;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)] // ANYSIZE_ARRAY = 1
        private LUID_AND_ATTRIBUTES[] Privileges;
    }
}
