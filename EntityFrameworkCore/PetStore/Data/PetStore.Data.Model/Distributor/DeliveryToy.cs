namespace PetStore.Data.Model.Distributor
{
    using ToyModel;

    public class DeliveryToy
    {
        public int DeliveryId { get; set; }
        public DistributorDelivery DistributorDelivery { get; set; }

        public int ToyId { get; set; }
        public Toy Toy { get; set; }
    }
}