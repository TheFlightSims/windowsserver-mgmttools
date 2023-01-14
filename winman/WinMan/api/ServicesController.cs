using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using WinMan.Models;
using WinMan.Services.Models;

namespace WinMan.api
{
    public class ServicesController : ApiController
    {
        public async Task<IEnumerable<ServiceModel>> Get()
        {
            return await Services.Util.GetServicesAsync();
        }

        [HttpGet]
        public async Task<ServiceModel> Details(string id)
        {
            return await Services.Util.GetServiceAsync(id);
        }

        [HttpGet]
        public async Task<ExtendedServiceModel> Extended(string id)
        {
            return await Services.Util.GetExtendedServiceAsync(id);
        }

        [HttpPost]
        public string Start([FromBody] ServiceControlModel service)
        {
            return Services.Util.ControlService(service.ServiceName, ControlType.StartService).ToString();
        }

        [HttpPost]
        public string Stop([FromBody] ServiceControlModel service)
        {
            return Services.Util.ControlService(service.ServiceName, ControlType.StopService).ToString();
        }

        [HttpPost]
        public string Pause([FromBody] ServiceControlModel service)
        {
            return Services.Util.ControlService(service.ServiceName, ControlType.PauseService).ToString();
        }

        [HttpPost]
        public string Resume([FromBody] ServiceControlModel service)
        {
            return Services.Util.ControlService(service.ServiceName, ControlType.ResumeService).ToString();
        }
    }
}
