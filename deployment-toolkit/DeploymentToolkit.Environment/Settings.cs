using DeploymentToolkit.Modals.Settings;
using DeploymentToolkit.Modals.Settings.Tray;
using System;
using System.IO;
using System.Xml.Serialization;

namespace DeploymentToolkit.ToolkitEnvironment
{
    public static class Settings
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private static void SaveXml(string path, object input)
        {
            path = Path.GetFullPath(path);
            _logger.Trace($"Saving settings to {path}");

            if(File.Exists(path))
            {
                File.Delete(path);
            }

            var xmlWriter = new XmlSerializer(input.GetType());

            using(var streamWriter = new StreamWriter(path))
            {
                xmlWriter.Serialize(streamWriter, input);
            }
        }

        private static T ReadXml<T>(string path) where T : new()
        {
            try
            {
                path = Path.GetFullPath(path);
                var xmlReader = new XmlSerializer(typeof(T));
                var text = File.ReadAllText(path);
                using(var stringReader = new StringReader(text))
                {
                    return (T)xmlReader.Deserialize(stringReader);
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex, $"Failed to deserialize {path}");
                return default(T);
            }
        }

        private static T GetSettings<T>(string fileName) where T : new()
        {
            var path = Path.Combine(EnvironmentVariables.DeploymentToolkitSettingsPath, $"{fileName}.config");
            _logger.Trace($"Reading settings from {path}");
            try
            {
                if(!File.Exists(path))
                {
                    var instance = (T)Activator.CreateInstance(typeof(T));
                    SaveXml(path, instance);
                    return instance;
                }

                return ReadXml<T>(path);
            }
            catch(Exception ex)
            {
                _logger.Error(ex, $"Failed to serialize {path}");
                return default(T);
            }
        }

        public static TrayAppSettings GetTrayAppSettings()
        {
            return GetSettings<TrayAppSettings>("TrayApp") ?? new TrayAppSettings();
        }

        public static BlockerSettings GetBlockerSettings()
        {
            return GetSettings<BlockerSettings>("Blocker") ?? new BlockerSettings();
        }

        public static DeploymentSettings GetDeploymentSettings()
        {
            return GetSettings<DeploymentSettings>("Deployment") ?? new DeploymentSettings();
        }
    }
}
