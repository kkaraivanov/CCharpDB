namespace PetStore.Data.Model.ToyModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Distributor;
    using StoreModel;
    using static DataValidationAttribute;

    public class Toy
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLenght)]
        public string Name { get; set; }

        [MaxLength(DescriptionMaxLenght)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<ToyOrder> ToyOrders { get; set; } = new HashSet<ToyOrder>();

        public ICollection<DeliveryToy> DeliveryToys { get; set; } = new HashSet<DeliveryToy>();
    }
}