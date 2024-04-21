using JewelryEC_Backend.Enum;
using JewelryEC_Backend.Models.Deliveries;
using JewelryEC_Backend.Models.OrderItems;
using JewelryEC_Backend.Models.Orders;
using JewelryEC_Backend.Models.Orders.Dto;

namespace JewelryEC_Backend.Mapper
{
    public class OrderMapper
    {
        public static Order OrderFromCreateOrderDto(CreateNewOrderDto orderDto)
        {
            Order newOrder = new Order
            {
                Id = new Guid(),
                UserId = orderDto.UserId,
                OrderStatus = OrderStatus.Pending,
                CreateDate = DateTime.Now,
            };
            newOrder.OrderItems = orderDto.OrderItems.Select(itemDto => new OrderItem
            {
                OrderId = newOrder.Id,
                ProductItemId = itemDto.ProductItemId,
                Quantity = itemDto.Quantity,
                Price = itemDto.Price,
                Subtotal = itemDto.Subtotal,
            }).ToList();
            return newOrder;
        }
    }
}
