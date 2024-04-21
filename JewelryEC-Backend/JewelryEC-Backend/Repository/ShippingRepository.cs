using JewelryEC_Backend.Core.Repository.EntityFramework;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Models.Categories;
using JewelryEC_Backend.Models.Shippings;
using JewelryEC_Backend.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JewelryEC_Backend.Repository
{
    public class ShippingRepository : JewelryEC_Backend.Core.Repository.EntityFramework.GenericRepository<Shipping>, IShippingRepository
    {
        public ShippingRepository(AppDbContext _context) : base(_context)
        {
        }

        public Task<Shipping> GetShipping(Expression<Func<Shipping, bool>> filter)
        {
            return _context.Shippings.FirstOrDefaultAsync(filter);
        }

        public Task<List<Shipping>> GetShippings(Expression<Func<Shipping, bool>> filter = null)
        {
            return filter == null ? _context.Shippings.ToListAsync() : _context.Shippings.Where(filter).ToListAsync();
        }

    }

}
