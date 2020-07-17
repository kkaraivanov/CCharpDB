namespace FastFood.Core.MappingConfiguration
{
    using AutoMapper;
    using FastFood.Models;
    using ViewModels.Categories;
    using ViewModels.Employees;
    using ViewModels.Items;
    using ViewModels.Orders;
    using ViewModels.Positions;

    public class FastFoodProfile : Profile
    {
        public FastFoodProfile()
        {
            //Positions
            this.CreateMap<CreatePositionInputModel, Position>()
                .ForMember(x => 
                    x.Name, y => 
                    y.MapFrom(s => s.PositionName));

            this.CreateMap<Position, PositionsAllViewModel>()
                .ForMember(x => 
                    x.Name, y => 
                    y.MapFrom(s => s.Name));

            this.CreateMap<Position, PositionsPositionViewModel>();

            //Categories
            this.CreateMap<CreateCategoryInputModel, Category>()
                .ForMember(
                    x => 
                        x.Name, y =>
                        y.MapFrom(x => x.CategoryName));

            this.CreateMap<Category, CategoryAllViewModel>()
                .ForMember(x => 
                        x.Name, y =>
                        y.MapFrom(x => x.Name));

            // Items
            this.CreateMap<CreateItemInputModel, Item>();

            this.CreateMap<Category, CreateItemViewModel>()
                .ForMember(x =>
                    x.CategoryId, y =>
                    y.MapFrom(x => x.Id));

            this.CreateMap<Item, ItemsAllViewModels>()
                .ForMember(x =>
                    x.Name, y =>
                    y.MapFrom(x => x.Name))
                .ForMember(x =>
                    x.Price, y =>
                    y.MapFrom(x => x.Price))
                .ForMember(x =>
                    x.Category, y =>
                    y.MapFrom(x => string.Join(", ", x.Category.Name)));

            // Employee
            this.CreateMap<RegisterEmployeeInputModel, Employee>();

            this.CreateMap<Position, RegisterEmployeeViewModel>()
                .ForMember(x =>
                    x.PositionId, y => 
                    y.MapFrom(p => p.Id));

            this.CreateMap<Employee, EmployeesAllViewModel>()
                .ForMember(x => 
                    x.Position, y => 
                    y.MapFrom(x => string.Join(", ", x.Position.Name)));

            // Order
            this.CreateMap<CreateOrderInputModel, OrderItem>();

            this.CreateMap<CreateOrderInputModel, Order>()
                .ForMember(x => 
                    x.Type, y => 
                    y.MapFrom(x => x.Type));

            this.CreateMap<OrderItem, OrderAllViewModel>()
                .ForMember(x =>
                    x.Customer, y =>
                    y.MapFrom(x => string.Join(", ", x.Order.Customer)))
                .ForMember(x =>
                    x.Employee, y =>
                    y.MapFrom(x => x.Order.Employee.Name))
                .ForMember(x =>
                    x.DateTime, y =>
                    y.MapFrom(x => string.Join(", ", x.Order.DateTime)));
        }
    }
}
