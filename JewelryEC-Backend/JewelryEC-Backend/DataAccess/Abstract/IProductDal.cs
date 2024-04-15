using JewelryEC_Backend.Core.DataAccess;
using JewelryEC_Backend.Models.Categories;
using JewelryEC_Backend.Models.Products;
using System.Linq.Expressions;

namespace JewelryEC_Backend.DataAccess.Abstract
{
    public interface IProductDal: IEntityRepository<Product>
    {
        Task<List<Product>> GetProducts(Expression<Func<Product, bool>> filter = null);
        Task<Product> GetProduct(Expression<Func<Product, bool>> filter);
    }
}
