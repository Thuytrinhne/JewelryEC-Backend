using JewelryEC_Backend.Core.Repository;
using JewelryEC_Backend.Models.CartItems.Entities;

namespace JewelryEC_Backend.Repository.IRepository
{
    public interface ICartItemRepository  : IGenericRepository<CartItem>
    {
        IEnumerable<CartItem> GetCartItems(Guid cardId);
        CartItem GetCartItem(Guid productId, Guid cartId);
        CartItem FindCartItem(Guid cardId, Guid productId);
    }
}
