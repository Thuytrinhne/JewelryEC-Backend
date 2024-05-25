using JewelryEC_Backend.Models.Coupon.Dto;
using JewelryEC_Backend.Models;
using JewelryEC_Backend.Enum;

namespace JewelryEC_Backend.Service.IService
{
    public interface IUserCouponService
    {
        Task<ResponseDto> ReceiveCoupon(String? code, Guid? couponProductId);
        Task<ResponseDto> FindCouponByStatusAndUserId(CouponStatus couponStatus, Guid userId);
        Task<ResponseDto> tryApplyCoupon(Guid couponId, Guid productItemId);

    }
}
