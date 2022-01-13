/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using IThink.Sqlsugar.Core.PrivateStartup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace IThink.Sqlsugar.Core
{
    /// <summary>
    /// 启动时通用配置
    /// </summary>
    public static class IThinkStartup
    {
        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static IServiceProvider AddIThinkApi(this IServiceCollection services, IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment)
        {
            // 添加TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            IdentityModelEventSource.ShowPII = true; //To show detail of error and see the problem
            
            services.AddMvcCore();

            //add accessor to HttpContext
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuthentication(configuration);

            // 文件提供者
            CommonHelper.DefaultFileProvider = new FileProvider(hostingEnvironment);

            //compression
            services.AddResponseCompression();

            //add options feature
            services.AddOptions();

            //add localization
            services.AddLocalization();

            // kestrel
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // 添加基本的MVC功能
            var mvcBuilder = services.AddMvc(opts =>
            {
                if (configuration.GetValue<string>("ApiConfig:DocName") == "identity")
                {
                    opts.EnableEndpointRouting = false;
                    return;
                }

                opts.EnableEndpointRouting = false;

                // 添加新的路由约定
                var convention = new ApiRouteConvention(
                    configuration.GetValue<string>("ApiConfig:DocName"),
                    (c) => c.ControllerType.BaseType == typeof(BaseApiController));
                opts.Conventions.Insert(0, convention);
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = c =>
                {
                    var error = c.ModelState.Values.Where(v => v.Errors.Count > 0)
                      .SelectMany(v => v.Errors)
                      .Select(v => v.ErrorMessage)
                      .FirstOrDefault();

                    return new BadRequestObjectResult(JsonConvert.DeserializeObject<SingleResponse>(error));
                };
            });

            services.Configure<FormOptions>(x =>
            {
                x.MultipartBodyLengthLimit = 1024000000;
            });

            // 设置MvcOptions上设置的默认值以匹配asp.net core mvc 2.2的行为
            mvcBuilder.SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //register controllers as services, it'll allow to override them
            mvcBuilder.AddControllersAsServices();

            services.AddCors(options =>
            {
                //options.AddPolicy("CorsPolicy",
                options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
            // 注入类型查找器
            services.AddSingleton<ITypeFinder,WebAppTypeFinder>();
            
            services.AddSqlSugar(configuration);


            // 初始化引擎
            var engine = EngineContext.Create();

            var serviceProvider = engine.ConfigureServices(services, configuration);

            return serviceProvider;
        }

        /// <summary>
        /// 配置请求管道
        /// </summary>
        /// <param name="application"></param>
        public static void AddIThinkApi(this IApplicationBuilder application)
        {
            application.UseResponseCompression();

            //use static files feature
            application.UseStaticFiles();

            application.UseRequestLocalization(options =>
            {
                //prepare supported cultures
                var defaultCultrue = new CultureInfo("zh-cn");
                options.SupportedCultures = new List<CultureInfo> { defaultCultrue };
                options.DefaultRequestCulture = new RequestCulture(defaultCultrue);
            });

            // 配置请求返回日志
            application.UseMiddleware<LoggerMiddleware>();

            application.UseCors("CorsPolicy");

            application.AddAuthentication();

            //路由
            application.UseMvc();

            application.AddSwagger();
        }
    }
}