using Microsoft.Owin.Hosting;
using System;
using System.Configuration;
using System.Net.Http;

namespace WinMan
{
    class Program
    {
        static void Main(string[] args)
        {
            InitApp();
            string baseAddress = "http://*:8000/";
            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                // Create HttpCient and make a request to api/values 
                HttpClient client = new HttpClient();
                Console.ReadLine();
                Factory.MemoryCounter.Dispose();
                Factory.ProcessorCounter.Dispose();
            }
        }

        private static void InitApp()
        {
            Factory.PassCode = ConfigurationManager.AppSettings["passcode"];
        }
    }
}
