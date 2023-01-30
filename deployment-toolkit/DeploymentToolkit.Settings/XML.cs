using System.IO;
using System.Xml.Serialization;

namespace DeploymentToolkit.Settings
{
    public static class XML
    {
        public static T ReadXml<T>(string path)
        {
            path = Path.GetFullPath(path);
            var fileName = Path.GetFileName(path);

            var xmlReader = new XmlSerializer(typeof(T));
            var text = File.ReadAllText(path);
            using (var stringReader = new StringReader(text))
            {
                return (T)xmlReader.Deserialize(stringReader);
            }
        }
    }
}
