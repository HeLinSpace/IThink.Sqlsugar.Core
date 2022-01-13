/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace IThink.Sqlsugar.Core
{
    /// <summary>
    /// IWebHostBuilder扩展
    /// </summary>
    public static class WebHostBuilderExtension
    {
        /// <summary>
        /// 加载P配置文件
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IWebHostBuilder ConfigureSettings(this IWebHostBuilder builder,
            Action<WebHostBuilderContext, IConfigurationBuilder> action = null)
        {
            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;

                // 本地开发环境
                if (env.EnvironmentName == "Development")
                {
                    config.AddJsonFile("appsettings.Development.json", optional: true);
                }
                // 线上测试环境
                else if(env.EnvironmentName == "Test")
                {
                    config.AddJsonFile("appsettings.Test.json", optional: true);
                }
                // 正式环境
                else
                {
                    // 发布环境寻找发布配置
                    config.AddJsonFile("appsettings.json", optional: true);
                }
                config.AddEnvironmentVariables();

                action?.Invoke(hostingContext, config);
            });
            return builder;
        }
    }
}
