/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IThink.Sqlsugar.Core
{
    /// <summary>
    /// 中间件
    /// </summary>
    public class LoggerMiddleware
    {
        private readonly ILogger<LoggerMiddleware> _logger;

        private readonly RequestDelegate _next;

        public LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestId = Guid.NewGuid().ToString();
            Stream originalBodyStream=null;
            try
            {
                context.Request.EnableBuffering();

                // 获取 Api 请求内容
                var requestBody = await GetRequesContent(context);

                _logger.LogInformation($"RequestId：{requestId}，请求开始：{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}，请求接口：{context.Request.Path.Value},请求参数：{requestBody}");

                originalBodyStream = context.Response.Body;

                using (var responseBody = new MemoryStream())
                {
                    //...and use that for the temporary response body
                    context.Response.Body = responseBody;

                    //Continue down the Middleware pipeline, eventually returning to this class
                    await _next(context);

                    var response = await GetResponse(context.Response);

                    _logger.LogInformation($"RequestId：{requestId}，请求结束：{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}，请求接口：{context.Request.Path.Value},返回值：{response}");

                    await responseBody.CopyToAsync(originalBodyStream);

                    context.Response.Body = originalBodyStream;
                }
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e, requestId, originalBodyStream);
            }
        }

        private async Task<string> GetRequesContent(HttpContext context)
        {
            var request = context.Request;
            var sr = new StreamReader(request.Body);

            var content = $"{await sr.ReadToEndAsync()}";

            if (!string.IsNullOrEmpty(content))
            {
                request.Body.Position = 0;
            }

            return content;
        }

        /// <summary>
        /// 格式化返回参数
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task<string> GetResponse(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();

            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);

            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return text;
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception e, string requestId, Stream originalBodyStream)
        {
            originalBodyStream = originalBodyStream ?? context.Response.Body;
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.ContentType = "application/json";

            var result = e as BusinessException;
            if (e.GetType() != typeof(BusinessException))
                result = new BusinessException(e.Message);

            _logger.LogInformation($"RequestId：{requestId}，请求异常结束：{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}，请求接口：{context.Request.Path.Value},异常消息：{e.Message}");
            if (!string.IsNullOrEmpty(e.StackTrace))
                _logger.LogInformation(e.StackTrace);
            if (e.InnerException != null)
                _logger.LogInformation(e.InnerException.Message);

            await JsonSerializer.SerializeAsync(originalBodyStream, DataResponse.Create(result.StatusCode, result.Message));

            context.Response.Body = originalBodyStream;
        }
    }
}

