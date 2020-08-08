namespace VaporStore.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<GameTag> GameTags { get; set; } = new HashSet<GameTag>();
    }
}

//Id – integer, Primary Key
//Name – text (required)
//GameTags - collection of type GameTag
