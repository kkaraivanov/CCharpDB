namespace PetStore.Data.Model.Distributor
{
    using FoodModel;

    public class DeliveryFood
    {
        public int DeliveryId { get; set; }
        public DistributorDelivery DistributorDelivery { get; set; }

        public int FoodId { get; set; }
        public Food Food { get; set; }
    }
}