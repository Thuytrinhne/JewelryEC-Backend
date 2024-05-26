using JewelryEC_Backend.Core.Repository;
using JewelryEC_Backend.Models.Coupon;
using JewelryEC_Backend.Models.Voucher;
using System.Linq.Expressions;

namespace JewelryEC_Backend.Repository.IRepository
{
    public interface IUserCouponRepository: JewelryEC_Backend.Core.Repository.IGenericRepository<UserCoupon>
    {
    }
}
