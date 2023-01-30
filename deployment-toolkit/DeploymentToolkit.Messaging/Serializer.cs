using Newtonsoft.Json;
using NLog;
using System;

namespace DeploymentToolkit.Messaging
{
    internal static class Serializer
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static string SerializeMessage(IMessage message)
        {
            try
            {
                return JsonConvert.SerializeObject(message);
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Failed to serialize message");
                return null;
            }
        }

        public static T DeserializeMessage<T>(string data) where T : IMessage
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Failed to deserialize message");
                return default(T);
            }
        }

    }
}
