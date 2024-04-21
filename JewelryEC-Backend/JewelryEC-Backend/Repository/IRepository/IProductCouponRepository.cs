using JewelryEC_Backend.Core.Repository;
using JewelryEC_Backend.Models.Coupon;
using System.Linq.Expressions;

namespace JewelryEC_Backend.Repository.IRepository
{
    public interface IProductCouponRepository: JewelryEC_Backend.Core.Repository.IGenericRepository<ProductCoupon>
    {
    }
}
