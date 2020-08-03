namespace Cinema.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        public decimal Price { get; set; }

        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey(nameof(Projection))]
        public int ProjectionId { get; set; }
        public Projection Projection { get; set; }
    }
}
