namespace PetSore.Services.Busines
{
    using PetStore.Data;
    using PetStore.Data.Model.Customer;

    public class CustomerService
    {
        private readonly PetStoreDbContext db;

        public CustomerService(PetStoreDbContext context) => db = context;

        public void Create(string name, string email, int phone)
        {
            var firstName = name.Split(' ')[0];
            var lastName = name.Split(' ')[1];

            var customer = new Customer
            {
                FirstName = firstName,
                LasttName = lastName,
                Email = email,
                PhoneNumber = phone
            };

            db.Customers.Add(customer);
            db.SaveChanges();
        }
    }
}