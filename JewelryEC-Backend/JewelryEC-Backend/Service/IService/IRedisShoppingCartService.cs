using Microsoft.Extensions.Caching.Distributed;

namespace JewelryEC_Backend.Service.IService
{
    public interface IRedisShoppingCartService
    {
        Dictionary<Guid, int> GetData(Guid userId);
        void SetData(Guid userId, Guid productId, int quantity);
        bool RemoveCartHeader(Guid userId);
        void RemoveProductFromCart(Guid userId, Guid productId);
        void SetCartTTL(Guid userId, TimeSpan expiry);
    }
}
