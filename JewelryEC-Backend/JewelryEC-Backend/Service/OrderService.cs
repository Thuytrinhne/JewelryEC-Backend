using AutoMapper;
using JewelryEC_Backend.Core.Utilities.Results;
using JewelryEC_Backend.Enum;
using JewelryEC_Backend.Mapper;
using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.CartItems.Entities;
using JewelryEC_Backend.Models.OrderItems;
using JewelryEC_Backend.Models.OrderItems.Dto;
using JewelryEC_Backend.Models.Orders;
using JewelryEC_Backend.Models.Orders.Dto;
using JewelryEC_Backend.Models.Products;
using JewelryEC_Backend.Models.Shippings;
using JewelryEC_Backend.Repository.IRepository;
using JewelryEC_Backend.Service.IService;
using JewelryEC_Backend.UnitOfWork;
using NuGet.Protocol;

namespace JewelryEC_Backend.Service
{
    public class OrderService: IOrderService
    {
        private readonly IOrderRepository _orderRe;
        private readonly ICartItemRepository _cartRe;
        private readonly IShippingRepository _shippingRe;
        private readonly IProductItemRespository _productItemRe;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _orderRe = unitOfWork.Orders;
            _shippingRe = unitOfWork.Shippings;
            _cartRe = unitOfWork.CartItems;
            _productItemRe = unitOfWork.ProductItem;
        }
        public async Task<ResponseDto> GetAll()
        {
            return new SuccessDataResult<List<Order>>(await _orderRe.GetAllAsync());
        }
        public async Task<ResponseDto> Add(CreateNewOrderDto orderDto)
        {
            Shipping shipping = ShippingMapper.ShippingFromCreateNewOrderDto(orderDto.DeliveryId, orderDto.DeliveryDto);
            List<OrderItem> orderItems = orderItemsFromOrderItemDto(orderDto.OrderItems.ToList());
            Order order = OrderMapper.OrderFromCreateOrderDto(orderDto, orderItems);
            shipping.OrderId = order.Id;
            Console.WriteLine(order.ToJson());
            await _orderRe.AddAsync(order);
            await _orderRe.SaveChangeAsync();
            await _shippingRe.AddAsync(shipping);
            Order newOrder =  _orderRe.GetById(order.Id);
            return new SuccessResult("Add order successfully", newOrder);
        }
        public async Task<ResponseDto> AddFromCart(CreateNewOrderFromCartDto dto)
        {
            Shipping shipping = ShippingMapper.ShippingFromCreateNewOrderDto(dto.DeliveryId, dto.DeliveryDto);
            List<CartItem> cartItems = _cartRe.GetListCartItems(dto.CartItemIds);
            List<OrderItem> orderItems = orderItemsFromCartItems(cartItems);
            Order order = OrderMapper.OrderFromCreateOrderDto(dto, orderItems);
            shipping.OrderId = order.Id;
            Console.WriteLine(order.ToJson());
            await _orderRe.AddAsync(order);
            await _orderRe.SaveChangeAsync();
            await _shippingRe.AddAsync(shipping);
            Order newOrder = _orderRe.GetById(order.Id);
            return new SuccessResult("Add order successfully", newOrder);
        }

        public async Task<ResponseDto> UpdateOrderStatus(Guid orderId, OrderStatus orderStatus)
        {
            Order order = await _orderRe.GetOrder(Order => Order.Id == (orderId));
            if(order != null)
            {
                order.OrderStatus = orderStatus;
                _orderRe.Update(order);
                return new SuccessDataResult<Order>(order);
            }
            return new ErrorResult();
        }
        public async Task<ResponseDto> GetById(Guid id)
        {
            Order order = await _orderRe.GetOrder(Order => Order.Id == (id));
            if (order != null)
            {
                return new SuccessDataResult<Order>(order);
            }
            return new ErrorResult();
        }

        public async Task<ResponseDto> GetOrdersByUserId(Guid userId)
        {
            List<Order> orders = _orderRe.GetOrders(order => order.UserId == userId).Result.ToList();
            return new SuccessDataResult<List<Order>>(orders);
        }
        private List<OrderItem> orderItemsFromCartItems(List<CartItem> cartItems)
        {
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var cartItem in cartItems)
            {
                ProductVariant productItem =  _productItemRe.GetById(cartItem.ProductItemId);
                OrderItem orderItem = new OrderItem
                {
                    ProductItemId = cartItem.ProductItemId,
                    Quantity = cartItem.Count,
                    Price = productItem.Price,
                    Subtotal = productItem.Price * cartItem.Count,
                };
                orderItems.Add(orderItem);
            }
            return orderItems;
        }
        private List<OrderItem> orderItemsFromOrderItemDto(List<CreateOrderItemDto> orderItemDtos)
        {
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in orderItemDtos)
            {
                ProductVariant productItem = _productItemRe.GetById(item.ProductItemId);
                OrderItem orderItem = new OrderItem
                {
                    ProductItemId = item.ProductItemId,
                    Quantity = item.Quantity,
                    Price = productItem.Price,
                    Subtotal = productItem.Price * item.Quantity,
                };
                orderItems.Add(orderItem);
            }
            return orderItems;
        }

    }
}
