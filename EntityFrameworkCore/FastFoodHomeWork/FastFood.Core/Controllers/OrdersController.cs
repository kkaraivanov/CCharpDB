namespace FastFood.Core.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using FastFood.Models;
    using FastFood.Models.Enums;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Orders;

    public class OrdersController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public OrdersController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            var viewOrder = new CreateOrderViewModel
            {
                Items = this.context.Items.Select(x => x.Id).ToList(),
                Employees = this.context.Employees.Select(x => x.Id).ToList()
            };
            ;
            return this.View(viewOrder);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderInputModel model)
        {
            if (!ModelState.IsValid)
                return this.RedirectToAction("Error", "Home");

            var request = Request.Form["orderType"].ToString();
            OrderType type;

            if (Enum.TryParse(request, out type))
                model.Type = (int)type;

            var order = this.mapper.Map<Order>(model);
            SaveOrder(order);

            var newOrderId = this.context.Orders.First(x => x.Id == order.Id);
            if(newOrderId == null)
                return this.RedirectToAction("Error", "Home");

            var orderItems = this.mapper.Map<OrderItem>(model);
            orderItems.OrderId = order.Id;
            SaveOrderItems(orderItems);
            
            return this.RedirectToAction("All", "Orders");
        }

        private void SaveOrder(Order order)
        {
            this.context.Orders.Add(order);
            this.context.SaveChanges();
        }

        private void SaveOrderItems(OrderItem orderItem)
        {
            this.context.OrderItems.Add(orderItem);
            this.context.SaveChanges();
        }

        public IActionResult All()
        {
            var orders = this.context.OrderItems
                .ProjectTo<OrderAllViewModel>(mapper.ConfigurationProvider)
                .ToList();
            var getPrice = this.context.OrderItems.Select(x => new
            {
                x.OrderId,
                TotalPrice = x.Quantity * x.Item.Price
            }).ToList();
            
            foreach (var price in getPrice)
            {
                for (int i = 0; i < orders.Count; i++)
                {
                    if (orders[i].OrderId == price.OrderId)
                        orders[i].TotalPrice = price.TotalPrice;
                }
            }

            return this.View(orders);
        }
    }
}
