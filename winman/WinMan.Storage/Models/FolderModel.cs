using System;
using System.Collections.Generic;

namespace WinMan.Storage.Models
{
    public class FolderModel
    {
        public bool IsHidden { get; set; }
        public bool IsArchived { get; set; }
        public bool IsCompressed { get; set; }
        public DateTime CreationTime { get; set; }
        public string DisplayPath { get; set; }
        public string FullName { get; set; }
        public DateTime LastAccessTime { get; set; }
        public DateTime LastWriteTime { get; set; }
        public string Name { get; set; }
        public FolderModel Parent { get; set; }
        public FolderModel Root { get; set; }
        public string ErrorMessage { get; set; }

        public List<FolderModel> Folders { get; set; }
        public List<FileModel> Files { get; set; }
    }
}
