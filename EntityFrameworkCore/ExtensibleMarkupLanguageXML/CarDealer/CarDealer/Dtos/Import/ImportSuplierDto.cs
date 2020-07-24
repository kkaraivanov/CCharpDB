namespace CarDealer.Dtos.Import
{
    using System.Xml.Serialization;

    [XmlType("Supplier")]
    public class ImportSuplierDto
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("isImporter")]
        public bool IsImporter { get; set; }
    }
}