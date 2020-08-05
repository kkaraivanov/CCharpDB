namespace MusicHub.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Enums;

    public class Song
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3), MaxLength(20)]
        public string Name { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTime CreatedOn { get; set; }

        public Genre Genre { get; set; }

        [ForeignKey(nameof(Album))]
        public int?  AlbumId { get; set; }
        public Album Album { get; set; }

        [ForeignKey(nameof(Writer))]
        public int WriterId { get; set; }
        public Writer Writer { get; set; }

        public decimal Price { get; set; }

        public virtual ICollection<SongPerformer> SongPerformers { get; set; } = new HashSet<SongPerformer>();
    }
}
