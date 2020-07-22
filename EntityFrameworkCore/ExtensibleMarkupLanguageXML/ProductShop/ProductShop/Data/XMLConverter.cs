namespace ProductShop.Data
{
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    public static class XMLConverter
    {
        public static string Serializer<T>(T obj, string rootAttributeName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootAttributeName));
            var sb = new StringBuilder();
            var writer = new StringWriter(sb);

            serializer.Serialize(writer, obj, GetXmlNamespaces());
            return sb.ToString();
        }

        public static string Serializer<T>(T[] obj, string rootAttributeName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T[]), new XmlRootAttribute(rootAttributeName));
            var sb = new StringBuilder();
            var writer = new StringWriter(sb);

            serializer.Serialize(writer, obj, GetXmlNamespaces());
            return sb.ToString();
        }

        public static T[] Deserializer<T>(string xmlString, string rootAttributeName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T[]), new XmlRootAttribute(rootAttributeName));
            var obj = serializer.Deserialize(new StringReader(xmlString)) as T[];

            return obj;
        }

        public static T Deserializer<T>(string xmlString, string rootAttributeName, bool isObject)
            where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootAttributeName));
            var obj = serializer.Deserialize(new StringReader(xmlString)) as T;

            return obj;
        }

        private static XmlSerializerNamespaces GetXmlNamespaces()
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            return ns;
        }
    }
}