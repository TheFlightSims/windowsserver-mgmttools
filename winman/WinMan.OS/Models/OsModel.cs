namespace WinMan.OS.Models
{
    public class OsModel
    {
        public string MachineName { get; set; }
        public string OsName { get; set; }
        public string Architecture { get; set; }
        public string Processor { get; set; }
        public ulong Memory { get; set; }
        public string SystemType { get; set; }
        public string InstallDate { get; set; }
        public string LastBootupTime { get; set; }
    }
}
