namespace MusicHub.DataProcessor.ImportDtos
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.CompilerServices;

    public class ImportProducersAlbumsDto
    {
        [Required]
        [MinLength(3), MaxLength(20)]
        public string Name { get; set; }

        [RegularExpression("[A-Z][a-z]+ [A-Z][a-z]+")]
        public string Pseudonym { get; set; }

        [RegularExpression(@"\+359 [0-9]{3} [0-9]{3} [0-9]{3}")]
        public string PhoneNumber { get; set; }

        
        public ImportAlbum[] Albums { get; set; }
    }

    public class ImportAlbum
    {
        [Required]
        [MinLength(3), MaxLength(40)]
        public string Name { get; set; }

        public string ReleaseDate { get; set; }
    }
}