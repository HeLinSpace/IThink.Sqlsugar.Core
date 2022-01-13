/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using SqlSugar;

namespace IThink.Sqlsugar.Core
{
    public class BaseSqlSugarClient : SqlSugarClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public BaseSqlSugarClient(SqlSugarDbConnectOption config) : base(config)
        {
            DbName = config.Name;
            Default = config.Default;
            UseCache = config.CacheModel != CacheModel.Off;
        }

        /// <summary>
        /// 
        /// </summary>
        public string DbName { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public bool Default { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public bool UseCache { set; get; }
    }
}
