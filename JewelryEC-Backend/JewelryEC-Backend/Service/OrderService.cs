using AutoMapper;
using JewelryEC_Backend.Core.Utilities.Results;
using JewelryEC_Backend.Mapper;
using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.Orders;
using JewelryEC_Backend.Models.Orders.Dto;
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
        private readonly IShippingRepository _shippingRe;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _orderRe = unitOfWork.Orders;
            _shippingRe = unitOfWork.Shippings;
        }
        public async Task<ResponseDto> GetAll()
        {
            return new SuccessDataResult<List<Order>>(await _orderRe.GetAllAsync());
        }
        public async Task<ResponseDto> Add(CreateNewOrderDto orderDto)
        {
            Shipping shipping = ShippingMapper.ShippingFromCreateNewOrderDto(orderDto);
            Order order = OrderMapper.OrderFromCreateOrderDto(orderDto);
            shipping.OrderId = order.Id;
            Console.WriteLine(order.ToJson());
            await _orderRe.AddAsync(order);
            await _orderRe.SaveChangeAsync();
            await _shippingRe.AddAsync(shipping);
            return new SuccessResult("Add order successfully");
        }
        public async Task<ResponseDto> Cancel(Guid orderId)
        {
            Order order = await _orderRe.GetOrder(Order => Order.Id == (orderId));
            if(order != null)
            {
                await _orderRe.Delete(order);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
        public async Task<ResponseDto> GetById(Guid id)
        {
            Order order = await _orderRe.GetOrder(Order => Order.Id == (id));
            if (order != null)
            {
                await _orderRe.Delete(order);
                return new SuccessResult();
            }
            return new ErrorResult();
        }
    }
}
