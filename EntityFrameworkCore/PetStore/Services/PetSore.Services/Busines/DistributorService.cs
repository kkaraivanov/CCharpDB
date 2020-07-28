namespace PetSore.Services.Busines
{
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.EntityFrameworkCore.Internal;
    using PetStore.Data;
    using PetStore.Data.Model.Distributor;

    public class DistributorService
    {
        private readonly PetStoreDbContext db;

        public DistributorService(PetStoreDbContext context) => db = context;

        public void Create(string name, string bancAcount)
        {
            var dist = new Distributor
            {
                Name = name,
                BankAccount = bancAcount
            };

            db.Distributors.Add(dist);

            if (!db.Distributors.Any(x => x.Name == name))
                db.SaveChanges();
        }
    }
}