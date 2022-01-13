/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using IThink.Sqlsugar.Core.Cache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace IThink.Sqlsugar.Core.PrivateStartup
{
    public static class SqlSugarStartUp
    {
        /// <summary>
        /// 添加sqlSugar支持多数据库
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="contextLifetime"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugar(this IServiceCollection services, IConfiguration configuration, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, string section = "DbConfig")
        {
            var connectOptions = configuration.GetSection(section).Get<List<SqlSugarDbConnectOption>>();
            if (connectOptions != null)
            {
                foreach (var option in connectOptions)
                {
                    if (option != null)
                    {
                        if (option.DbType == DbType.PostgreSQL)
                        {
                            option.MoreSettings = new ConnMoreSettings { PgSqlIsAutoToLower = option.SqlIsAutoToLower };
                        }

                        option.InitKeyType = InitKeyType.Attribute;

                        if (option.CacheModel == CacheModel.Redis)
                        {
                            var instance = new RedisCache(configuration);
                            option.ConfigureExternalServices = new ConfigureExternalServices
                            {
                                DataInfoCacheService = instance
                            };
                        }

                        if (contextLifetime == ServiceLifetime.Scoped)
                        {
                            services.AddScoped(s => new BaseSqlSugarClient(option));
                        }
                        if (contextLifetime == ServiceLifetime.Singleton)
                        {
                            services.AddSingleton(s => new BaseSqlSugarClient(option));
                        }
                        if (contextLifetime == ServiceLifetime.Transient)
                        {
                            services.AddTransient(s => new BaseSqlSugarClient(option));
                        }
                    }
                }

                if (contextLifetime == ServiceLifetime.Scoped)
                {
                    services.AddScoped<ISqlSugarDbRepository, SqlSugarDbRepository>();
                }
                if (contextLifetime == ServiceLifetime.Singleton)
                {
                    services.AddSingleton<ISqlSugarDbRepository, SqlSugarDbRepository>();
                }
                if (contextLifetime == ServiceLifetime.Transient)
                {
                    services.AddTransient<ISqlSugarDbRepository, SqlSugarDbRepository>();
                }
            }

            return services;
        }
    }
}
