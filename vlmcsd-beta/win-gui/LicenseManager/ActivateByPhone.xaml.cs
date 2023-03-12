using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace HGM.Hotbird64.LicenseManager
{
    public partial class ActivateByPhone
    {
        public ActivateByPhone(MainWindow mainWindow)
        {
            InitializeComponent();
        }

        //--------------------------------------------------------------------------------------------------------------------------------
        
        private const string Action = "http://www.microsoft.com/BatchActivationService/BatchActivate";
        private static readonly Uri Uri = new Uri("https://activation.sls.microsoft.com/BatchActivation/BatchActivation.asmx");
        private static readonly XNamespace SoapSchemaNs = "http://schemas.xmlsoap.org/soap/envelope/";
        private static readonly XNamespace XmlSchemaInstanceNs = "http://www.w3.org/2001/XMLSchema-instance";
        private static readonly XNamespace XmlSchemaNs = "http://www.w3.org/2001/XMLSchema";
        private static readonly XNamespace BatchActivationServiceNs = "http://www.microsoft.com/BatchActivationService";
        private static readonly XNamespace BatchActivationRequestNs = "http://www.microsoft.com/DRM/SL/BatchActivationRequest/1.0";
        private static readonly XNamespace BatchActivationResponseNs = "http://www.microsoft.com/DRM/SL/BatchActivationResponse/1.0";
        private static readonly byte[] MacKey = new byte[64] {
            254,  49, 152, 117, 251,  72, 132, 134,
            156, 243, 241, 206, 153, 168, 144, 100,
            171,  87,  31, 202,  71,   4,  80,  88,
            48,   36, 226,  20,  98, 135, 121, 160,
            0,     0,   0,   0,   0,   0,   0,   0,
            0,     0,   0,   0,   0,   0,   0,   0,
            0,     0,   0,   0,   0,   0,   0,   0,
            0,     0,   0,   0,   0,   0,   0,   0
        };

        //--------------------------------------------------------------------------------------------------------------------------------
        private void GetCID_Click(object sender, EventArgs e)
        {
            int requestType = 1;
            string installationId, extendedProductId;

            while (PhoneIID.Text == null || EPID.Text == null)
            {
                MessageBox.Show("No EPID nor Phone IID provided. Check again", "Enter something else", MessageBoxButtons.OK);
                CID.Text = "Invaild EPID nor IID. Error provided.";
                break;
            }

            if (PhoneIID.Text.ToLower().Contains(' '))
            {
                installationId = PhoneIID.Text.Replace(" ", "");
            }
            else if (PhoneIID.Text.ToLower().Contains("-"))
            {
                installationId = PhoneIID.Text.Replace("-", "");
            }
            else if (PhoneIID.Text.ToLower().Contains("_"))
            {
                installationId = PhoneIID.Text.Replace("_", "");
            }
            else if (PhoneIID.Text.ToLower().Contains("."))
            {
                installationId = PhoneIID.Text.Replace(".", "");
            }
            else
            {
                installationId = PhoneIID.Text;
            }

            if (EPID.Text.ToLower().Contains(' '))
            {
                extendedProductId = EPID.Text.Replace(" ", "");
            }
            else if (EPID.Text.ToLower().Contains('-'))
            {
                extendedProductId = EPID.Text.Replace('-', ' ');
            }
            else if (EPID.Text.ToLower().Contains('_'))
            {
                extendedProductId = EPID.Text.Replace('_', ' ');
            }
            else if (EPID.Text.ToLower().Contains("."))
            {
                extendedProductId = EPID.Text.Replace(".", "");
            }
            else
            {
                extendedProductId = EPID.Text;
            }

            XDocument soapRequest = CreateSoapRequest(requestType, installationId, extendedProductId);
            HttpWebRequest webRequest = CreateWebRequest(soapRequest);
            XDocument soapResponse = new XDocument();

            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);
            asyncResult.AsyncWaitHandle.WaitOne();

            // Read data from the response stream.
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
            {
                soapResponse = XDocument.Parse(streamReader.ReadToEnd());
            }

             CID.Text = ParseSoapResponse(soapResponse);

        }

        //-------------------------------------------------------------------------------------------------------------------------
        //Create Soap Request
        private static XDocument CreateSoapRequest(int requestType, string installationId, string extendedProductId)
        {
            // Create an activation request.           
            XElement activationRequest = new XElement(BatchActivationRequestNs + "ActivationRequest",
                new XElement(BatchActivationRequestNs + "VersionNumber", "2.0"),
                new XElement(BatchActivationRequestNs + "RequestType", requestType),
                new XElement(BatchActivationRequestNs + "Requests",
                    new XElement(BatchActivationRequestNs + "Request",
                        new XElement(BatchActivationRequestNs + "PID", extendedProductId),
                        requestType == 1 ? new XElement(BatchActivationRequestNs + "IID", installationId) : null)
                )
            );

            byte[] bytes = Encoding.Unicode.GetBytes(activationRequest.ToString());
            string requestXml = Convert.ToBase64String(bytes);

            XDocument soapRequest = new XDocument();

            using (HMACSHA256 hMACSHA = new HMACSHA256(MacKey))
            {
                string digest = Convert.ToBase64String(hMACSHA.ComputeHash(bytes));
                soapRequest = new XDocument(
                new XDeclaration("1.0", "UTF-8", "no"),
                new XElement(SoapSchemaNs + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "soap", SoapSchemaNs),
                    new XAttribute(XNamespace.Xmlns + "xsi", XmlSchemaInstanceNs),
                    new XAttribute(XNamespace.Xmlns + "xsd", XmlSchemaNs),
                    new XElement(SoapSchemaNs + "Body",
                        new XElement(BatchActivationServiceNs + "BatchActivate",
                            new XElement(BatchActivationServiceNs + "request",
                                new XElement(BatchActivationServiceNs + "Digest", digest),
                                new XElement(BatchActivationServiceNs + "RequestXml", requestXml)
                            )
                        )
                    )
                ));

            }

            return soapRequest;
        }
        //-------------------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------------------
        //Web Request
        private static HttpWebRequest CreateWebRequest(XDocument soapRequest)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Uri);
            webRequest.Accept = "text/xml";
            webRequest.ContentType = "text/xml; charset=\"utf-8\"";
            webRequest.Headers.Add("SOAPAction", Action);
            webRequest.Host = "activation.sls.microsoft.com";
            webRequest.Method = "POST";

            // Insert SOAP envelope
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapRequest.Save(stream);
            }

            return webRequest;

        }
        //-------------------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------------------
        //ParseSoapRespone
        private static string ParseSoapResponse(XDocument soapResponse)
        {
            if (soapResponse == null)
            {
                System.Windows.Forms.MessageBox.Show($"The remote server returned an unexpected response", $"Server error", MessageBoxButtons.OK);
                return "No CID. An error has provided. ";
            } 

            if (!soapResponse.Descendants(BatchActivationServiceNs + "ResponseXml").Any())
            {
                System.Windows.Forms.MessageBox.Show($"The remote server returned an unexpected response", $"Error", MessageBoxButtons.OK);
                return "No CID. An error has provided. ";
            }

            XDocument responseXml = XDocument.Parse(soapResponse.Descendants(BatchActivationServiceNs + "ResponseXml").First().Value);

            if (responseXml.Descendants(BatchActivationResponseNs + "ErrorCode").Any())
            {
                string errorCode = responseXml.Descendants(BatchActivationResponseNs + "ErrorCode").First().Value;

                switch (errorCode)
                {
                    case "0x7F":
                        System.Windows.Forms.MessageBox.Show($"The Multiple Activation Key has exceeded its limit", $"Error", MessageBoxButtons.OK);
                        return "No CID. An error has provided. ";

                    case "0x67":
                        System.Windows.Forms.MessageBox.Show($"The product key has been blocked", $"Error", MessageBoxButtons.OK);
                        return "No CID. An error has provided. ";

                    case "0x68":
                        System.Windows.Forms.MessageBox.Show($"Invalid product key", $"Error", MessageBoxButtons.OK);
                        return "No CID. An error has provided. ";

                    case "0x86":
                        System.Windows.Forms.MessageBox.Show($"Invaild key type", $"Error", MessageBoxButtons.OK);
                        return "No CID. An error has provided. ";

                    case "0x90":
                        System.Windows.Forms.MessageBox.Show($"Please check the Installation ID and try again", $"Error", MessageBoxButtons.OK);
                        return "No CID. An error has provided. ";

                    default:
                        System.Windows.Forms.MessageBox.Show($"The remote server reported an error.", $"Error", MessageBoxButtons.OK);
                        return "No CID. An error has provided. ";
                }

            }
            else if (responseXml.Descendants(BatchActivationResponseNs + "ResponseType").Any())
            {
                return responseXml.Descendants(BatchActivationResponseNs + "CID").First().Value;

            }
            else
            {
                System.Windows.Forms.MessageBox.Show($"The remote server returned an unrecognized response", $"Error", MessageBoxButtons.OK);
                return "No CID. An error has provided. ";
            }

        }
    }
}