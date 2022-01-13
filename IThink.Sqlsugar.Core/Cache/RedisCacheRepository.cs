/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace IThink.Sqlsugar.Core.Cache
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisCacheRepository : IRedisCacheRepository
    {
        private readonly RedisHandle _dbCache;
        private readonly string _prefix;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public RedisCacheRepository(IConfiguration configuration)
        {
            var _config = configuration.GetSection("RedisConfig").Get<RedisConfig>();
            _prefix = _config.Prefix;
            _dbCache = new RedisHandle(_config);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add<V>(string key, V value)
        {
            _dbCache.Set(_prefix + key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheDurationInSeconds"></param>
        public void Add<V>(string key, V value, int cacheDurationInSeconds = 24 * 3600)
        {
            _dbCache.Set(_prefix + key, value, cacheDurationInSeconds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return _dbCache.ContainsKey(_prefix + key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public V Get<V>(string key)
        {
            return _dbCache.Get<V>(_prefix + key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <returns></returns>
        public IEnumerable<string> GetAllKeys(string pattern = "*")
        {
            return _dbCache.GetAllKeys(_prefix + pattern);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="create"></param>
        /// <param name="cacheDurationInSeconds"></param>
        /// <returns></returns>
        public V Get<V>(string cacheKey, Func<V> create = null, int cacheDurationInSeconds = int.MaxValue)
        {
            if (ContainsKey(cacheKey))
            {
                return Get<V>(cacheKey);
            }
            if (create != null)
            {
                var result = create();
                Add(cacheKey, result, cacheDurationInSeconds);
                return result;
            }
            return default(V);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            _dbCache.Remove(_prefix + key);
        }
    }
}