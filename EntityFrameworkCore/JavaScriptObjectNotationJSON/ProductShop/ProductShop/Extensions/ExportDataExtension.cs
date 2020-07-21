namespace ProductShop.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using Data;
    using DTO.Category;
    using DTO.Product;
    using DTO.User;
    using Newtonsoft.Json;

    public static class ExportDataExtension
    {
        /// <summary>
        /// Query 5. Export Products in Range
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetProductsInRange(this ProductShopContext context)
        {
            ProductsInRangeDTO[] getProducts = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .ProjectTo<ProductsInRangeDTO>()
                .ToArray();
            
            return getProducts.SerializeObject(Formatting.Indented);
        }

        /// <summary>
        /// Query 6. Export Successfully Sold Products
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetSoldProducts(this ProductShopContext context)
        {
            UserWithSoldProductDTO[] users = context.Users
                .Where(x => x.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .ProjectTo<UserWithSoldProductDTO>()
                .ToArray();
            
            return users.SerializeObject(Formatting.Indented);
        }

        /// <summary>
        /// Query 7. Export Categories by Products Count
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetCategoriesByProductsCount(this ProductShopContext context)
        {
            CategoriesByProductsCountDTO[] categories = context.Categories
                .ProjectTo<CategoriesByProductsCountDTO>()
                .OrderByDescending(x => x.ProductsCount)
                .ToArray();

            return categories.SerializeObject(Formatting.Indented);
        }

        /// <summary>
        /// Query 8. Export Users and Products
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetUsersWithProducts(this ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .Select(x => new
                {
                    lastName = x.LastName,
                    age = x.Age,
                    soldProducts = new
                    {
                        count = x.ProductsSold.Count(p => p.Buyer != null),
                        products = x.ProductsSold
                            .Where(p => p.Buyer != null)
                            .Select(p => new
                            {
                                name = p.Name,
                                price = p.Price.ToString("f2")
                            })
                            .ToArray()
                    }
                })
                .OrderByDescending(x => x.soldProducts.count)
                .ToArray();
            var obj = new
            {
                usersCount = users.Length,
                users = users
            };

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            return obj.SerializeObject(settings);
        }
    }
}