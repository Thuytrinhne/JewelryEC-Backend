using JewelryEC_Backend.Core.Repository.EntityFramework;
using JewelryEC_Backend.Data;
using JewelryEC_Backend.Models.CartItems.Entities;
using JewelryEC_Backend.Repository.IRepository;

namespace JewelryEC_Backend.Repository
{
    public class CartItemRepository : GenericRepository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(AppDbContext context) : base(context)
        {
        }

        public CartItem FindCartItem(Guid cardId, Guid productId)
        {
            return Find(u => u.ProductId == productId &&
                     u.CartId == cardId).FirstOrDefault();
        }

        public CartItem GetCartItem(Guid productId, Guid cartId)
        {
            return Find(u => u.ProductId == productId &&
                      u.CartId == cartId).FirstOrDefault();
        }

        public IEnumerable<CartItem> GetCartItems(Guid cardId)
        {
            return Find(c => c.CartId == cardId).ToList();
        }
    }
}
