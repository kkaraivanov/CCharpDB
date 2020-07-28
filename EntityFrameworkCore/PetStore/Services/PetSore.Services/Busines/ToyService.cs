namespace PetSore.Services.Busines
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using Interfaces;
    using PetStore.Data;
    using PetStore.Data.Model.ToyModel;
    using PetStore.Services.Model.Toy;

    public class ToyService : IToyService
    {
        private readonly PetStoreDbContext db;

        public ToyService(PetStoreDbContext context) => db = context;

        public void Create(string name, string description, decimal price, string brandName, string categoryName)
        {
            var brand = new BrandService(db);
            if(!brand.SearchBrandByName(brandName).Any())
                brand.Create(brandName);

            var category = new CategoryService(db);
            if (!category.SearchCategoryByName(categoryName).Any())
                category.Create(categoryName);

            var toy = new Toy
            {
                Name = name,
                Description = description,
                Price = price,
                BrandId = db.Brands.Where(x => x.Name == brandName).Select(x => x.Id).First(),
                CategoryId = db.Categories.Where(x => x.Name == categoryName).Select(x => x.Id).First()
            };

            var toyExist = SearchToysByName(name).Any();
            var currentToy = db.Categories
                .Where(x => x.Name.ToLower().Contains(categoryName.ToLower()))
                .Select(x => x.Id).First();
            
            if (toyExist)
            {
                var updateToy = db.Toys.First(x => x.Id == currentToy);
                var toyPrice = updateToy.Price;
                if (toy.Price != toyPrice)
                    updateToy.Price = toy.Price;

                updateToy.Quantity += 1;
                db.Toys.Update(updateToy);
            }
            else
                db.Toys.Add(toy);
            
            db.SaveChanges();
        }

        public void Create(string name, string description, decimal price, string brandName, string categoryName, int quantity)
        {
            var brand = new BrandService(db);
            if (!brand.SearchBrandByName(brandName).Any())
                brand.Create(brandName);

            var category = new CategoryService(db);
            if (!category.SearchCategoryByName(categoryName).Any())
                category.Create(categoryName);

            var toy = new Toy
            {
                Name = name,
                Description = description,
                Price = price,
                Quantity = quantity,
                BrandId = db.Brands.Where(x => x.Name == brandName).Select(x => x.Id).First(),
                CategoryId = db.Categories.Where(x => x.Name == categoryName).Select(x => x.Id).First()
            };

            var toyExist = SearchToysByName(name).Any();
            var currentToy = db.Toys
                .Where(x => x.Name.ToLower().Contains(name.ToLower()))
                .Select(x => x.Id).First();

            if (toyExist)
            {
                var updateToy = db.Toys.First(x => x.Id == currentToy);
                var toyPrice = updateToy.Price;
                if (price != toyPrice)
                    updateToy.Price = price;

                updateToy.Quantity += quantity;
                db.Toys.Update(updateToy);
            }
            else
                db.Toys.Add(toy);

            db.SaveChanges();
        }

        public IEnumerable<Toys> SearchToysByName(string name) => db.Toys
            .Where(x => x.Name.ToLower().Contains(name.ToLower()))
            .Select(x => new Toys
            {
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Quantity = x.Quantity,
                Category = x.Category.Name,
                Brand = x.Brand.Name
            }).ToList();

        public IEnumerable<Toys> ListToysByCategory(string categoryName) => db.Toys
            .Where(x => x.Category.Name.ToLower().Contains(categoryName.ToLower()))
            .Select(x => new Toys
            {
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Category = x.Category.Name,
                Brand = x.Brand.Name
            }).ToList();

        public decimal TotalPrice => db.Toys.Sum(x => x.Price);
    }
}