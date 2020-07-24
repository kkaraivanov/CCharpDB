namespace CarDealer.Dtos.Export
{
    using System.Xml.Serialization;

    [XmlType("car")]
    public class GetCarsWithTheirListOfPartsDto
    {
        [XmlAttribute("make")] public string Make { get; set; }

        [XmlAttribute("model")] public string Model { get; set; }

        [XmlAttribute("travelled-distance")] public long TravelledDistance { get; set; }

        [XmlElement("parts")] public GetPart Parts { get; set; }
    }

    public class GetPart
    {
        [XmlElement("part")] public Parts[] GetParts { get; set; }
    }

    public class Parts
    {
        [XmlAttribute("name")] public string Name { get; set; }

        [XmlAttribute("price")] public decimal Price { get; set; }
    }
}