using JewelryEC_Backend.Core.Repository.EntityFramework;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Models.Products;
using JewelryEC_Backend.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Linq.Expressions;

namespace JewelryEC_Backend.Repository
{
    public class ProductRepository : JewelryEC_Backend.Core.Repository.EntityFramework.GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext _context) : base(_context)
        {
        }

        public async Task<Product> GetProduct(Expression<Func<Product, bool>> filter)
        {
            return await _context.Products.FirstOrDefaultAsync(filter);
        }

        public async  Task<List<Product>> GetProducts( int pageNumber, int pageSize, Expression<Func<Product, bool>> filter = null)
        {

            try
            {
                var products = filter != null ? await _context.Products.Where(filter)
               .Skip((pageNumber - 1) * pageSize) // Skip items on previous pages
               .Take(pageSize) // Take items for the current page
                   .ToListAsync() : await _context.Products
               .Skip((pageNumber - 1) * pageSize) // Skip items on previous pages
               .Take(pageSize) // Take items for the current page
                   .ToListAsync();
                return products;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }

}
