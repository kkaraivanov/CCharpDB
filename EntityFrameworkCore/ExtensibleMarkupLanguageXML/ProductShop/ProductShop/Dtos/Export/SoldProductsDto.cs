namespace ProductShop.Dtos.Export
{
    using System.Xml.Serialization;

    public class SoldProductsDto
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public ProductDto[] Products { get; set; }
    }
}