namespace PetStore.Services.Model.Distributor
{
    using System;

    public class Delivery
    {
        private int Id { get; set; }

        public DateTime DeliveryDate { get; set; }

        public decimal Cost { get; set; }

        public int DistributorId { get; set; }

        public int OrderId { get; set; }
    }
}