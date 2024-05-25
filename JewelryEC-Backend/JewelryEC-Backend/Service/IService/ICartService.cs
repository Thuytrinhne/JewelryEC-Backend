using JewelryEC_Backend.Models.CartItems.Entities;
using JewelryEC_Backend.Models.Carts.Entities;
using JewelryEC_Backend.Models.Catalogs.Entities;
using JewelryEC_Backend.Models.Products;

namespace JewelryEC_Backend.Service.IService
{
    public interface ICartService
    {
        bool CreateCart(Cart catalogToCreate);
        Cart GetDetailCart(Guid userId);
        void SetStatusForCart(int status, Guid cartId);
        CartItem CartUpSert(Guid userId, CartItem cartItem);
        bool DeleteCartItem(Guid userId, Guid productId);
        ProductVariant GetCartItemDetail(Guid productId);
        string GetCartItemName(Guid productId);
        void HanldeCartAfterCheckout (Guid userId);

    }
}
