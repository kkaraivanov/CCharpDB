namespace PetStore.Data.Model.Customer
{
    using System.Collections.Generic;
    using StoreModel;

    public class Customer
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LasttName { get; set; }

        public string Email { get; set; }

        public int PhoneNumber { get; set; }

        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}