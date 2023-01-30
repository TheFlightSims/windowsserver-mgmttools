namespace DeploymentToolkit.Util
{
    internal static class ProcessCreationFlags
    {
        internal const uint DEBUG_PROCESS = 0x00000001;
        internal const uint DEBUG_ONLY_THIS_PROCESS = 0x00000002;
        internal const uint CREATE_SUSPENDED = 0x00000004;
        internal const uint DETACHED_PROCESS = 0x00000008;
        internal const uint CREATE_NEW_CONSOLE = 0x00000010;
        internal const uint NORMAL_PRIORITY_CLASS = 0x00000020;
        internal const uint IDLE_PRIORITY_CLASS = 0x00000040;
        internal const uint HIGH_PRIORITY_CLASS = 0x00000080;
        internal const uint REALTIME_PRIORITY_CLASS = 0x00000100;
        internal const uint CREATE_NEW_PROCESS_GROUP = 0x00000200;
        internal const uint CREATE_UNICODE_ENVIRONMENT = 0x00000400;
        internal const uint CREATE_SEPARATE_WOW_VDM = 0x00000800;
        internal const uint CREATE_SHARED_WOW_VDM = 0x00001000;
        internal const uint CREATE_FORCEDOS = 0x00002000;
        internal const uint BELOW_NORMAL_PRIORITY_CLASS = 0x00004000;
        internal const uint ABOVE_NORMAL_PRIORITY_CLASS = 0x00008000;
        internal const uint INHERIT_PARENT_AFFINITY = 0x00010000;
        internal const uint INHERIT_CALLER_PRIORITY = 0x00020000;
        internal const uint CREATE_PROTECTED_PROCESS = 0x00040000;
        internal const uint EXTENDED_STARTUPINFO_PRESENT = 0x00080000;
        internal const uint PROCESS_MODE_BACKGROUND_BEGIN = 0x00100000;
        internal const uint PROCESS_MODE_BACKGROUND_END = 0x00200000;
        internal const uint CREATE_BREAKAWAY_FROM_JOB = 0x01000000;
        internal const uint CREATE_PRESERVE_CODE_AUTHZ_LEVEL = 0x02000000;
        internal const uint CREATE_DEFAULT_ERROR_MODE = 0x04000000;
        internal const uint CREATE_NO_WINDOW = 0x08000000;
        internal const uint PROFILE_USER = 0x10000000;
        internal const uint PROFILE_KERNEL = 0x20000000;
        internal const uint PROFILE_SERVER = 0x40000000;
        internal const uint CREATE_IGNORE_SYSTEM_DEFAULT = 0x80000000;
    }
}
