using System.IO;
using System.Xml.Serialization;

namespace TraceService.Application.Extensions
{
   

    public static class XmlExtensions
    {
        public static string Serialize<T>(this T xmlObject)
        {
            string result = string.Empty;
            XmlSerializer ser = new(typeof(T));

            using (StringWriter textWriter = new())
            {
                ser.Serialize(textWriter, xmlObject);
                result = textWriter.ToString();
            }

            return result;
        }

        public static T Deserialize<T>(this string xmlText)
        {
            XmlSerializer responseSerializer = new(typeof(T));
            T request = (T)responseSerializer.Deserialize(new StringReader(xmlText));
            return request;
        }
    }
}
