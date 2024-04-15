using AutoMapper;
using JewelryEC_Backend.Core.Utilities.Results;
using JewelryEC_Backend.DataAccess.Abstract;
using JewelryEC_Backend.Mapper;
using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.Orders;
using JewelryEC_Backend.Models.Orders.Dto;
using JewelryEC_Backend.Models.Shippings;
using JewelryEC_Backend.Service.IService;
using NuGet.Protocol;

namespace JewelryEC_Backend.Service
{
    public class OrderService: IOrderService
    {
        private IOrderDal _orderDal;
        private IShippingDal _shippingDal;

        public OrderService(IOrderDal orderDal, IShippingDal shippingDal)
        {
            _orderDal = orderDal;
            _shippingDal = shippingDal;
        }
        public async Task<ResponseDto> GetAll()
        {
            return new SuccessDataResult<List<Order>>(await _orderDal.GetAllAsync());
        }
        public async Task<ResponseDto> Add(CreateNewOrderDto orderDto)
        {
            Shipping shipping = ShippingMapper.ShippingFromCreateNewOrderDto(orderDto);
            Order order = OrderMapper.OrderFromCreateOrderDto(orderDto);
            shipping.OrderId = order.Id;
            Console.WriteLine(order.ToJson());
            await _orderDal.AddAsync(order);
            await _orderDal.SaveChangeAsync();
            await _shippingDal.AddAsync(shipping);
            return new SuccessResult("Add order successfully");
        }
        public async Task<ResponseDto> Cancel(Guid orderId)
        {
            Order order = await _orderDal.GetOrder(Order => Order.Id == (orderId));
            if(order != null)
            {
                await _orderDal.Delete(order);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
        public async Task<ResponseDto> GetById(Guid id)
        {
            Order order = await _orderDal.GetOrder(Order => Order.Id == (id));
            if (order != null)
            {
                await _orderDal.Delete(order);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
