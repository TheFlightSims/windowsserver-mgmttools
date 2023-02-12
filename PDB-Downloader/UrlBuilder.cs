using SymbolFetch.Helpers;
using System.IO;
using System.Windows;

namespace SymbolFetch
{
    class UrlBuilder
    {
        public string BuildUrl(string filename)
        {
            string downloadURL = string.Empty;
            string SymbolServerUrl;

            if (File.Exists(filename))
            {
                PeHeaderReader reader = new PeHeaderReader(filename);
                string pdbName;

                if (string.IsNullOrEmpty(reader.pdbName))
                {
                    downloadURL = string.Empty;
                }
                else
                {
                    if (reader.pdbName.Contains("\\"))
                    {
                        pdbName = (reader.pdbName.Split(new char[] { '\\' }))[reader.pdbName.Split(new char[] { '\\' }).Length - 1];
                    }
                    else
                        pdbName = reader.pdbName;

                    SymbolServerUrl = ConfigurationReader.SymbolServerUrl;

                    if (string.IsNullOrEmpty(SymbolServerUrl))
                        downloadURL = "http://msdl.microsoft.com/download/symbols/" + pdbName + "/" + reader.debugGUID.ToString("N").ToUpper() + reader.pdbage + "/" + pdbName;
                    else
                        downloadURL = SymbolServerUrl + "/" + pdbName + "/" + reader.debugGUID.ToString("N").ToUpper() + reader.pdbage + "/" + pdbName;
                }
            }
            return downloadURL;
        }
    }
}
