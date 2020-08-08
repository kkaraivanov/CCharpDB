namespace VaporStore.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Enums;

    public class Card
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(19)]
        [RegularExpression("^(\\d{4}) (\\d{4}) (\\d{4}) (\\d{4})$")]
        public string Number { get; set; }

        [Required]
        [MaxLength(3)]
        [RegularExpression("^(\\d{3})$")]
        public string Cvc { get; set; }

        [Required]
        public CardType Type { get; set; }


        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; } = new HashSet<Purchase>();
    }
}

//Id – integer, Primary Key
//Number – text, which consists of 4 pairs of 4 digits, separated by spaces (ex. “1234 5678 9012 3456”)
//(required)
//Cvc – text, which consists of 3 digits (ex. “123”) (required)
//Type – enumeration of type CardType, with possible values (“Debit”, “Credit”) (required)
//UserId – integer, foreign key(required)
//User – the card’s user (required)
//Purchases – collection of type Purchase
