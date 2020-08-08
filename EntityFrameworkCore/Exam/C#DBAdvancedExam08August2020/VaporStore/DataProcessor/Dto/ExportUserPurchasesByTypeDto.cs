namespace VaporStore.DataProcessor.Dto
{
    using System.Xml.Serialization;

    [XmlType("User")]
    public class ExportUserPurchasesByTypeDto
    {
        [XmlAttribute("username")]
        public string UserName { get; set; }

        [XmlArray("Purchases")]
        public ExportPurchase[] Purchases { get; set; }

        [XmlElement("TotalSpent")]
        public decimal TotalSpent { get; set; }
    }

    [XmlType("Purchase")]
    public class ExportPurchase
    {
        [XmlElement("Card")]
        public string CardNumber { get; set; }

        [XmlElement("Cvc")]
        public string CardCvc { get; set; }

        [XmlElement("Date")]
        public string CardDate { get; set; }

        [XmlElement("Game")]
        public ExportGame Game { get; set; }
    }

    [XmlType("Game")]
    public class ExportGame
    {
        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlElement("Genre")]
        public string Genre { get; set; }

        [XmlElement("Price ")]
        public decimal Price { get; set; }
    }
}