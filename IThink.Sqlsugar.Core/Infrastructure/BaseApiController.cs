/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace IThink.Sqlsugar.Core
{
    /// <summary>
    /// api控制器基类
    /// </summary>
    [ApiController]
    [Authorize]
    public abstract class BaseApiController : ControllerBase
    {
        /// <summary>
        /// 重写OK方法
        /// </summary>
        /// <returns></returns>
        protected OkObjectResult OkResult()
        {
            return Ok(SingleResponse.Create());
        }

        /// <summary>
        /// ok data result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        protected OkObjectResult OkDataResultT<T>(T data)
        {
            return Ok(NLSAPDataResponse<T>.Create(data));
        }

        /// <summary>
        /// ok data result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        protected OkObjectResult OkDataResult(dynamic data)
        {
            return Ok(NLSAPDataResponse.Create(data));
        }

        /// <summary>
        /// ok page result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="totalCount"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected OkObjectResult OkPageDataResult<T>(int totalCount, List<T> data)
        {
            return Ok(NLSAPDataPageResponse<T>.Create(totalCount,data));
        }

        /// <summary>
        /// ok page result
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected OkObjectResult OkPageDataResult(int totalCount, List<dynamic> data)
        {
            var obj = NLSAPDataPageResponse.Create(totalCount, data);
            return Ok(obj);
        }
    }
}
