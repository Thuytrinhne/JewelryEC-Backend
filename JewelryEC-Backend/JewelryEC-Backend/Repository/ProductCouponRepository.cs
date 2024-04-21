using JewelryEC_Backend.Core.Repository.EntityFramework;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Models.Categories;
using JewelryEC_Backend.Models.Coupon;
using JewelryEC_Backend.Models.Products;
using JewelryEC_Backend.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JewelryEC_Backend.Repository
{
    public class ProductCouponRepository : JewelryEC_Backend.Core.Repository.EntityFramework.GenericRepository<ProductCoupon>, IProductCouponRepository
    {
        public ProductCouponRepository(AppDbContext _context) : base(_context)
        {
        }

    }

}
