using JewelryEC_Backend.Core.Repository.EntityFramework;
using JewelryEC_Backend.Data;
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

        public async  Task<List<Product>> GetProducts(Expression<Func<Product, bool>> filter = null)
        {
           
                // dun need include here -> config "auto include" items for product in appdbcontext
                return await _context.Products.ToListAsync(); 

           
            //return filter == null ?
            //    _context.Products
            //        .Include(p => p.Items)  
            //        .Include(p => p.Images) 
            //        .ToListAsync() :
            //    _context.Products
            //        .Where(filter)
            //        .Include(p => p.Items)
            //        .Include(p => p.Images)
            //        .ToListAsync();
        }
    }

}
