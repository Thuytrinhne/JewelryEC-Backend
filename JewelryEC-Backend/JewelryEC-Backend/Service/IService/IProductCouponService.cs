using JewelryEC_Backend.Models;
using JewelryEC_Backend.Models.Coupon.Dto;

namespace JewelryEC_Backend.Service.IService
{
    public interface IProductCouponService
    {
        Task<ResponseDto> GetAll();
        Task<ResponseDto> GetById(Guid id);
        Task<ResponseDto> Add(CreateProductCouponDto couponDto);
        Task<ResponseDto> Update(UpdateProductCouponDto couponDto);
    }
}
