using JewelryEC_Backend.Core.DataAccess;
using JewelryEC_Backend.Models.Categories;
using JewelryEC_Backend.Models.Orders;
using System.Linq.Expressions;

namespace JewelryEC_Backend.DataAccess.Abstract
{
    public interface IOrderDal: IEntityRepository<Order>
    {
        Task<List<Order>> GetOrders(Expression<Func<Order, bool>> filter = null);
        Task<Order> GetOrder(Expression<Func<Order, bool>> filter);
    }
}
