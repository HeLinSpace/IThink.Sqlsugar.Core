/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using System;

namespace IThink.Sqlsugar.Core
{
    /// <summary>
    /// 业务异常
    /// </summary>
    [Serializable]
    public class BusinessException : Exception
    {
        /// <summary>
        /// 状态Code
        /// </summary>
        public SystemCode StatusCode { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        public BusinessException()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="formatParameters"></param>
        public BusinessException(SystemCode statusCode = SystemCode.SystemException, params object[] formatParameters)
            : base(statusCode.Message(formatParameters))
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="formatParameters"></param>
        public BusinessException(string message)
            : base(message)
        {
            StatusCode = SystemCode.SystemException;
        }


        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="formatParameters"></param>
        public BusinessException(SystemCode code, string message)
            : base(message)
        {
            StatusCode = code;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="statusCode"></param>
        /// <param name="formatParameters"></param>
        public BusinessException(Exception innerException, SystemCode statusCode = SystemCode.SystemException,
            params object[] formatParameters)
            : base(statusCode.Message(formatParameters), innerException)
        {
            StatusCode = statusCode;
        }
    }
}
