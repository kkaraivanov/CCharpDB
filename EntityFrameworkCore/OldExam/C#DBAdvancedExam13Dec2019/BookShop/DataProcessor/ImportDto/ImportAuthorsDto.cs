namespace BookShop.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    public class ImportAuthorsDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string LastName { get; set; }

        [RegularExpression("^(\\d{3})\\-(\\d{3})\\-(\\d{4})$")]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [JsonProperty("Books")]
        public AuthorsBooksDto[] AuthorsBooks{ get; set; }
    }
}