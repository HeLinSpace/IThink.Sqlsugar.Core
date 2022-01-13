/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using System.Collections.Generic;
using System.Security.Claims;

namespace IThink.Sqlsugar.Core
{
    /// <summary>
    /// TokenValidate扩展
    /// </summary>
    public static class TokenValidatedExtension
    {
        /// <summary>
        /// 解析token内容，设定工作上下文
        /// </summary>
        /// <param name="principal"></param>
        public static void SetWorkEmployee(this ClaimsPrincipal principal)
        {
            var fileProvider = Singleton<IEngine>.Instance.Resolve<IThinkFileProvider>();
            var workContext = Singleton<IEngine>.Instance.Resolve<IWorkContext>();
            // 设定上下文用户对象
            workContext.Current = new WorkEmployee();
            foreach (var item in principal.Claims)
            {
                switch (item.Type)
                {
                    
                    case "id":
                        workContext.Current.EmplId = item.Value;
                        break;
                    case "name":
                        workContext.Current.EmployeeName = item.Value;
                        break;
                    case "login_name":
                        workContext.Current.LoginName = item.Value;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}