using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CM_App_Creator
{
    public class CMApplication
    {
        public string ApplicationName { get; set; }
        public string ApplicationManufacturer { get; set; }
        public string ApplicationDescription { get; set; }
        public string ApplicationVersion { get; set; }
        public string[] SecurityScopes { get; set; }
        public string ApplicationCreatedBy { get; set; }
        public string CollectionName { get; set; }
        public string ObjectPath { get; set; }
    }
}