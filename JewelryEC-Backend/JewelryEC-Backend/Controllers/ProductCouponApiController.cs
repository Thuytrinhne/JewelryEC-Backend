using AutoMapper;
using JewelryEC_Backend.Models.Products.Dto;
using Microsoft.AspNetCore.Mvc;
using JewelryEC_Backend.Service.IService;
using JewelryEC_Backend.Models.Coupon.Dto;
using Microsoft.AspNetCore.Authorization;


namespace JewelryEC_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductCouponApiController : ControllerBase
    {
        private readonly IProductCouponService _productCouponService;

        public ProductCouponApiController(IProductCouponService service)
        {
            _productCouponService = service;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productCouponService.GetAll();
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _productCouponService.GetById(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("add")]
        [Authorize(Roles = "ADMIN")]

        public async Task<IActionResult> Add([FromBody] CreateProductCouponDto dto)
        {
            var result = await _productCouponService.Add(dto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPut("update")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Update(
            [FromBody] UpdateProductCouponDto updateProductCoupon)
        {
            var result = await _productCouponService.Update(updateProductCoupon);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpDelete("delete/{couponId}")]
        [Authorize(Roles = "ADMIN")]

        public async Task<IActionResult> Delete(
        [FromRoute] Guid couponId)
            {
                var result = await _productCouponService.Delete(couponId);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
    }

}
