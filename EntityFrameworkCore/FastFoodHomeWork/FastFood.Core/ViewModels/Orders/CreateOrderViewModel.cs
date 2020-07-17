namespace FastFood.Core.ViewModels.Orders
{
    using System.Collections.Generic;
    using FastFood.Models.Enums;

    public class CreateOrderViewModel
    {
        public List<int> Items { get; set; }

        public List<int> Employees { get; set; }

        public OrderType Types { get; set; }
    }
}
