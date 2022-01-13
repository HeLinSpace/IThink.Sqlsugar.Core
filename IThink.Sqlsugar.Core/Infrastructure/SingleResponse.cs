﻿/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IThink.Sqlsugar.Core
{
    /// <summary>
    /// 返回对象
    /// </summary>
    [DataContract]
    public class SingleResponse
    {
        /// <summary>
        /// 操作状态
        /// </summary>
        [DataMember(Name = "status")]
        public SystemCode Status { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        private string _message;

        /// <summary>
        /// 消息
        /// </summary>
        [DataMember(Name = "message")]
        public string Message
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_message))
                {
                    return Status.Message();
                }
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        /// <summary>
        /// 创建response
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static SingleResponse Create(SystemCode status = SystemCode.Success, params object[] formatParameters)
        {
            return new SingleResponse
            {
                Message = status.Message(formatParameters),
                Status = status
            };
        }

        /// <summary>
        /// 创建response
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static SingleResponse Create(string message, SystemCode status = SystemCode.Success)
        {
            return new SingleResponse
            {
                Message = message,
                Status = status
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,object> ToDictionary()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary["status"] = Status;
            dictionary["message"] = Message;
            return dictionary;
        }
    }

    /// <summary>
    /// 分页查询返回
    /// </summary>
    [DataContract]
    public class NLSAPDataResponse : SingleResponse
    {
        /// <summary>
        /// 数据列表
        /// </summary>
        [DataMember(Name = "data")]
        public dynamic Data { get; set; }

        /// <summary>
        /// 创建response
        /// </summary>
        /// <param name="data"></param>
        /// <param name="status"></param>
        /// <param name="formatParameters"></param>
        /// <returns></returns>
        public static NLSAPDataResponse Create(dynamic data,
            SystemCode status = SystemCode.Success,
            params object[] formatParameters)
        {
            return new NLSAPDataResponse
            {
                Message = status.Message(formatParameters),
                Status = status,
                Data = data
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    /// <summary>
    /// 查询操作结果返回
    /// </summary>
    [DataContract]
    public class NLSAPDataResponse<T> : NLSAPDataResponse
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        [DataMember(Name = "data")]
        public new T Data { get; set; }

        /// <summary>
        /// 创建response
        /// </summary>
        /// <param name="data"></param>
        /// <param name="status"></param>
        /// <param name="formatParameters"></param>
        /// <returns></returns>
        public static NLSAPDataResponse<T> Create(T data, SystemCode status = SystemCode.Success,
            params object[] formatParameters)
        {
            return new NLSAPDataResponse<T>
            {
                Message = status.Message(formatParameters),
                Status = status,
                Data = data
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    /// <summary>
    /// 分页查询返回
    /// </summary>
    [DataContract]
    public class NLSAPDataPageResponse : NLSAPDataResponse
    {
        /// <summary>
        /// total count
        /// </summary>
        [DataMember(Name = "totalCount")]
        public int TotalCount { get; set; }

        /// <summary>
        /// 创建response
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="data"></param>
        /// <param name="status"></param>
        /// <param name="formatParameters"></param>
        /// <returns></returns>
        public static NLSAPDataPageResponse Create(int totalCount, List<dynamic> data, SystemCode status = SystemCode.Success,
            params object[] formatParameters)
        {
            return new NLSAPDataPageResponse
            {
                TotalCount = totalCount,
                Message = status.Message(formatParameters),
                Status = status,
                Data = data
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    /// <summary>
    /// 分页查询返回
    /// </summary>
    [DataContract]
    public class NLSAPDataPageResponse<T> : NLSAPDataPageResponse
    {
        /// <summary>
        /// 创建response
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="data"></param>
        /// <param name="status"></param>
        /// <param name="formatParameters"></param>
        /// <returns></returns>
        public static NLSAPDataPageResponse<T> Create(int totalCount, List<T> data, SystemCode status = SystemCode.Success,
            params object[] formatParameters)
        {
            return new NLSAPDataPageResponse<T>
            {
                TotalCount = totalCount,
                Message = status.Message(formatParameters),
                Status = status,
                Data = data
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
