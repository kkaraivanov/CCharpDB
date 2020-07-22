namespace ProductShop.Dtos.Export
{
    using System.Xml.Serialization;

    public class ExportUsersWithProductsDto
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public UserDto[] Users { get; set; }
    }
}