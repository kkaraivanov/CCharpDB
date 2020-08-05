﻿namespace MusicHub.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public class Album
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3), MaxLength(40)]
        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        public decimal Price => Songs.Sum(x => x.Price);

        [ForeignKey(nameof(Producer))]
        public int? ProducerId { get; set; }
        public Producer Producer { get; set; }

        public virtual ICollection<Song> Songs { get; set; } = new HashSet<Song>();
    }
}
//Id – integer, Primary Key
//Name – text with min length 3 and max length 40 (required)
//ReleaseDate – Date(required)
//Price – calculated property(the sum of all song prices in the album)
//ProducerId – integer foreign key
//Producer – the album’s producer
//Songs – collection of all songs in the album 
