using JewelryEC_Backend.Service.IService;
using StackExchange.Redis;
using System.Text.Json;


namespace JewelryEC_Backend.Service
{
    public class CacheService : ICacheService
    {
        private IDatabase _cacheDb;
        protected readonly IConfiguration _configuration;
        public CacheService(IConfiguration configuration)
        {
            _configuration = configuration;
            connectRedis();

        }

        private void connectRedis()
        {
            var redis = ConnectionMultiplexer.Connect(_configuration.GetConnectionString("MyRedisConStr").ToString());
            _cacheDb = redis.GetDatabase();
        }

        public T GetData<T>(string key)
        {
            var value = _cacheDb.StringGet(key);
            if(!string.IsNullOrEmpty(value))
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            return default;
        }

        public object RemoveData(string key)
        {
            var _exist  = _cacheDb.KeyExists(key);
            if(_exist)
            {
                return _cacheDb.KeyDelete(key);
            }
            return false;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            return _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expiryTime);
        }
    }
}
