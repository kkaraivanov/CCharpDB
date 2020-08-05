namespace MusicHub.DataProcessor.ImportDtos
{
    using System.Xml.Serialization;

    [XmlType("Song")]
    public class ExportSongsAboveDurationDto
    {
        [XmlElement("SongName")]
        public string SongName { get; set; }

        [XmlElement("Writer")]
        public string WriterName { get; set; }

        [XmlElement("Performer")]
        public string PerformerFullName { get; set; }

        [XmlElement("AlbumProducer")]
        public string ProducerName { get; set; }

        [XmlElement("Duration")]
        public string Duration { get; set; }
    }
}