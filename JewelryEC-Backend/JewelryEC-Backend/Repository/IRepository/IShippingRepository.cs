using JewelryEC_Backend.Core.Repository;
using JewelryEC_Backend.Models.Categories;
using JewelryEC_Backend.Models.Shippings;
using System.Linq.Expressions;

namespace JewelryEC_Backend.Repository.IRepository
{
    public interface IShippingRepository : JewelryEC_Backend.Core.Repository.IGenericRepository<Shipping>
    {
        Task<Shipping> GetShipping(Expression<Func<Shipping, bool>> filter);
    }
}
