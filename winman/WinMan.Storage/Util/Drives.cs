using System.Collections.Generic;
using System.Linq;
using WinMan.Storage.Models;

namespace WinMan.Storage.Util
{
    public class Drives
    {
        public static List<DriveModel> GetDrives()
        {
            var drives = System.IO.DriveInfo.GetDrives();
            var rtnVal = (from u in drives
                          where u.DriveType == System.IO.DriveType.Fixed
                          orderby u.Name
                          select new DriveModel()
                          {
                              AvailableFreeSpace = u.AvailableFreeSpace,
                              DriveFormat = u.DriveFormat.ToString(),
                              IsReady = u.IsReady,
                              Name = u.Name,
                              TotalFreeSpace = u.TotalFreeSpace,
                              TotalSize = u.TotalSize,
                              VolumeLabel = u.VolumeLabel
                          }).ToList();
            return rtnVal;
        }
    }
}
