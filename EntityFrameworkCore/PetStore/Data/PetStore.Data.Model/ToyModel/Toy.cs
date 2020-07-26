namespace PetStore.Data.Model.ToyModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
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

        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<ToyOrders> ToyOrders { get; set; } = new HashSet<ToyOrders>();
    }
}