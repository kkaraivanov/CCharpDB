namespace PetStore.Data.Model.ToyModel
{
    using StoreModel;

    public class ToyOrders
    {
        public int ToyId { get; set; }
        public Toy Toy { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}