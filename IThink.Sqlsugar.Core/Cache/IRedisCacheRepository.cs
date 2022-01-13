/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;

namespace IThink.Sqlsugar.Core.Cache
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRedisCacheRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Add<V>(string key, V value);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheDurationInSeconds"></param>
        void Add<V>(string key, V value, int cacheDurationInSeconds = 3600 * 24);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        bool ContainsKey(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="create"></param>
        /// <param name="cacheDurationInSeconds"></param>
        /// <returns></returns>
        V Get<V>(string cacheKey, Func<V> create = null, int cacheDurationInSeconds = int.MaxValue);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetAllKeys(string pattern = "*");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
    }
}