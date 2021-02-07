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
    [Serializable, XmlRoot("Config")]
    public class Config
    {
        [XmlElement("Path")]
        public string Path { get; set; }

        public static Config FromXml(String xml)
        {
            Config returnedXmlClass = new Config();
            using (TextReader reader = new StringReader(xml))
            {
                returnedXmlClass = (Config)new XmlSerializer(typeof(Config)).Deserialize(reader);
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
