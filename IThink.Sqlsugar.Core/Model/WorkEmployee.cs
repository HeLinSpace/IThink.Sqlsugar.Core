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
    /// 当前工作的人员信息
    /// </summary>
    public class WorkEmployee
    {
        /// <summary>
        /// 人员ID
        /// </summary>
        public string EmplId { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 人员名称
        /// </summary>
        public string EmployeeName { get; set; }
    }
}
