using JewelryEC_Backend.Helpers.Payments.VnPay;
using JewelryEC_Backend.Models.Orders.Dto;
using JewelryEC_Backend.Service.IService;
using Microsoft.AspNetCore.Mvc;
using static JewelryEC_Backend.Utility.SD;


namespace JewelryEC_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderApiController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IVnPayService _vnPayService;

        public OrderApiController(IOrderService orderService, IVnPayService vnPayService)
        {
            _orderService = orderService;
            _vnPayService= vnPayService;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderService.GetAll();
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

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] CreateNewOrderDto orderDto, string payment = "COD")
        {
            var result = await _orderService.Add(orderDto);
     
            if (result.IsSuccess)
            {
                // PAYMENT VNPAY
                    // if payment method = vnpay
                    // 1. update order status "Chờ thành toán"
                    if (payment == PaymentMethod.VNPAY.ToString())
                    {
                        var vnPaymentModel = new VnPaymentRequestModel
                        {
                            Amount = 1000,
                            CreatedDate = DateTime.Now,
                            Description = "",
                            FullName = "Trinh",
                            OrderId = new Guid()
                        };
                        // return payment url 
                        return Ok(_vnPayService.CreatePaymentUrl(HttpContext, vnPaymentModel));
                    }
                //END PAYMENT VNPAY 
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("cancel/{orderId}")]
        public async Task<IActionResult> Cancel([FromRoute] Guid orderId)
        {
            var result = await _orderService.Cancel(orderId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        [HttpPost("PaymentCallBack")]
        public async  Task <IActionResult> PaymentCallBack ()
        {
            var response =  _vnPayService.PaymentExecute(Request.Query);
            if(response == null || response.VnPayResponseCode != "00")
            {
                // thông báo lỗi thanh toán k thành công
                return BadRequest(response.VnPayResponseCode);
            }
            // 2. thay đổi trạng thái đơn hàng thành Đã thanh toán 
           return Ok(response);
        }
    }   
        [HttpPost("getbyuser/{ùserId}")]
        public async Task<IActionResult> GetByUserId([FromRoute] Guid userId)
        {
            var result = await _orderService.GetOrdersByUserId(userId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

    }

}
