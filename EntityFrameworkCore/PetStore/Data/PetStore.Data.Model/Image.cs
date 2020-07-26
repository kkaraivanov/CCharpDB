namespace PetSore.Data.Model
{
    using System.Collections.Generic;
    using PetStore.Data.Model.PetModel;

    public class Image
    {
        public Image()
        {
            Pets = new HashSet<Pet>();
        }

        public int Id { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public ICollection<Pet> Pets { get; set; }
    }
}