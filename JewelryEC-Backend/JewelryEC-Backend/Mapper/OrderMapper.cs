using JewelryEC_Backend.Enum;
using JewelryEC_Backend.Models.CartItems.Entities;
using JewelryEC_Backend.Models.Deliveries;
using JewelryEC_Backend.Models.OrderItems;
using JewelryEC_Backend.Models.Orders;
using JewelryEC_Backend.Models.Orders.Dto;

namespace JewelryEC_Backend.Mapper
{
    public class OrderMapper
    {
        public static Order OrderFromCreateOrderDto(CreateNewOrderDto orderDto, List<OrderItem> orderItems, Guid userId)
        {
            Order newOrder = new Order
            {
                Id = new Guid(),
                UserId = userId,
                OrderStatus = OrderStatus.Pending,
                CreateDate = DateTime.Now,
                PaymentMethod = orderDto.PaymentMethod,
                OrderItems = orderItems
            };
            //decimal totalPrice = 0;
            //newOrder.OrderItems = orderItems.Select(itemDto =>
            //{
            //    totalPrice += itemDto.Subtotal;
            //    return new OrderItem
            //    {
            //        OrderId = newOrder.Id,
            //        ProductItemId = itemDto.ProductItemId,
            //        Quantity = itemDto.Quantity,
            //        Price = itemDto.Price,
            //        Subtotal = itemDto.Subtotal,
            //    };
            //}).ToList();
            //newOrder.TotalPrice = totalPrice;
            return newOrder;
        }
        public static Order OrderFromCreateOrderDto(CreateNewOrderFromCartDto orderDto, List<OrderItem> orderItems, Guid userId)
        {
            Order newOrder = new Order
            {
                Id = new Guid(),
                UserId = userId,
                OrderStatus = OrderStatus.Pending,
                CreateDate = DateTime.Now,
                PaymentMethod = orderDto.PaymentMethod,
            };
            decimal totalPrice = 0;
            newOrder.OrderItems = orderItems.Select(itemDto =>
            {
                totalPrice += itemDto.Subtotal;
                return new OrderItem
                {
                    OrderId = newOrder.Id,
                    ProductItemId = itemDto.ProductItemId,
                    Quantity = itemDto.Quantity,
                    Price = itemDto.Price,
                    Subtotal = itemDto.Subtotal,
                };
            }).ToList();
            newOrder.TotalPrice = totalPrice;
            return newOrder;
        }
    }
}
