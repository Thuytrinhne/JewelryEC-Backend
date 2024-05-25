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
using JewelryEC_Backend.Models.Voucher;
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
        private readonly IUserCouponRepository _userCouponRe;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _orderRe = unitOfWork.Orders;
            _shippingRe = unitOfWork.Shippings;
            _cartRe = unitOfWork.CartItems;
            _productItemRe = unitOfWork.ProductItem;
            _userCouponRe = unitOfWork.UserCoupon;
        }
        //get orders
        public async Task<ResponseDto> GetAll()
        {
            return new SuccessDataResult<List<Order>>(await _orderRe.GetAllAsync());
        }
        //add new order
        public async Task<ResponseDto> Add(CreateNewOrderDto orderDto)
        {
            Shipping shipping = ShippingMapper.ShippingFromCreateNewOrderDto(orderDto.DeliveryId, orderDto.DeliveryDto);
            Tuple<List<OrderItem>, decimal> res =await orderItemsFromOrderItemDto(orderDto.OrderItems.ToList());
            Order order = OrderMapper.OrderFromCreateOrderDto(orderDto, res.Item1);
            shipping.OrderId = order.Id;
            order.TotalPrice = res.Item2;
            
            Console.WriteLine(order.ToJson());
            await _orderRe.AddAsync(order);
            await _orderRe.SaveChangeAsync();
            await _shippingRe.AddAsync(shipping);
            Order newOrder =  _orderRe.GetById(order.Id);
            return new SuccessResult("Add order successfully", newOrder);
        }
        //add new order from carts
        public async Task<ResponseDto> AddFromCart(CreateNewOrderFromCartDto dto)
        {
            Shipping shipping = ShippingMapper.ShippingFromCreateNewOrderDto(dto.DeliveryId, dto.DeliveryDto);
            List<CartItem> cartItems = _cartRe.GetListCartItems(dto.CartItemIds);
            List<OrderItem> orderItems = orderItemsFromCartItems(cartItems);
            Order order = OrderMapper.OrderFromCreateOrderDto(dto, orderItems);
            //TASK: calculate discount value

            shipping.OrderId = order.Id;
            Console.WriteLine(order.ToJson());
            await _orderRe.AddAsync(order);
            await _orderRe.SaveChangeAsync();
            await _shippingRe.AddAsync(shipping);
            Order newOrder = _orderRe.GetById(order.Id);
            return new SuccessResult("Add order successfully", newOrder);
        }
        //update order status
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
        //get order by id
        public async Task<ResponseDto> GetById(Guid id)
        {
            Order order = await _orderRe.GetOrder(Order => Order.Id == (id));
            if (order != null)
            {
                return new SuccessDataResult<Order>(order);
            }
            return new ErrorResult();
        }
        //get orders by user id
        public async Task<ResponseDto> GetOrdersByUserId(Guid userId)
        {
            List<Order> orders = _orderRe.GetOrders(order => order.UserId == userId).Result.ToList();
            return new SuccessDataResult<List<Order>>(orders);
        }
        //create orderItems from CartItems
        private List<OrderItem> orderItemsFromCartItems(List<CartItem> cartItems)
        {
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var cartItem in cartItems)
            {
                ProductVariant productItem =  _productItemRe.GetById(cartItem.ProductId);
                OrderItem orderItem = new OrderItem
                {
                    ProductItemId = cartItem.ProductId,
                    Quantity = cartItem.Count,
                    Price = productItem.Price,
                    Subtotal = productItem.Price * cartItem.Count,
                };
                orderItems.Add(orderItem);
            }
            return orderItems;
        }
        //create orderItems from OrderItemDto (calculate discount)
        private async Task<Tuple<List<OrderItem>, decimal>> orderItemsFromOrderItemDto(List<CreateOrderItemDto> orderItemDtos)
        {
            List<OrderItem> orderItems = new List<OrderItem>();
            decimal totalPrice = 0;
            foreach (var item in orderItemDtos)
            {
                ProductVariant productItem = _productItemRe.GetById(item.ProductItemId);
                OrderItem orderItem = new OrderItem
                {
                    Id = new Guid(),
                    ProductItemId = item.ProductItemId,
                    Quantity = item.Quantity,
                    Price = productItem.Price,
                    Subtotal = productItem.Price * item.Quantity,
                };
                if(item.UserCouponId != null)
                {
                    Tuple<bool, decimal> result = await tryApplyCoupon(item.UserCouponId.Value, item.ProductItemId, orderItem.Subtotal);
                    if (result.Item1 == true)
                    {
                        CouponApplication couponApplication = new CouponApplication();
                        couponApplication.Id = new Guid();
                        couponApplication.OrderItemId = orderItem.Id;
                        couponApplication.UserCouponId = item.UserCouponId.Value;
                        couponApplication.DiscountAmount = result.Item2;
                        orderItem.CouponApplication = couponApplication;
                        orderItem.CouponApplicationId = couponApplication.Id;
                        orderItem.Discount = result.Item2;
                        if (orderItem.Subtotal > result.Item2)
                            orderItem.Subtotal -= result.Item2;
                        else orderItem.Subtotal = 0;
                    }
                }
                totalPrice += orderItem.Subtotal;
                orderItems.Add(orderItem);
            }
            return new Tuple<List<OrderItem>,decimal>(orderItems, totalPrice);
        }
        public async Task<Tuple<bool, decimal>> tryApplyCoupon(Guid userCouponId, Guid productItemId, decimal subTotal)
        {
            DateTime currentDate = DateTime.UtcNow;
            UserCoupon userCoupon = _userCouponRe.GetById(userCouponId);
            if (userCoupon == null) return new Tuple<bool, decimal>(false, 0);
            if (userCoupon.RemainingUsage == 0 || !userCoupon.Status.Equals(CouponStatus.ACTIVE) || userCoupon.ProductCoupon.ValidFrom >= currentDate || userCoupon.ProductCoupon.ValidTo < currentDate)
            {
                
               return new Tuple<bool, decimal>(false, 0);
            }
            else
            {
                if (!userCoupon.ProductCoupon.Product.Items.Any(item => item.Id == productItemId) || userCoupon.ProductCoupon.MinimumOrderValue > subTotal)
                    return new Tuple<bool, decimal>(false, 0);
                //TASK: calculate discount value
                decimal value = 0;
                if(userCoupon.ProductCoupon.DiscountUnit == DiscountUnit.PERCENTAGE)
                {
                    decimal calculatedDiscount = (decimal)(userCoupon.ProductCoupon.DiscountValue / 100) * subTotal;
                    value = Math.Min(userCoupon.ProductCoupon.MaximumDiscountValue ?? decimal.MaxValue, calculatedDiscount);
                }
                else
                {
                    value = Math.Min(userCoupon.ProductCoupon.MaximumDiscountValue ?? decimal.MaxValue,
                      (decimal)userCoupon.ProductCoupon.DiscountValue
                      );
                }
                updateUserCoupon(userCoupon);
                return new Tuple<bool, decimal>(true, value);
            }
        }
        //update UserCoupon 
        public void updateUserCoupon(UserCoupon userCoupon)
        {
            if(userCoupon.RemainingUsage > 1)
            {
                userCoupon.RemainingUsage -= 1;
            }
            else
            {
                userCoupon.RemainingUsage = 0;
                userCoupon.Status = CouponStatus.USED;
            }
        }
    }
}
//private async Task<Tuple< List<CouponApplication>,decimal>> applyCouponsToOrder(List<CreateOrderItemDto> orderItemDtos, Guid orderId)
//{
//    List<CouponApplication> couponApplications = [];
//    decimal totalDiscount = 0;
//    foreach (CreateOrderItemDto item in orderItemDtos)
//    {
//        Tuple<bool, decimal> result = await tryApplyCoupon(item.ProductItemId, item.UserCouponId);
//        if (result.Item1 == true)
//        {
//            CouponApplication couponApplication = new CouponApplication();
//            couponApplication.OrderItemId = 
//            couponApplication.UserCouponId = item.UserCouponId;
//            couponApplications.Add(couponApplication);
//            couponApplication.DiscountAmount = result.Item2;
//            totalDiscount += result.Item2;
//        }
//    }
//    return new Tuple<List<CouponApplication>, decimal>(couponApplications, totalDiscount);
//}
//private List<CouponApplication> applyCouponsToOrder(List<CartItem> cartItems, Guid orderId)
//{
//    List<CouponApplication> couponApplications = [];
//    foreach (CartItem item in cartItems)
//    {
//        CouponApplication couponApplication = new CouponApplication();
//        couponApplication.OrderId = orderId;
//        couponApplication.UserCouponId = item.UserCouponId;
//        couponApplications.Add(couponApplication);
//    }
//    return couponApplications;
//}
//try apply userCoupon to a orderItem
