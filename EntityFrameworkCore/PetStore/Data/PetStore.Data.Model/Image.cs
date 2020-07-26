namespace PetStore.Data.Model
{
    using System.Collections.Generic;
    using PetModel;

    public class Image
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public ICollection<Pet> Pets { get; set; } = new HashSet<Pet>();
    }
}