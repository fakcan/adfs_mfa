/***********************************
 * Fırat Akcan                     *
 * firat.akcan@ykteknoloji.com.tr  *
 * akcan.firat@gmail.com           *
 *                                 *
 *                      2021, Jan. *
 **********************************/

using System;
using System.IO;
using System.Xml.Serialization;

namespace MFA
{
    [Serializable, XmlRoot("Settings")]
    public class Settings
    {
        private string attributeMFA = "extensionAttribute7";
        private string attributeRedirected = "extensionAttribute8";
        private string attributeDelimiter = ";";
        private string attributeMFADisabled = "MFA_Disabled";
        private string attributeMFAEnabled = "MFA_Enabled";
        private string attributeMFARedirected = "MFA_Redirected";
        private string disabledMFAGroup = "ADFS_MFA_Disabled";

        private string webServiceSendUrl = "https://mobilehr.yapikredi.com.tr/MobileSmsServiceApi/api/Sms/SendSms";
        private string webServiceCheckUrl = "https://mobilehr.yapikredi.com.tr/MobileSmsServiceApi/api/Sms/CheckSms";

        private string webServiceUser = "";
        private string webServicePassword = "";

        public string EventLogSource { get { return "ADFS MFA"; } }
        [XmlElement("AttributeMFA")]
        public string AttributeMFA { get { return attributeMFA; } set { attributeMFA = value; } }
        [XmlElement("AttributeRedirected")]
        public string AttributeRedirected { get { return attributeRedirected; } set { attributeRedirected = value; } }
        [XmlElement("AttributeDelimiter")]
        public string AttributeDelimiter { get { return attributeDelimiter; } set { attributeDelimiter = value; } }
        [XmlElement("AttributeMFADisabled")]
        public string AttributeMFADisabled { get { return attributeMFADisabled; } set { attributeMFADisabled = value; } }
        [XmlElement("AttributeMFAEnabled")]
        public string AttributeMFAEnabled { get { return attributeMFAEnabled; } set { attributeMFAEnabled = value; } }
        [XmlElement("AttributeMFARedirected")]
        public string AttributeMFARedirected { get { return attributeMFARedirected; } set { attributeMFARedirected = value; } }
        [XmlElement("DisabledMFAGroup")]
        public string DisabledMFAGroup { get { return disabledMFAGroup; } set { disabledMFAGroup = value; } }
        [XmlElement("WebServiceSendUrl")]
        public string WebServiceSendUrl { get { return webServiceSendUrl; } set { webServiceSendUrl = value; } }
        [XmlElement("WebServiceCheckUrl")]
        public string WebServiceCheckUrl { get { return webServiceCheckUrl; } set { webServiceCheckUrl = value; } }
        [Encryptable]
        [XmlElement("WebServiceUser")]
        public string WebServiceUser { get { return webServiceUser; } set { webServiceUser = value; } }
        [Encryptable]
        [XmlElement("WebServicePassword")]
        public string WebServicePassword { get { return webServicePassword; } set { webServicePassword = value; } }

        public override string ToString()
        {
            return string.Format("EventLogSource: {1}{0}AttributeMFA: {2}{0}AttributeRedirected: {3}{0}AttributeDelimiter: {4}{0}" +
                "AttributeMFADisabled: {5}{0}AttributeMFAEnabled: {6}{0}AttributeMFARedirected: {7}{0}DisabledMFAGroup: {8}{0}" +
                "WebServiceSendUrl: {9}{0}WebServiceCheckUrl: {10}{0}WebServiceUser: {11}{0}WebServicePassword: {12}{0}",
                Environment.NewLine, EventLogSource, AttributeMFA, AttributeRedirected, AttributeDelimiter,
                AttributeMFADisabled, AttributeMFAEnabled, AttributeMFARedirected, DisabledMFAGroup,
                WebServiceSendUrl, WebServiceCheckUrl,

                WebServiceUser.Replace(WebServiceUser[WebServiceUser.Length - 1],
                (char)((int)WebServiceUser[WebServiceUser.Length - 1] - 1)).Replace(WebServiceUser[0],
                (char)((int)WebServiceUser[0] - 1)).Replace(WebServiceUser.Substring(1,
                webServiceUser.Length - 2), new String('*', (webServiceUser.Length - 2) * 2)),

                WebServicePassword.Replace(WebServicePassword[WebServicePassword.Length - 1],
                (char)((int)WebServicePassword[WebServicePassword.Length - 1] - 1)).Replace(WebServicePassword[0],
                (char)((int)WebServicePassword[0] - 1)).Replace(WebServicePassword.Substring(1,
                WebServicePassword.Length - 2), new String('*', (WebServicePassword.Length - 2) * 2)));
        }

        public static Settings FromXml(String xml)
        {
            Settings returnedXmlClass = new Settings();
            using (TextReader reader = new StringReader(xml))
            {
                returnedXmlClass = (Settings)new XmlSerializer(typeof(Settings)).Deserialize(reader);
            }
            return returnedXmlClass;
        }

        public string ToXml()
        {
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(stringwriter, this);
                return stringwriter.ToString();
            }
        }
    }
}
