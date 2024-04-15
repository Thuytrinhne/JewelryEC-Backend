using Microsoft.Extensions.Caching.Distributed;

namespace JewelryEC_Backend.Service.IService
{
    public interface IRedisShoppingCartService
    {
        Dictionary<Guid, int> GetData(string userId);
        void SetData(string userId, Guid productId, int quantity);
        object RemoveData (string userId);
    }
}
