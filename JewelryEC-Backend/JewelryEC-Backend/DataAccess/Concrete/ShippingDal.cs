using JewelryEC_Backend.Core.DataAccess.EntityFramework;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.DataAccess.Abstract;
using JewelryEC_Backend.Models.Categories;
using JewelryEC_Backend.Models.Shippings;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JewelryEC_Backend.DataAccess.Concrete
{
    public class EfShippingDal : EfEntityRepositoryBase<Shipping>, IShippingDal
    {
        public EfShippingDal(AppDbContext context) : base(context)
        {
        }

        public Task<Shipping> GetShipping(Expression<Func<Shipping, bool>> filter)
        {
            return context.Shippings.FirstOrDefaultAsync(filter);
        }

        public Task<List<Shipping>> GetShippings(Expression<Func<Shipping, bool>> filter = null)
        {
            return filter == null ? context.Shippings.ToListAsync() : context.Shippings.Where(filter).ToListAsync();
        }

    }
    
}
