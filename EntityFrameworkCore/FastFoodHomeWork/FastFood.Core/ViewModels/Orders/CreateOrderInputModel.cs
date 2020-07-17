namespace FastFood.Core.ViewModels.Orders
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CreateOrderInputModel
    {
        [Required]
        public string Customer { get; set; }

        public int ItemId { get; set; }

        public int EmployeeId { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        public DateTime DateTime { get; set; } = DateTime.Now;

        public int Quantity { get; set; }
    }
}
