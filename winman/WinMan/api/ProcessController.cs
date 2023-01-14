using System.Collections.Generic;
using System.Web.Http;
using WinMan.Process.Models;

namespace WinMan.api
{
    public class ProcessController : ApiController
    {
        [HttpGet]
        public List<ProcessModel> GetProcesses()
        {
            return Process.Util.GetProcesses();
        }
    }
}
