using System.Web.Http;
using WinMan.Registry.Models;

namespace WinMan.api
{
    public class RegistryController : ApiController
    {
        [HttpGet]
        public KeyModel GetRegistry(string id)
        {
            return Registry.Utils.Util.GetRegistry(id);
        }
    }
}
