using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace CM_App_Creator
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);

            var Assembly = typeof(Global).Assembly.GetName();
            var versionInfo = FileVersionInfo.GetVersionInfo(typeof(Global).Assembly.Location);
            Version version = Assembly.Version;
            var App = HttpContext.Current.Application;
            App.Lock();
            App.Add("VERSION", String.Format("{0}.{1}", version.Major, version.Minor));
            App.Add("BUILD", String.Format("{0}.{1}", version.Build, version.Revision));
            App.Add("COPYRIGHT", versionInfo.LegalCopyright);
            App.Add("NAME", versionInfo.ProductName);
            App.UnLock();
        }
    }
}