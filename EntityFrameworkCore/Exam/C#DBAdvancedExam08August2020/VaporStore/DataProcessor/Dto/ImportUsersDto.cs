namespace VaporStore.DataProcessor.Dto
{
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    public class ImportUsersDto
    {
        [Required]
        [RegularExpression("[A-Z][a-zA-Z][^#&<>\\\"~;$^%{}?]{1,50}$")]
        public string FullName { get; set; }

        [Required]
        [MinLength(3), MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Range(3, 103)]
        public int Age { get; set; }

        public ImportCards[] Cards { get; set; }
    }

    public class ImportCards
    {
        [Required]
        [RegularExpression("^(\\d{4}) (\\d{4}) (\\d{4}) (\\d{4})$")]
        public string Number { get; set; }

        [Required]
        [RegularExpression("^(\\d{3})$")]
        [JsonProperty("CVC")]
        public string Cvc { get; set; }

        public string Type { get; set; }
    }
}