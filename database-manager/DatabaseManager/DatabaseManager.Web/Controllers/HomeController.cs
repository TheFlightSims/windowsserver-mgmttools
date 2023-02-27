using DatabaseConverter.Model;
using DatabaseInterpreter.Model;
using DatabaseManager.Core;
using DatabaseManager.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SqlAnalyser.Model;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DatabaseManager.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Translate";
            return View();
        }

        public async Task<IActionResult> Translate()
        {
            string source = this.Request.Form["source"];
            var sourceDbType = (DatabaseType)Enum.Parse(typeof(DatabaseType), this.Request.Form["sourceDatabaseType"]);
            var targetDbType = (DatabaseType)Enum.Parse(typeof(DatabaseType), this.Request.Form["targetDatabaseType"]);

            try
            {
                TranslateManager translateManager = new TranslateManager();

                TranslateResult result = await Task.Run(() => translateManager.Translate(sourceDbType, targetDbType, source));

                string resultData = result.Data?.ToString();

                dynamic jsonResult = new { HasError = result.HasError, Data = resultData, Message = (result.Error as SqlSyntaxError)?.ToString() };

                return new JsonResult(jsonResult);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
