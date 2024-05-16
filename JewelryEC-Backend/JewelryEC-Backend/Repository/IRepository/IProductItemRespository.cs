using JewelryEC_Backend.Core.Repository;
using JewelryEC_Backend.Models.CartItems.Entities;
using JewelryEC_Backend.Models.Products;

namespace JewelryEC_Backend.Repository.IRepository
{
    public interface IProductItemRespository : IGenericRepository<ProductVariant>
    {
        ProductVariant GetInforOfProductItem(int ProductItemId);
    }
}
