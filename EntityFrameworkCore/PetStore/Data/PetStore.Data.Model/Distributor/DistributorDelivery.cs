namespace PetStore.Data.Model.Distributor
{
    using System;
    using System.Collections.Generic;
    using StoreModel;

    public class DistributorDelivery
    {
        public int Id { get; set; }

        public DateTime DeliveryDate { get; set; }

        public decimal Cost { get; set; }

        public int DistributorId { get; set; }
        public Distributor Distributor { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public ICollection<DeliveryFood> DeliveryFoods { get; set; } = new HashSet<DeliveryFood>();

        public ICollection<DeliveryToy> DeliveryToys { get; set; } = new HashSet<DeliveryToy>();
    }
}