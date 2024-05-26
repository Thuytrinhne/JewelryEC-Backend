using JewelryEC_Backend.Core.Entity;
using JewelryEC_Backend.Enum;
using JewelryEC_Backend.Models.Auths.Entities;
using JewelryEC_Backend.Models.Coupon;
using JewelryEC_Backend.Models.Products;
using System.Runtime.InteropServices;

namespace JewelryEC_Backend.Models.Voucher
{
    public class UserCoupon: IEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductCouponId { get; set; }
        public int RemainingUsage { get; set; }
        public CouponStatus Status { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ProductCoupon ProductCoupon { get; set; }
        public virtual List<CouponApplication> CouponApplications { get; set; }
    }
}
