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

        public static string Action1 => Action;
        public static Uri Uri1 => Uri;
        public static XNamespace SoapSchemaNs1 => SoapSchemaNs;
        public static XNamespace XmlSchemaInstanceNs1 => XmlSchemaInstanceNs;
        public static XNamespace XmlSchemaNs1 => XmlSchemaNs;
        public static XNamespace BatchActivationServiceNs1 => BatchActivationServiceNs;
        public static XNamespace BatchActivationRequestNs1 => BatchActivationRequestNs;
        public static XNamespace BatchActivationResponseNs1 => BatchActivationResponseNs;
        public static byte[] MacKey1 => MacKey;

        private void GetCID_Click(object sender, EventArgs e)
        {
            while (PhoneIID.Text == null || EPID.Text == null)
            {
                MessageBox.Show("No EPID nor Phone IID provided. Check again", "Enter something else", MessageBoxButtons.OK);
                CID.Text = "Invaild EPID nor IID. Error provided.";
                break;
            }

            int requestType = 1;
            string extendedProductId = EPID.Text.Replace("_", "").Replace(" ", "");
            string installationId = PhoneIID.Text.Replace(".", "").Replace("-", "").Replace("_", "").Replace(" ", "");

            XDocument soapRequest = CreateSoapRequest(requestType, installationId, extendedProductId);
            HttpWebRequest webRequest = CreateWebRequest(soapRequest);
            XDocument soapResponse = new XDocument();

            try
            {
                IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);
                asyncResult.AsyncWaitHandle.WaitOne();
                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResponse = XDocument.Parse(streamReader.ReadToEnd());
                }

                CID.Text = ParseSoapResponse(soapResponse);

            }
            catch (Exception ex) 
            {
                CID.Text = "An unexpected error has throw: " + ex.Message;
            }
        }



        private static XDocument CreateSoapRequest(int requestType, string installationId, string extendedProductId)
        {
            // Create an activation request.           
            XElement activationRequest = new XElement(BatchActivationRequestNs1 + "ActivationRequest",
                new XElement(BatchActivationRequestNs1 + "VersionNumber", "2.0"),
                new XElement(BatchActivationRequestNs1 + "RequestType", requestType),
                new XElement(BatchActivationRequestNs1 + "Requests",
                    new XElement(BatchActivationRequestNs1 + "Request",
                        new XElement(BatchActivationRequestNs1 + "PID", extendedProductId),
                        requestType == 1 ? new XElement(BatchActivationRequestNs1 + "IID", installationId) : null)
                )
            );

            // Get the unicode byte array of activationRequest and convert it to Base64.
            byte[] bytes = Encoding.Unicode.GetBytes(activationRequest.ToString());
            string requestXml = Convert.ToBase64String(bytes);

            XDocument soapRequest = new XDocument();

            using (HMACSHA256 hMACSHA = new HMACSHA256(MacKey1))
            {
                // Convert the HMAC hashed data to Base64.
                string digest = Convert.ToBase64String(hMACSHA.ComputeHash(bytes));

                soapRequest = new XDocument(
                new XDeclaration("1.0", "UTF-8", "no"),
                new XElement(SoapSchemaNs1 + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "soap", SoapSchemaNs1),
                    new XAttribute(XNamespace.Xmlns + "xsi", XmlSchemaInstanceNs1),
                    new XAttribute(XNamespace.Xmlns + "xsd", XmlSchemaNs1),
                    new XElement(SoapSchemaNs1 + "Body",
                        new XElement(BatchActivationServiceNs1 + "BatchActivate",
                            new XElement(BatchActivationServiceNs1 + "request",
                                new XElement(BatchActivationServiceNs1 + "Digest", digest),
                                new XElement(BatchActivationServiceNs1 + "RequestXml", requestXml)
                            )
                        )
                    )
                ));

            }

            return soapRequest;
        }

        private static HttpWebRequest CreateWebRequest(XDocument soapRequest)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Uri1);
            webRequest.Accept = "text/xml";
            webRequest.ContentType = "text/xml; charset=\"utf-8\"";
            webRequest.Headers.Add("SOAPAction", Action1);
            webRequest.Host = "activation.sls.microsoft.com";
            webRequest.Method = "POST";

            try
            {
                // Insert SOAP envelope
                using (Stream stream = webRequest.GetRequestStream())
                {
                    soapRequest.Save(stream);
                }

                return webRequest;

            }
            catch
            {
                throw;
            }
        }

        private static string ParseSoapResponse(XDocument soapResponse)
        {
            if (soapResponse == null)
            {
                throw new ArgumentNullException(nameof(soapResponse), "The remote server returned an unexpected response.");
            }

            if (!soapResponse.Descendants(BatchActivationServiceNs + "ResponseXml").Any())
            {
                throw new Exception("The remote server returned an unexpected response");
            }

            try
            {
                XDocument responseXml = XDocument.Parse(soapResponse.Descendants(BatchActivationServiceNs + "ResponseXml").First().Value);

                if (responseXml.Descendants(BatchActivationResponseNs + "ErrorCode").Any())
                {
                    string errorCode = responseXml.Descendants(BatchActivationResponseNs + "ErrorCode").First().Value;

                    switch (errorCode)
                    {
                        case "0x7F":
                            throw new Exception("The Multiple Activation Key has exceeded its limit");

                        case "0x67":
                            throw new Exception("The product key has been blocked");

                        case "0x68":
                            throw new Exception("Invalid product key");

                        case "0x86":
                            throw new Exception("Invalid key type");

                        case "0x90":
                            throw new Exception("Please check the Installation ID and try again");

                        default:
                            throw new Exception(string.Format("The remote server reported an error ({0})", errorCode));
                    }

                }
                else if (responseXml.Descendants(BatchActivationResponseNs + "ResponseType").Any())
                {
                    string responseType = responseXml.Descendants(BatchActivationResponseNs + "ResponseType").First().Value;

                    switch (responseType)
                    {
                        case "1":
                            string confirmationId = responseXml.Descendants(BatchActivationResponseNs + "CID").First().Value;
                            return confirmationId;

                        case "2":
                            string activationsRemaining = responseXml.Descendants(BatchActivationResponseNs + "ActivationRemaining").First().Value;
                            return activationsRemaining;

                        default:
                            throw new Exception("The remote server returned an unrecognized response");
                    }

                }
                else
                {
                    throw new Exception("The remote server returned an unrecognized response");
                }

            }
            catch
            {
                throw;
            }
        }
    }
}