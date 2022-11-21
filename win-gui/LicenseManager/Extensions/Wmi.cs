using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace HGM.Hotbird64.LicenseManager.Extensions
{
    public static class Wmi
    {
        public static dynamic TryGetObject(this ManagementBaseObject mo, string propertyName, Func<dynamic, dynamic> converter = null)
        {
            try
            {
                dynamic o = mo[propertyName];
                return converter == null ? o : converter(o);
            }
            catch
            {
                return null;
            }
        }
    }
}
