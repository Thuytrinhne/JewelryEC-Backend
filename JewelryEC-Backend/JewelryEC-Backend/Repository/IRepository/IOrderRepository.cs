using JewelryEC_Backend.Core.Repository;
using JewelryEC_Backend.Models.Orders;
using JewelryEC_Backend.Models.Products;
using System.Linq.Expressions;

namespace JewelryEC_Backend.Repository.IRepository
{
    public interface IOrderRepository : JewelryEC_Backend.Core.Repository.IGenericRepository<Order>
    {
        Task<List<Order>> GetOrders(int pageNumber, int pageSize,Expression < Func<Order, bool>> filter = null);
        Task<Order> GetOrder(Expression<Func<Order, bool>> filter);
    }
}
