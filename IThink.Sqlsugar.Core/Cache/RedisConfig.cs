/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

namespace IThink.Sqlsugar.Core.Cache
{
    /// <summary>
    /// 
    /// </summary>
    public class RedisConfig
    {
        /// <summary>
        /// host
        /// </summary>
        public string[] Connection { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public string[] ConnectionReadOnly { get; set; }

        /// <summary>
        /// DefaultDatabase
        /// </summary>
        public int? DefaultDatabase { get; set; }

        /// <summary>
        /// 前缀
        /// </summary>
        public string Prefix { get; set; }
    }
}
