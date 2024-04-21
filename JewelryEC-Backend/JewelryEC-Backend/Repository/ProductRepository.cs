using JewelryEC_Backend.Core.Repository.EntityFramework;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Models.Categories;
using JewelryEC_Backend.Models.Products;
using JewelryEC_Backend.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JewelryEC_Backend.Repository
{
    public class ProductRepository : JewelryEC_Backend.Core.Repository.EntityFramework.GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext _context) : base(_context)
        {
        }

        public Task<Product> GetProduct(Expression<Func<Product, bool>> filter)
        {
            return _context.Products.FirstOrDefaultAsync(filter);
        }

        public Task<List<Product>> GetProducts(Expression<Func<Product, bool>> filter = null)
        {
            return filter == null ? _context.Products.ToListAsync() : _context.Products.Where(filter).ToListAsync();
        }

    }

}
