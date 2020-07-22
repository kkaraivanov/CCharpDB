namespace ProductShop
{
    using System;
    using System.IO;
    using System.Linq;
    using Data;
    using Dtos.Export;
    using Dtos.Import;
    using Models;

    public class StartUp
    {
        private static void CreateDatabase(ProductShopContext context)
        {
            //db.Database.EnsureDeleted();
            //Console.WriteLine("Database was deleted!");

            Console.WriteLine(context.Database.EnsureCreated()
                ? $"Database {DbContextConfiguration.DatabaseName} is created!"
                : $"Database {DbContextConfiguration.DatabaseName} is exist!");
        }

        private static void WriteFile(string file, string contents)
        {
            var exportDirectory = @"ExportData\";
            if (!Directory.Exists(exportDirectory))
                Directory.CreateDirectory(exportDirectory);

            var writeDirectory = $"{exportDirectory}{file}";
            File.WriteAllText(writeDirectory, contents);
        }

        public static void Main(string[] args)
        {
            var db = new ProductShopContext();
            //CreateDatabase(db);

            #region Import XML elements to database
            //var sb = new StringBuilder();

            //var usersRead = File.ReadAllText(@"Datasets\users.xml");
            //var productsRead = File.ReadAllText(@"Datasets\products.xml");
            //var categoriesRead = File.ReadAllText(@"Datasets\categories.xml");
            //var categoryProductsRead = File.ReadAllText(@"Datasets\categories-products.xml");

            //sb.AppendLine(ImportUsers(db, usersRead));
            //sb.AppendLine(ImportProducts(db, productsRead));
            //sb.AppendLine(ImportCategories(db, categoriesRead));
            //sb.AppendLine(ImportCategoryProducts(db, categoryProductsRead));

            //Console.WriteLine(sb.ToString().Trim());
            #endregion

            #region Export data from database in XML document

            //var productInRange = GetProductsInRange(db);
            //var getSoldProducts = GetSoldProducts(db);
            //var getCategories = GetCategoriesByProductsCount(db);
            //var getUsersWithProducts = GetUsersWithProducts(db);


            //var productInRangeFile = "products-in-range.xml";
            //var getSoldProductsFile = "users-sold-products.xml";
            //var getCategoriesFile = "categories-by-products.xml";
            //var getUsersWithProductsFile = "users-and-products.xml";


            //WriteFile(productInRangeFile, productInRange);
            //WriteFile(getSoldProductsFile, getSoldProducts);
            //WriteFile(getCategoriesFile, getCategories);
            //WriteFile(getUsersWithProductsFile, getUsersWithProducts);

            #endregion
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var usersDto = XMLConverter.Deserializer<InputUsersDTO>(inputXml, "Users");
            var users = usersDto
                .Select(x => new User
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Age = x.Age
                })
                .ToArray();

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var productDto = XMLConverter.Deserializer<ImportProductsDto>(inputXml, "Products");
            var products = productDto
                .Select(x => new Product
                {
                    Name = x.Name,
                    Price = x.Price,
                    SellerId = x.SellerId,
                    BuyerId = x.BuyerId
                })
                .ToArray();

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var categoriesDto = XMLConverter.Deserializer<ImportCategoriesDto>(inputXml, "Categories");
            var categories = categoriesDto
                .Select(x => new Category
                {
                    Name = x.Name
                })
                .ToArray();

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Length}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var categoriesDto = XMLConverter.Deserializer<ImportCategoryProducts>(inputXml, "CategoryProducts");
            var categoryProducts = categoriesDto
                .Select(x => new CategoryProduct
                {
                    CategoryId = x.CategoryId,
                    ProductId = x.ProductId
                })
                .ToArray();

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Length}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var rootName = "Products";
            var getProducts = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .Select(x => new ExportProductInRangeDto
                {
                    Name = x.Name,
                    Price = x.Price,
                    Buyer = $"{x.Buyer.FirstName} {x.Buyer.LastName}"
                })
                .Take(10)
                .ToArray();
            var result = XMLConverter.Serializer(getProducts, rootName);

            return result;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var rootName = "Users";
            var getUsers = context.Users
                .Where(x => x.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Select(x => new ExportUsersDto
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SoldProducts = x.ProductsSold.Select(p => new ExportProductDto
                    {
                        Name = p.Name,
                        Price = p.Price
                    }).ToArray()
                })
                .Take(5)
                .ToArray();

            var result = XMLConverter.Serializer(getUsers, rootName);
            return result;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var rootName = "Categories";
            var categories = context.Categories
                .Select(x => new ExportCategoriesDto
                {
                    Name = x.Name,
                    Count = x.CategoryProducts.Select(p => p.Product).Count(),
                    AveragePrice = x.CategoryProducts.Average((p => p.Product.Price)),
                    TotalRevenue = x.CategoryProducts.Sum((p => p.Product.Price))
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.TotalRevenue)
                .ToArray();

            var result = XMLConverter.Serializer(categories, rootName);
            return result;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var rootName = "Users";
            var usersWithProducts = context.Users
                .ToArray()
                .Where(x => x.ProductsSold.Any())
                .Select(x => new UserDto
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Age = x.Age,
                    SoldProducts = new SoldProductsDto
                    {
                        Count = x.ProductsSold.Count,
                        Products = x.ProductsSold
                            .Select(p => new ProductDto
                            {
                                Name = p.Name,
                                Price = p.Price
                            })
                            .OrderByDescending(p => p.Price)
                            .ToArray()
                    }
                })
                .OrderByDescending(x => x.SoldProducts.Count)
                .Take(10)
                .ToArray();

            var users = new ExportUsersWithProductsDto
            {
                Count = context.Users.Count(x => x.ProductsSold.Any()),
                Users = usersWithProducts
            };

            var result = XMLConverter.Serializer(users, rootName);
            return result;
        }
    }
}
