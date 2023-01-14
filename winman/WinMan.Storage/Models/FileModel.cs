using System;

namespace WinMan.Storage.Models
{
    public class FileModel
    {
        public bool IsHidden { get; set; }
        public bool IsArchived { get; set; }
        public bool IsCompressed { get; set; }
        public DateTime CreationTime { get; set; }
        public string FullName { get; set; }
        public DateTime LastAccessTime { get; set; }
        public DateTime LastWriteTime { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public long Length { get; set; }
    }
}
