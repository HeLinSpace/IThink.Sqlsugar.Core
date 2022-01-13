/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.Extensions.Configuration;

namespace IThink.Sqlsugar.Core
{
    /// <summary>
    /// 任务startup
    /// </summary>
    public interface IJobStartup
    {
        /// <summary>
        /// 任务startup
        /// </summary>
        void Configure(IConfiguration configuration);

        /// <summary>
        /// 顺序
        /// </summary>
        int Order { get; }
    }
}
