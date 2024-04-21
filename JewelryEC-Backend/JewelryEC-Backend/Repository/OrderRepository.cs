using JewelryEC_Backend.Core.Repository.EntityFramework;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Models.Categories;
using JewelryEC_Backend.Models.Orders;
using JewelryEC_Backend.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
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

        public Task<List<Order>> GetOrders(Expression<Func<Order, bool>> filter = null)
        {
            return filter == null ? _context.Orders.ToListAsync() : _context.Orders.Where(filter).ToListAsync();
        }

    }

}
