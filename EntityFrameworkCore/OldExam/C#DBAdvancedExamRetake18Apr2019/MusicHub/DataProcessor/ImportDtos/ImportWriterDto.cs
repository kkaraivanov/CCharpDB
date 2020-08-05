namespace MusicHub.DataProcessor.ImportDtos
{
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    public class ImportWriterDto
    {
        [Required]
        [MinLength(3), MaxLength(20)]
        [JsonProperty("Name")]
        public string Name { get; set; }

        [RegularExpression("[A-Z][a-z]+ [A-Z][a-z]+")]
        [JsonProperty("Pseudonym")]
        public string Pseudonym  { get; set; }
    }
}