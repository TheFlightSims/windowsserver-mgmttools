using System.Web.Http;
using WinMan.PerformanceCounters.Models;

namespace WinMan.api
{
    public class PerformanceCounterController : ApiController
    {
        [HttpGet]
        public PerformanceCounterModel GetPerformanceCounter()
        {
            return PerformanceCounters.Util.GetPerformanceCounter(Factory.ProcessorCounter, Factory.MemoryCounter);
        }
    }
}
