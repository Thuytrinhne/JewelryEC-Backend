using AutoMapper;
using JewelryEC_Backend.Models.Products.Dto;
using Microsoft.AspNetCore.Mvc;
using JewelryEC_Backend.Service.IService;
using JewelryEC_Backend.Models.Coupon.Dto;
using JewelryEC_Backend.Enum;
using System.Security.Claims;


namespace JewelryEC_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCouponApiController : ControllerBase
    {
        private readonly IUserCouponService _userCouponService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserCouponApiController(IUserCouponService service, IHttpContextAccessor httpContextAccessor)
        {
            _userCouponService = service;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetByCouponStatus([FromQuery] CouponStatus status)
        {
            String userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //TASK: Don't hard code userId
            var result = await _userCouponService.FindCouponByStatusAndUserId(status, new Guid(userId));
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("receiveCoupon")]
        public async Task<IActionResult> GetById([FromQuery] String? code,[FromQuery] Guid? couponId)
        {
            var result = await _userCouponService.ReceiveCoupon(code, couponId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        [HttpGet("tryApply")]
        public async Task<IActionResult> TryApplyCoupon([FromQuery]Guid userCouponId, [FromQuery] Guid productItemId)
        {
            var result = await _userCouponService.tryApplyCoupon(userCouponId, productItemId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

    }

}
