using JewelryEC_Backend.Core.DataAccess.EntityFramework;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.DataAccess.Abstract;
using JewelryEC_Backend.Models.Categories;
using JewelryEC_Backend.Models.Products;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JewelryEC_Backend.DataAccess.Concrete
{
    public class EfProductDal : EfEntityRepositoryBase<Product>, IProductDal
    {
        public EfProductDal(AppDbContext context) : base(context)
        {
        }

        public Task<Product> GetProduct(Expression<Func<Product, bool>> filter)
        {
            return context.Products.FirstOrDefaultAsync(filter);
        }

        public Task<List<Product>> GetProducts(Expression<Func<Product, bool>> filter = null)
        {
            return filter == null ? context.Products.ToListAsync() : context.Products.Where(filter).ToListAsync();
        }

    }
    
}
