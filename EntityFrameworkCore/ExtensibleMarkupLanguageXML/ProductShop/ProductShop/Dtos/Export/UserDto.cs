namespace ProductShop.Dtos.Export
{
    using System.Xml.Serialization;

    [XmlType("User")]
    public class UserDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastName { get; set; }

        [XmlElement("age")]
        public int? Age { get; set; }

        [XmlElement("SoldProducts")]
        public SoldProductsDto SoldProducts { get; set; }
    }
}