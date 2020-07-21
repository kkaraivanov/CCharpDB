namespace ProductShop.Extensions
{
    using System.Linq;
    using AutoMapper;
    using Data;
    using DTO.User;
    using Models;
    using Newtonsoft.Json;

    public static class ImportDataExtension
    {
        /// <summary>
        /// Query 2. Import Users
        /// </summary>
        /// <param name="context"></param>
        /// <param name="inputJson"></param>
        /// <returns></returns>
        public static string ImportUsers(this ProductShopContext context, string inputJson)
        {
            var userDTO = inputJson.DeserializeObject<UsersDTO>();
            var users = userDTO.Select(Mapper.Map<User>).ToArray();
            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        /// <summary>
        /// Query 3. Import Products
        /// </summary>
        /// <param name="context"></param>
        /// <param name="inputJson"></param>
        /// <returns></returns>
        public static string ImportProducts(this ProductShopContext context, string inputJson)
        {
            var products = inputJson.DeserializeObject<Product>();
            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }

        /// <summary>
        /// Query 4. Import Categories
        /// </summary>
        /// <param name="context"></param>
        /// <param name="inputJson"></param>
        /// <returns></returns>
        public static string ImportCategories(this ProductShopContext context, string inputJson)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            var categories = inputJson.DeserializeObject<Category>(settings);
            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Length}";
        }

        /// <summary>
        /// Query 5. Import Categories and Products
        /// </summary>
        /// <param name="context"></param>
        /// <param name="inputJson"></param>
        /// <returns></returns>
        public static string ImportCategoryProducts(this ProductShopContext context, string inputJson)
        {
            var categoryProducts = inputJson.DeserializeObject<CategoryProduct>();
            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Length}";
        }
    }
}