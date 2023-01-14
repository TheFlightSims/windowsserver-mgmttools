using System.Web.Http;
using WinMan.OS.Models;

namespace WinMan.api
{
    public class OsController : ApiController
    {
        [HttpGet]
        public OsModel Info()
        {
            return OS.Utils.Util.GetInfo();
        }
    }
}
