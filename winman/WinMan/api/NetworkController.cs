using System.Collections.Generic;
using System.Web.Http;
using WinMan.Network.Models;

namespace WinMan.api
{
    public class NetworkController : ApiController
    {
        [HttpGet]
        public List<NICModel> GetNetworks()
        {
            return Network.Utils.Util.GetNetworkCards();
        }
    }
}
