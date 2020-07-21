namespace ProductShop
{
    using System;
    using AutoMapper;
    using Data;
    using Extensions;

    public class StartUp
    {
        static void Main(string[] args)
        {
            var db = new ProductShopContext();
            CreateDataBase(db);
            InitializeMapper();

            //var usersFileName = "users.json";
            //var productsFileName = "products.json";
            //var categoriesFileName = "categories.json";
            //var categoryProductsFileName = "categories-products.json";

            //var resultUsers = db.ImportUsers(usersFileName.Read());
            //var productsResult = db.ImportProducts(productsFileName.Read());
            //var categoriesResult = db.ImportCategories(categoriesFileName.Read());
            //var categoryProductsResult = db.ImportCategoryProducts(categoryProductsFileName.Read());

            //Console.WriteLine(resultUsers);
            //Console.WriteLine(productsResult);
            //Console.WriteLine(categoriesResult);
            //Console.WriteLine(categoryProductsResult);

            var productsInRangeFileName = "users-and-products.json";
            var productsInRangeContent = db.GetUsersWithProducts();
            Console.WriteLine(productsInRangeFileName.Write(productsInRangeContent));
        }

        private static void InitializeMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
        }

        private static void CreateDataBase(ProductShopContext dbContext)
        {
            dbContext.Database.EnsureCreated();
            Console.WriteLine("Database was successfully created!");
        }
    }
}
