using JewelryEC_Backend.Enum;
using JewelryEC_Backend.Helpers.Payments.VnPay;
using JewelryEC_Backend.Models.Orders;
using JewelryEC_Backend.Models.Orders.Dto;
using JewelryEC_Backend.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using StackExchange.Redis;
using System.ComponentModel;
using System.Security.Claims;
using static JewelryEC_Backend.Utility.SD;
using Order = JewelryEC_Backend.Models.Orders.Order;


namespace JewelryEC_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderApiController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IVnPayService _vnPayService;
        private readonly ICartService _cartService;

        public OrderApiController(IOrderService orderService, IVnPayService vnPayService, ICartService cartService)
        {
            _orderService = orderService;
            _vnPayService= vnPayService;
            _cartService = cartService;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _orderService.GetAll(pageNumber, pageSize);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _orderService.GetById(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        //[HttpPost("add")]
        //[Authorize]
        //public async Task<IActionResult> Add([FromBody] CreateNewOrderDto orderDto)
        //{

        //    var result = await _orderService.Add(orderDto);
            
        //    if (result.IsSuccess)
        //    {
        //        Order newOrder = (Order)result.Result;
        //        #region if payment method = vnpay
        //        if (orderDto.PaymentMethod == PaymentMethod.VNPAY)
        //        {
        //            var vnPaymentModel = new VnPaymentRequestModel
        //                {
        //                    Amount = newOrder.TotalPrice,
        //                    CreatedDate = newOrder.CreateDate,
        //                    Description = "",
        //                    FullName = "Trinh",
        //                    OrderId = newOrder.Id,
        //                };
        //                // return payment url 
        //            return Ok(_vnPayService.CreatePaymentUrl(HttpContext, vnPaymentModel));
        //        }
        //        #endregion
        //        #region handle cart after checkout
        //        //_cartService.HanldeCartAfterCheckout(newOrder.UserId);
        //        #endregion
        //        return Ok(result);
        //    }

        //    return BadRequest(result);
        //}
        [HttpPost("addfromcart")]
        [Authorize]
        public async Task<IActionResult> AddFromCart([FromBody] CreateNewOrderFromCartDto orderDto)
        {
            var result = await _orderService.AddFromCart(orderDto);

            if (result.IsSuccess)
            {
                Order newOrder = (Order)result.Result;
                #region if payment method = vnpay
                if (orderDto.PaymentMethod == PaymentMethod.VNPAY)
                {
                    var vnPaymentModel = new VnPaymentRequestModel
                    {
                        Amount = newOrder.TotalPrice,
                        CreatedDate = newOrder.CreateDate,
                        Description = "",
                        FullName = "Trinh",
                        OrderId = newOrder.Id,
                        
                    };
                    // return payment url 
                    return Ok(_vnPayService.CreatePaymentUrl(HttpContext, vnPaymentModel));
                }
                #endregion
                #region handle cart after checkout
                //_cartService.HanldeCartAfterCheckout(newOrder.UserId);
                #endregion
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPatch("cancel/{orderId}")]
        public async Task<IActionResult> Cancel([FromRoute] Guid orderId)
        {
            var result = await _orderService.UpdateOrderStatus(orderId, Enum.OrderStatus.Cancelled);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        [HttpGet("paymentcallback")]
        public async  Task <IActionResult> PaymentCallBack ([FromQuery(Name = "vnp_OrderInfo")] string vnpOrderInfo)
        {
            try
            {
                var response = _vnPayService.PaymentExecute(Request.Query);
                if (response == null || response.VnPayResponseCode != "00")
                {
                    // thông báo lỗi thanh toán k thành công
                    return BadRequest("THANH TOÁN KHÔNG THÀNH CÔNG, VUI LÒNG THỬ LẠI");
                }
                string orderIdStr = vnpOrderInfo.Split(':').LastOrDefault();
                Guid orderId = new Guid(orderIdStr);
                // 2. thay đổi trạng thái đơn hàng thành Đã thanh toán (Processing)
                var result = await _orderService.UpdateOrderStatus(orderId, Enum.OrderStatus.Processing);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        [HttpGet("getbyuser/{userId}")]
        public async Task<IActionResult> GetByUserId([FromRoute] Guid userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _orderService.GetOrdersByUserId(userId,pageNumber, pageSize );
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }   
       

    

}
