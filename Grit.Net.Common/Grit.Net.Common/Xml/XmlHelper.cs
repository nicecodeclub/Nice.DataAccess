using System.IO;
using System.Xml.Serialization;

namespace Grit.Net.Common.Helper
{
    public class XmlHelper
    {
        public static string Serialize<T>(T obj)
        {
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer xz = new XmlSerializer(typeof(T));
                xz.Serialize(sw, obj);
                return sw.ToString();
            }
        }
        public static T Deserialize<T>(string str)
        {
            using (StringReader sr = new StringReader(str))
            {
                XmlSerializer xz = new XmlSerializer(typeof(T));

                return (T)xz.Deserialize(sr);
            }
        }
    }
}