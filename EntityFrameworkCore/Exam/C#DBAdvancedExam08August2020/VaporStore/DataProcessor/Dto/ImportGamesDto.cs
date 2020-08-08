namespace VaporStore.DataProcessor.Dto
{
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    using Newtonsoft.Json;

    public class ImportGamesDto
    {
        [Required]
        public string Name { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public string Price { get; set; }

        public string ReleaseDate { get; set; }

        public string Developer { get; set; }

        public string Genre { get; set; }

        public string[] Tags { get; set; }
    }

    public class ImportTags
    {
        //[Required]
        public string Name { get; set; }
    }
}