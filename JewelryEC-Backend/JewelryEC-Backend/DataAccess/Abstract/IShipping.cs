using JewelryEC_Backend.Core.DataAccess;
using JewelryEC_Backend.Models.Categories;
using JewelryEC_Backend.Models.Shippings;
using System.Linq.Expressions;

namespace JewelryEC_Backend.DataAccess.Abstract
{
    public interface IShippingDal: IEntityRepository<Shipping>
    {
        Task<Shipping> GetShipping(Expression<Func<Shipping, bool>> filter);
    }
}
