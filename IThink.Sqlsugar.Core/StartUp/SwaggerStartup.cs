/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IThink.Sqlsugar.Core.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IThink.Sqlsugar.Core.PrivateStartup
{
    /// <summary>
    /// 
    /// </summary>
    public static class SwaggerStartup
    {
        /// <summary>
        /// 添加Swagger配置启动
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            var apiConfig = configuration.GetSection("ApiConfig").Get<ApiConfig>();
            if (apiConfig.DocName == "identity")
            {
                return;
            }
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(apiConfig.DocName, new OpenApiInfo()
                {
                    Title = apiConfig.DocName,
                    Version = "v1"
                });

                var controllers = Singleton<ITypeFinder>.Instance.FindClassesOfType<BaseApiController>();

                var groups = controllers.Where(s => s.CustomAttributes.Any(x => x.AttributeType == typeof(SwaggerGroupAttribute)))
                .SelectMany(s => s.GetCustomAttributes(true).OfType<SwaggerGroupAttribute>()).Where(s => s != null)
                .GroupBy(s => s.GroupName).Select(s => new
                {
                    GroupName = s.Key,
                    Description = s.Max(x => x.Description)
                }).ToList();

                foreach (var group in groups)
                {
                    c.SwaggerDoc(apiConfig.DocName + "-" + group?.GroupName, new OpenApiInfo
                    {
                        Title = apiConfig.DocName + "-" + group?.GroupName,
                        Version = "v1",
                        Description = group?.Description
                    });
                }

                c.DocInclusionPredicate((docNameItem, apiDescription) =>
                {
                    if (!apiDescription.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                    var groupName = methodInfo.DeclaringType.GetCustomAttributes(true).OfType<SwaggerGroupAttribute>().Select(s => s.GroupName);

                    if (apiConfig.DocName == docNameItem && groupName.FirstOrDefault() == null)
                    {
                        return true;
                    }
                    return groupName.Any(v => apiConfig.DocName + "-" + v == docNameItem);
                });

                // 获取xml
                var files = CommonHelper.DefaultFileProvider.GetFiles(CommonHelper.DefaultFileProvider.MapPath("App_Data/XmlDocuments/"), "*.xml");
                foreach (var file in files)
                {
                    c.IncludeXmlComments(file, true);
                }

                //Swagger认证拦截器
                //c.OperationFilter<SwaggerAuthorizationOperationFilter>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

                // 添加样例拦截器
                c.ExampleFilters();

                //c.AddFluentValidationRules();
            }).AddSwaggerExamples();
        }

        /// <summary>
        /// 配置api中间件
        /// </summary>
        /// <param name="application"></param>
        public static void AddSwagger(this IApplicationBuilder application)
        {

            var configuration = application.ApplicationServices.GetService<IConfiguration>();
            var apiConfig = configuration.GetSection("ApiConfig").Get<ApiConfig>();
            if (apiConfig.DocName == "identity")
            {
                return;
            }

            // 启用中间件以将生成的Swagger作为JSON端点提供服务。
            application.UseSwagger(c =>
            {
                c.RouteTemplate = "/{documentName}/swagger/swagger.json";
                c.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    var scheme = "http";
                    if (!httpReq.Host.Value.StartsWith("localhost"))
                    {
                        scheme = "https";
                    }
                    swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{scheme}://{httpReq.Host.Value}" } };
                });
            });

            var docName = configuration.GetValue<string>("ApiConfig:DocName");
            // 启用中间件以提供swagger-ui（HTML，JS，CSS等），指定Swagger JSON端点。
            application.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/{docName}/swagger/swagger.json", docName);
                var controllers = Singleton<ITypeFinder>.Instance.FindClassesOfType<BaseApiController>();
                controllers.Where(s => s.CustomAttributes.Any(x => x.AttributeType == typeof(SwaggerGroupAttribute)))
                .SelectMany(s => s.GetCustomAttributes(true).OfType<SwaggerGroupAttribute>()).Where(s => s != null)
                .GroupBy(s => s.GroupName).Select(s => new
                {
                    GroupName = s.Key,
                    Description = s.Max(x => x.Description)
                }).ToList()
                .ForEach(s =>
                {
                    c.SwaggerEndpoint($"/{docName}-{ s?.GroupName}/swagger/swagger.json", docName + "-" + s?.GroupName);
                });

                c.RoutePrefix = $"{docName}/swagger";
            });
        }
    }
}