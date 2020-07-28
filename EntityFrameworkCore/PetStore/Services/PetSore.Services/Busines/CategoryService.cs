namespace PetSore.Services.Busines
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using PetStore.Data;
    using PetStore.Data.Model.StoreModel;
    using PetStore.Services.Model.Category;

    public class CategoryService : ICategoryService
    {
        private readonly PetStoreDbContext db;

        public CategoryService(PetStoreDbContext context) => db = context;

        public void Create(string name)
        {
            //TODO: Check validation  

            var category = new Category
            {
                Name = name
            };

            db.Categories.Add(category);
            if (!SearchCategoryByName(name).Any())
                db.SaveChanges();
        }

        public void Create(string name, string description)
        {
            //TODO: Check validation  

            var category = new Category
            {
                Name = name,
                Description = description
            };

            db.Categories.Add(category);
            if (!SearchCategoryByName(name).Any())
                db.SaveChanges();
        }

        public IEnumerable<Categories> SearchCategoryByName(string name) => db.Categories
                .Where(x => x.Name == name)
                .Select(x => new Categories
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();
    }
}