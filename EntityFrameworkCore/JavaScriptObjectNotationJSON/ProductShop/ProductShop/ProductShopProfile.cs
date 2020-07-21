namespace ProductShop
{
    using System.Linq;
    using AutoMapper;
    using DTO.Category;
    using DTO.Product;
    using DTO.User;
    using Models;

    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<Product, ProductsInRangeDTO>()
                .ForMember(x =>
                    x.SellerName, y =>
                    y.MapFrom(x => $"{x.Seller.FirstName} {x.Seller.LastName}"));

            this.CreateMap<Product, UserSoldProductDTO>()
                .ForMember(x =>
                    x.FirstName, y =>
                    y.MapFrom(x => x.Buyer.FirstName))
                .ForMember(x =>
                    x.LastName, y =>
                    y.MapFrom(x => x.Buyer.LastName));

            this.CreateMap<User, UserWithSoldProductDTO>()
                .ForMember(x =>
                    x.SoldProducts, y =>
                    y.MapFrom(x => x.ProductsSold.Where(ps => ps.Buyer != null)));

            this.CreateMap<User, GetUsersWithProducts>()
                .ForMember(x =>
                    x.UsersCount, y =>
                    y.MapFrom(x => x.Id));

            this.CreateMap<Category, CategoriesByProductsCountDTO>()
                .ForMember(x =>
                    x.AveragePrice, y =>
                    y.MapFrom(x => x.CategoryProducts.Average(x => x.Product.Price)))
                .ForMember(x =>
                    x.TotalRevenue, y =>
                    y.MapFrom(x => x.CategoryProducts.Sum(x => x.Product.Price)));

            // Import
            this.CreateMap<UsersDTO, User>();
        }
    }
}