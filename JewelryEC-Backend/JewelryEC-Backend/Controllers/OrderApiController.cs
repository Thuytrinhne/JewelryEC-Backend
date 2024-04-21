using AutoMapper;
using JewelryEC_Backend.Models.Products.Dto;
using Microsoft.AspNetCore.Mvc;
using JewelryEC_Backend.Service.IService;
using JewelryEC_Backend.Models.Orders.Dto;


namespace JewelryEC_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderApiController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderApiController(IOrderService orderService)
        {
            _orderService = orderService;
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
        public async Task<IActionResult> Add([FromBody] CreateNewOrderDto orderDto)
        {
            var result = await _orderService.Add(orderDto);
            if (result.IsSuccess)
            {
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
