namespace PetSore.Services.Busines
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using PetStore.Data;
    using PetStore.Data.Model.StoreModel;
    using PetStore.Services.Model.Brands;

    public class BrandService : IBrandService
    {
        private readonly PetStoreDbContext db;

        public BrandService(PetStoreDbContext context) => db = context;

        public void Create(string name)
        {
            //TODO: Check validation   

            var brand = new Brand
            {
                Name = name
            };

            db.Brands.Add(brand);
            db.SaveChanges();
        }

        public IEnumerable<Brands> SearchBrandByName(string name)
        {
            var brands = db.Brands
                .Where(x => x.Name.ToLower().Contains(name.ToLower())) //skip input validation
                .Select(x => new Brands
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();

            return brands;
        }
    }
}