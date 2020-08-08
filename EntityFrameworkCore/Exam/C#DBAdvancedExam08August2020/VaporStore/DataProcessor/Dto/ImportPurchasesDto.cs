namespace VaporStore.DataProcessor.Dto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Purchase")]
    public class ImportPurchasesDto
    {
        [XmlAttribute("title")]
        public string GameName { get; set; }

        [XmlElement("Type")]
        public string TypeS { get; set; }

        [Required]
        [RegularExpression("^([A-Z0-9]{4})-([A-Z0-9]{4})-([A-Z0-9]{4})$")]
        [XmlElement("Key")]
        public string ProductKey { get; set; }

        [Required]
        [RegularExpression("^(\\d{4}) (\\d{4}) (\\d{4}) (\\d{4})$")]
        [XmlElement("Card")]
        public string Card { get; set; }

        [XmlElement("Date")]
        public string Date { get; set; }
    }
}