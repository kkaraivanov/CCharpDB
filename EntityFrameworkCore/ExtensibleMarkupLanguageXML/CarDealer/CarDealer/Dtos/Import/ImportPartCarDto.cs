﻿namespace CarDealer.Dtos.Import
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType("partId")]
    public class ImportPartCarDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}