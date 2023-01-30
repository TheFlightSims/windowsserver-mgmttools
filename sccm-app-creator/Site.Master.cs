using System;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.UI;

namespace CM_App_Creator
{
    // Author: Christoffer Bennerstedt - email bennerstedt77@gmail.com - official repo: https://github.com/CBennerstedt/CMAppCreator
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string ActivePage = Request.RawUrl;

            if (ActivePage.Contains("About"))
            {
                about.Attributes.Add("class", "active");
            }
            else if (ActivePage.Contains("Eventlog"))
            {
                eventlog.Attributes.Add("class", "active");
            }
            else
            {
                home.Attributes.Add("class", "active");
            }
            lblfooter.Text = string.Format("{0} Version {1}", HttpContext.Current.Application["NAME"], HttpContext.Current.Application["VERSION"]);
        }
        public static bool Log(string message, params object[] args)
        {
            //Console.WriteLine(message, args);
            // System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString()
            bool results;
            Debug.WriteLine(message, args);

            string logfile = "eventlog.log";
            DateTime time = DateTime.Now;

            try
            {
                using StreamWriter _testData = new StreamWriter(HttpContext.Current.Server.MapPath("~/" + logfile), true);
                string line = string.Format("{0};{1};{2}", time.ToString("yyyy-MM-dd HH:mm:ss"), HttpContext.Current.User.Identity.Name, message);
                _testData.WriteLine(line, args); // Write the file.
                results = true;
            }
            catch
            {
                results = false;
            }
            return results;
        }
    }
}