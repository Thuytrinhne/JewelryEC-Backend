
using AutoMapper;
using JewelryEC_Backend.Core.Utilities.Results;
using JewelryEC_Backend.Models.Coupon;
using JewelryEC_Backend.Models;
using JewelryEC_Backend.Repository.IRepository;
using JewelryEC_Backend.Service.IService;
using JewelryEC_Backend.UnitOfWork;
using JewelryEC_Backend.Enum;
using JewelryEC_Backend.Models.Voucher;
using JewelryEC_Backend.Models.Orders;
using System.Diagnostics;
using JewelryEC_Backend.Models.Products;
using System.Drawing.Printing;

namespace JewelryEC_Backend.Service
{
    public class UserCouponService : IUserCouponService
    {
        private IUserCouponRepository _userCouponRe;
        private IProductCouponRepository _productCouponRe;
        private IUserRespository _userRe;
        private IMapper _mapper;

        public UserCouponService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userCouponRe = unitOfWork.UserCoupon;
            _productCouponRe = unitOfWork.ProductCoupons;
            _userRe = unitOfWork.Users;
            _mapper = mapper;
        }

        public async Task<ResponseDto> FindCouponByStatusAndUserId(CouponStatus couponStatus, Guid userId)
        {
            List<UserCoupon> userCoupons = _userCouponRe.GetAll(p => p.Status == couponStatus && p.UserId == userId);
            return new SuccessDataResult<List<UserCoupon>>(userCoupons);
        }
        //save coupon to user coupon list
        public async Task<ResponseDto> ReceiveCoupon(string? code, Guid? couponProductId)
        {
            DateTime currentDate = DateTime.UtcNow;
            ProductCoupon productCoupon;
            if(code != null)
            {
                productCoupon = _productCouponRe.Get(p => p.CouponCode == code);
            }
            else
            {
                productCoupon = _productCouponRe.Get(p => p.Id == couponProductId && p.ValidTo >= currentDate && p.ValidFrom <= currentDate);
            } 
            if(productCoupon == null) {
                return new ErrorResult("Can not find product coupon");
            }
            else
            {
                UserCoupon existingCoupon = _userCouponRe.Get(e => e.ProductCouponId == productCoupon.Id);
                if(existingCoupon != null) return new SuccessResult("Existing coupon",  existingCoupon);
                UserCoupon userCoupon = new UserCoupon();
                userCoupon.Id = new Guid();
                userCoupon.ProductCoupon = productCoupon;
                userCoupon.User = new Models.Auths.Entities.ApplicationUser();
                userCoupon.RemainingUsage = productCoupon.UsedTime;
                userCoupon.Status = CouponStatus.ACTIVE;
                await _userCouponRe.AddAsync(userCoupon);
                return new SuccessResult("Receive coupon successfully", _userCouponRe.GetById(userCoupon.Id));
            }
        }
        //check if an coupon can apply to an product item
        public async Task<ResponseDto> tryApplyCoupon(Guid userCouponId, Guid productItemId)
        {
            DateTime currentDate = DateTime.UtcNow;
            UserCoupon userCoupon = _userCouponRe.GetById(userCouponId);
            if (userCoupon == null) return new ErrorResult("User coupon is not exists");
            if (userCoupon.RemainingUsage == 0 || !userCoupon.Status.Equals(CouponStatus.ACTIVE) || userCoupon.ProductCoupon.ValidFrom >= currentDate || userCoupon.ProductCoupon.ValidTo < currentDate)
            {
                return new ErrorResult("Coupon inactive");
            }
            else
            {
                if (!userCoupon.ProductCoupon.Product.Items.Any(item => item.Id == productItemId))
                    return new ErrorResult("This coupon with id " + userCouponId + " can't not apply to this " + productItemId);
                else return new SuccessResult("Coupon active to apply", userCoupon);
            }
        }
    }
}

