using JewelryEC_Backend.Core.Repository.EntityFramework;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Models.Catalogs.Entities;
using JewelryEC_Backend.Models.Products;
using JewelryEC_Backend.Repository.IRepository;

namespace JewelryEC_Backend.Repository
{
    public class ProductItemRespository : GenericRepository<ProductItem>, IProductItemRespository
    {
        public ProductItemRespository(AppDbContext context) : base(context)
        {
        }
        public ProductItem GetInforOfProductItem(Guid ProductItemId)
        {
            return _context.ProductItems
                  .Where(x => x.Id == ProductItemId)
                  .Select(x => new ProductItem
                  {
                      Name = x.Name,
                      DiscountPrice = x.DiscountPrice,
                  })
                  .FirstOrDefault();
        }
    }
}
