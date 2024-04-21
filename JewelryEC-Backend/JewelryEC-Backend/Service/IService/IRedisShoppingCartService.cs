using Microsoft.Extensions.Caching.Distributed;

namespace JewelryEC_Backend.Service.IService
{
    public interface IRedisShoppingCartService
    {
        Dictionary<Guid, int> GetData(Guid userId);
        void SetData(Guid userId, Guid productId, int quantity);
        object RemoveData (Guid userId);
        void RemoveProductFromCart(Guid userId, Guid productId);
    }
}
