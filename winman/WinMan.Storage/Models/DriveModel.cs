namespace WinMan.Storage.Models
{
    public class DriveModel
    {
        public long AvailableFreeSpace { get; set; }
        public string DriveFormat { get; set; }
        public bool IsReady { get; set; }
        public string Name { get; set; }
        public long TotalFreeSpace { get; set; }
        public long TotalSize { get; set; }
        public string VolumeLabel { get; set; }
    }
}
