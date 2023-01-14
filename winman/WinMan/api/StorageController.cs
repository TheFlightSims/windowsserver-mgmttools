using System.Collections.Generic;
using System.Web.Http;
using WinMan.Models;
using WinMan.Storage.Models;

namespace WinMan.api
{
    public class StorageController : ApiController
    {

        [HttpGet]
        public List<DriveModel> Drives()
        {
            return Storage.Util.Drives.GetDrives();
        }

        [HttpGet]
        public FolderModel FolderContent(string id)
        {
            var folders = Storage.Util.Folders.GetFolder(id);
            return folders;
        }

        [HttpGet]
        public List<TreeViewModel> Folders(string id)
        {
            id = id.Replace("|", "\\");

            if (id == "#")
            {
                var rtnVal = new List<TreeViewModel>();
                rtnVal.Add(new TreeViewModel()
                {
                    children = true,
                    id = "##",
                    text = "This Server",
                    icon = "folder"
                });
                return rtnVal;
            }

            if (id == "##")
            {
                var drives = Storage.Util.Drives.GetDrives();
                var rtnVal = new List<TreeViewModel>();
                foreach (var drive in drives)
                {
                    var node = new TreeViewModel()
                    {
                        icon = "folder",
                        id = drive.Name,
                        text = drive.Name,
                        children = true
                    };
                    rtnVal.Add(node);
                }
                return rtnVal;
            }
            else
            {
                var folders = Storage.Util.Folders.GetFolder(id);
                var rtnVal = new List<TreeViewModel>();
                foreach (var folder in folders.Folders)
                {
                    var node = new TreeViewModel()
                    {
                        icon = "folder",
                        id = folder.FullName,
                        text = folder.Name,
                        children = true
                    };
                    rtnVal.Add(node);
                }
                return rtnVal;
            }

        }

        [HttpPost]
        public FolderModel Folders([FromBody] PostFolderModel folder)
        {
            folder.Folder = folder.Folder.Replace("|", "\\");
            return Storage.Util.Folders.GetFolder(folder.Folder);
        }

        // GET api/values/5 
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values 
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5 
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
        }

    }
}
