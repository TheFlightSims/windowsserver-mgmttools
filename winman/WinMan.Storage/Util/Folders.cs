using System.Collections.Generic;
using WinMan.Storage.Models;

namespace WinMan.Storage.Util
{
    public class Folders
    {
        public static FolderModel GetFolder(string folderName)
        {
            var rtnVal = new FolderModel()
            {
                ErrorMessage = ""
            };

            try
            {
                var dir = new System.IO.DirectoryInfo(folderName);
                rtnVal.CreationTime = dir.CreationTime;
                rtnVal.FullName = dir.FullName;
                rtnVal.IsArchived = dir.Attributes.HasFlag(System.IO.FileAttributes.Archive);
                rtnVal.IsCompressed = dir.Attributes.HasFlag(System.IO.FileAttributes.Compressed);
                rtnVal.IsHidden = dir.Attributes.HasFlag(System.IO.FileAttributes.Hidden);
                rtnVal.LastAccessTime = dir.LastAccessTime;
                rtnVal.LastWriteTime = dir.LastWriteTime;
                rtnVal.Name = dir.Name;
                rtnVal.Folders = new List<FolderModel>();
                rtnVal.Files = new List<FileModel>();

                foreach (var folder in dir.GetDirectories())
                {
                    var subFolder = new FolderModel();

                    subFolder.CreationTime = folder.CreationTime;
                    subFolder.FullName = folder.FullName;
                    subFolder.IsArchived = folder.Attributes.HasFlag(System.IO.FileAttributes.Archive);
                    subFolder.IsCompressed = folder.Attributes.HasFlag(System.IO.FileAttributes.Compressed);
                    subFolder.IsHidden = folder.Attributes.HasFlag(System.IO.FileAttributes.Hidden);
                    subFolder.LastAccessTime = folder.LastAccessTime;
                    subFolder.LastWriteTime = folder.LastWriteTime;
                    subFolder.Name = folder.Name;
                    rtnVal.Folders.Add(subFolder);
                }

                foreach (var file in dir.GetFiles())
                {
                    var subFile = new FileModel();
                    subFile.CreationTime = file.CreationTime;
                    subFile.Extension = file.Extension;
                    subFile.FullName = file.FullName;
                    subFile.IsArchived = file.Attributes.HasFlag(System.IO.FileAttributes.Archive);
                    subFile.IsCompressed = file.Attributes.HasFlag(System.IO.FileAttributes.Compressed);
                    subFile.IsHidden = file.Attributes.HasFlag(System.IO.FileAttributes.Hidden);
                    subFile.LastAccessTime = file.LastAccessTime;
                    subFile.LastWriteTime = file.LastWriteTime;
                    subFile.Length = file.Length;
                    subFile.Name = file.Name;
                    rtnVal.Files.Add(subFile);
                }

            }
            catch (System.UnauthorizedAccessException)
            {
                rtnVal.ErrorMessage = "UnauthorizedAccessException";
            }
            catch (System.Exception)
            {
                rtnVal.ErrorMessage = "Generic Error, check the log file.";
            }
            return rtnVal;
        }
    }
}
