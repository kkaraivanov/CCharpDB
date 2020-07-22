namespace ProductShop.Dtos.Export
{
    using System.Xml.Serialization;

    [XmlType("User")]
    public class ExportUsersDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlArray("soldProducts")]
        public ExportProductDto[] SoldProducts { get; set; }
    }
}