namespace DeploymentToolkit.Registry
{
    public enum RegAccess
    {
        KEY_ALL_ACCESS = 0xF003F,
        KEY_CREATE_LINK = 0x0020,
        KEY_CREATE_SUB_KEY = 0x0004,
        KEY_ENUMERATE_SUB_KEYS = 0x0008,
        KEY_EXECUTE = 0x20019,
        KEY_NOTIFY = 0x0010,
        KEY_QUERY_VALUE = 0x0001,
        KEY_READ = 0x20019,
        KEY_SET_VALUE = 0x0002,
        KEY_WOW64_32KEY = 0x0200,
        KEY_WOW64_64KEY = 0x0100,
        KEY_WRITE = 0x20006
    }
}
