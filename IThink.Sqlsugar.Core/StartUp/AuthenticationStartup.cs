/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace IThink.Sqlsugar.Core.PrivateStartup
{
    /// <summary>
    /// 身份认证启动器
    /// </summary>
    public static class AuthenticationStartup
    {
        /// <summary>
        /// 添加和配置身份认证
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var apiConfig = configuration.GetSection("ApiConfig").Get<ApiConfig>();
            if (apiConfig.DocName == "identity")
            {
                return;
            }
            var identityUrl = configuration.GetValue<string>("IdentityServer");
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = identityUrl;
                options.RequireHttpsMetadata = false;
                options.Audience = apiConfig.Audience;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false
                };
                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = ctx =>
                    {
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = ctx =>
                    {
                        ctx.Principal.SetWorkEmployee();
                        return Task.CompletedTask;
                    },
                };
            });
        }

        /// <summary>
        /// 配置添加中间件的使用
        /// </summary>
        /// <param name="application"></param>
        public static void AddAuthentication(this IApplicationBuilder application)
        {
            // 配置身份认证并且添加身份认证中间件
            application.UseAuthentication();
        }
    }
}
