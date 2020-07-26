namespace PetStore.Data.Model.PetModel
{
    using System.Collections.Generic;

    public class Bread
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Pet> Pets { get; set; } = new HashSet<Pet>();
    }
}