using JewelryEC_Backend.Service.IService;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System.Text.Json;


namespace JewelryEC_Backend.Service
{
    public class RedisShoppingCartService : IRedisShoppingCartService
    {
        private IDatabase _cacheDb;
        protected readonly IConfiguration _configuration;
        public RedisShoppingCartService(IConfiguration configuration)
        {
            _configuration = configuration;
            connectRedis();

        }

        private void connectRedis()
        {
            var redis = ConnectionMultiplexer.Connect(_configuration.GetConnectionString("MyRedisConStr").ToString());
            _cacheDb = redis.GetDatabase();
        }

        public Dictionary<Guid, int> GetData(Guid userId)
        {
       
            var cartKey = $"cart:{userId}_ref";

            // Lấy tất cả các mặt hàng từ hashset và chuyển đổi chúng thành dictionary
            var cartItems = _cacheDb.HashGetAll(cartKey)
                             .ToDictionary(
                                 x => Guid.Parse(x.Name.ToString()),
                                 x => (int)x.Value);

            return cartItems;
        }

       

        public void SetData (Guid userId, Guid productId,int quantity)
        {
        
            var cartKey = $"cart:{userId}_ref";

            // Kiểm tra xem mặt hàng đã tồn tại trong giỏ hàng chưa
            if (_cacheDb.HashExists(cartKey, productId.ToString()))
            {
                // Nếu mặt hàng đã tồn tại, tăng số lượng lên
                var existingQuantity = (int)_cacheDb.HashGet(cartKey, productId.ToString());
                _cacheDb.HashSet(cartKey, productId.ToString(), existingQuantity + quantity);
            }
            else
            {
                // Nếu mặt hàng chưa tồn tại, thêm mới vào giỏ hàng
                _cacheDb.HashSet(cartKey, productId.ToString(), quantity);
            }

        }
        public object RemoveData(Guid userId)
        {
            var cartKey = $"cart:{userId}_ref";
            var _exist = _cacheDb.KeyExists(cartKey);
            if (_exist)
            {
                return _cacheDb.KeyDelete(cartKey);
            }
            return false;
        }
        public void RemoveProductFromCart(Guid userId, Guid productId)
        {
            var cartKey = $"cart:{userId}_ref";

            // Kiểm tra xem sản phẩm có tồn tại trong giỏ hàng không
            if (_cacheDb.HashExists(cartKey, productId.ToString()))
            {
                // Nếu sản phẩm tồn tại, xóa nó khỏi giỏ hàng
                _cacheDb.HashDelete(cartKey, productId.ToString());
            }
        }


    }
}
