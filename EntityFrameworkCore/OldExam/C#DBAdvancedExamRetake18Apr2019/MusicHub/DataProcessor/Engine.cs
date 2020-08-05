namespace MusicHub.DataProcessor
{
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    public static class Engine
    {
        public static string JsonSerializer<T>(T obj) => JsonConvert.SerializeObject(obj, Formatting.Indented);

        public static T JsonDeserializer<T>(string json) => JsonConvert.DeserializeObject<T>(json);

        public static string XmlSerializer<T>(T obj, string rootAttributeName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootAttributeName));
            var sb = new StringBuilder();
            var writer = new StringWriter(sb);

            serializer.Serialize(writer, obj, GetXmlNamespaces());
            return sb.ToString();
        }

        public static string XmlSerializer<T>(T[] obj, string rootAttributeName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T[]), new XmlRootAttribute(rootAttributeName));
            var sb = new StringBuilder();
            var writer = new StringWriter(sb);

            serializer.Serialize(writer, obj, GetXmlNamespaces());
            return sb.ToString();
        }

        public static T[] XmlDeserializer<T>(string xmlString, string rootAttributeName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T[]), new XmlRootAttribute(rootAttributeName));
            var obj = serializer.Deserialize(new StringReader(xmlString)) as T[];

            return obj;
        }

        public static T XmlDeserializer<T>(string xmlString, string rootAttributeName, bool isObject)
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