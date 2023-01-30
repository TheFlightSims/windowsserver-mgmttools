using System;
using Microsoft.ConfigurationManagement.ManagementProvider;
using Microsoft.ConfigurationManagement.ManagementProvider.WqlQueryEngine;

namespace CM_App_Creator
{
    public class SmsProvider
    {
        public WqlConnectionManager Connect(string serverName)
        {

            try
            {
                //' Connect to SMS Provider
                SmsNamedValuesDictionary namedValues = new SmsNamedValuesDictionary();
                WqlConnectionManager connection = new WqlConnectionManager(namedValues);
                connection.Connect(serverName);

                return connection;
            }
            catch (SmsException ex)
            {
                //AppCreator.Log(String.Format("Unhandled expection thrown by SMS Provider: {0}", ex.Message));
                SiteMaster.Log(String.Format("Unhandled expection thrown by SMS Provider: {0}", ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                //ConfigMgrWebService.WriteEventLog(String.Format("Unathorized access exception thrown: {0}", ex.Message), EventLogEntryType.Error);
                SiteMaster.Log(String.Format("Unathorized access exception thrown: {0}", ex.Message));

            }
            catch (Exception ex)
            {
                //ConfigMgrWebService.WriteEventLog(String.Format("Unhandled expection thrown: {0}", ex.Message), EventLogEntryType.Error);
                SiteMaster.Log(String.Format("Unhandled expection thrown: {0}", ex.Message));
            }

            return null;
        }
    }
}