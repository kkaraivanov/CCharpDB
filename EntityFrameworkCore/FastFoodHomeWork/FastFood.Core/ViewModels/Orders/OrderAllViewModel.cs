namespace FastFood.Core.ViewModels.Orders
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class OrderAllViewModel
    {
        public int OrderId { get; set; }

        public string Customer { get; set; }

        public string Employee { get; set; }

        [NotMapped]
        public decimal TotalPrice { get; set; }

        public string DateTime { get; set; }
    }
}
