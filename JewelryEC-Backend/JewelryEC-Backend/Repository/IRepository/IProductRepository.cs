using JewelryEC_Backend.Core.Repository;
using JewelryEC_Backend.Models.Products;
using System.Linq.Expressions;

namespace JewelryEC_Backend.Repository.IRepository
{
    public interface IProductRepository : JewelryEC_Backend.Core.Repository.IGenericRepository<Product>
    {
        Task<List<Product>> GetProducts( int pageNumber, int pageSize, Expression<Func<Product, bool>> filter = null);
        Task<Product> GetProduct(Expression<Func<Product, bool>> filter);
    }
}
