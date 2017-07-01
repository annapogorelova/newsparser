using System;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace NewsParser.Cache 
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        
        /// <summary>
        /// Default cache duration in seconds 
        /// </summary>
        private static int DefaultCacheDuration => 60;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public T Get<T>(string key) where T : class
        {
            var fromCache = _cache.Get(key);
            if (fromCache == null)
            {
                return null;
            }

            var str = Encoding.UTF8.GetString(fromCache);
            if (typeof(T) == typeof(string))
            {
                return str as T;
            }

            return JsonConvert.DeserializeObject<T>(str);
        }

        public void Store(string key, object content)
        {
            Store(key, content, DefaultCacheDuration);
        }

        public void Store(string key, object content, int duration)
        {
            string toStore;
            if (content is string)
            {
                toStore = (string)content;
            }
            else
            {
                toStore = JsonConvert.SerializeObject(content);
            }

            duration = duration <= 0 ? DefaultCacheDuration : duration;
            _cache.Set(key, Encoding.UTF8.GetBytes(toStore), new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now + TimeSpan.FromSeconds(duration)
            });
        }
    }
}