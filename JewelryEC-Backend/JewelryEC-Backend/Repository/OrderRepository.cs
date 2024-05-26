using JewelryEC_Backend.Core.Repository.EntityFramework;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Models.Orders;
using JewelryEC_Backend.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace JewelryEC_Backend.Repository
{
    public class OrderRepository : JewelryEC_Backend.Core.Repository.EntityFramework.GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext _context) : base(_context)
        {
        }

        public Task<Order> GetOrder(Expression<Func<Order, bool>> filter)
        {
            return _context.Orders.FirstOrDefaultAsync(filter);
        }

        public async Task<List<Order>> GetOrders(int pageNumber, int pageSize ,Expression<Func<Order, bool>> filter = null)
        {
            try
            {
                var orders = filter != null ? await _context.Orders.Where(filter)
               .Skip((pageNumber - 1) * pageSize) // Skip items on previous pages
               .Take(pageSize) // Take items for the current page
                   .ToListAsync() : await _context.Orders
               .Skip((pageNumber - 1) * pageSize) // Skip items on previous pages
               .Take(pageSize) // Take items for the current page
                   .ToListAsync();
                return orders;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }

}
