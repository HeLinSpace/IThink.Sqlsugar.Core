/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

namespace IThink.Sqlsugar.Core
{
    /// <summary>
    /// 当前工作上下文
    /// </summary>
    public interface IWorkContext
    {
        /// <summary>
        /// 当前人员信息
        /// </summary>
        WorkEmployee Current { get; set; }
    }
}
