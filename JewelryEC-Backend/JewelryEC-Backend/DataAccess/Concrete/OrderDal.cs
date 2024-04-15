using JewelryEC_Backend.Core.DataAccess.EntityFramework;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.DataAccess.Abstract;
using JewelryEC_Backend.Models.Categories;
using JewelryEC_Backend.Models.Orders;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JewelryEC_Backend.DataAccess.Concrete
{
    public class EfOrderDal : EfEntityRepositoryBase<Order>, IOrderDal
    {
        public EfOrderDal(AppDbContext context) : base(context)
        {
        }

        public Task<Order> GetOrder(Expression<Func<Order, bool>> filter)
        {
            return context.Orders.FirstOrDefaultAsync(filter);
        }

        public Task<List<Order>> GetOrders(Expression<Func<Order, bool>> filter = null)
        {
            return filter == null ? context.Orders.ToListAsync() : context.Orders.Where(filter).ToListAsync();
        }

    }
    
}
