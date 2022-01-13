/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace IThink.Sqlsugar.Core
{
    public static class HangFireStartUp
    {
        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddHangFire(this IServiceCollection services, IConfiguration configuration, string section = "HangFireConfig")
        {
            var config = configuration.GetSection(section).Get<HangFireConfig>();

            if (config != null)
            {
                if (config.Identity == HangfireIdentity.Off)
                {
                    return;
                }

                switch (config.Endurance)
                {
                    case Endurance.Redis:
                        services.AddHangfire(x => x.UseRedisStorage(config.ConnectionString));
                        break;
                    case Endurance.SqlServer:
                        services.AddHangfire(x => x.UseSqlServerStorage(config.ConnectionString,
                           new SqlServerStorageOptions
                           {
                               UsePageLocksOnDequeue = true, // Migration to Schema 7 is required
                               DisableGlobalLocks = true,    // Migration to Schema 7 is required
                               EnableHeavyMigrations = false // Default value: false
                           }));
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 配置请求管道
        /// </summary>
        /// <param name="application"></param>
        public static void AddHangFire(this IApplicationBuilder application, string section = "HangFireConfig")
        {
            var configuration = application.ApplicationServices.GetService<IConfiguration>();
            var config = configuration.GetSection(section).Get<HangFireConfig>();

            if (config != null)
            {
                // SQL server 只生成客户端连接 可能需要单独配置
                //if (configuration.GetValue<bool>("HangFireConfig:UseHangFire") && !configuration.GetValue<bool>("HangFireConfig:UseHangfireServer"))
                //{
                //    application.UseHangfireDashboard(configuration.GetValue<string>("HangFireConfig:DashboardPath"), new DashboardOptions
                //    {
                //        Authorization = new[] { new HangfireDashboardAuthorizationFilter() }
                //    });

                //    JobStorage.Current = new SqlServerStorage(configuration.GetValue<string>("HangFireConfig:ConnectionStr"));
                //}

                if (config.Identity != HangfireIdentity.Server)
                {
                    return;
                }

                // add these
                application.UseHangfireDashboard(config.DashboardPath);

                if (config.JobQueues == null)
                {
                    config.JobQueues = new string[] { "default" };
                }

                // 工作队列
                application.UseHangfireServer(new BackgroundJobServerOptions
                {
                    Queues = config.JobQueues,
                    WorkerCount = config.WorkerCount ?? 5
                });

                GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = config.RetryTimes ?? 5 });

                if (config.InitRecurringJob)
                {
                    //启动任务
                    var typeFinder = application.ApplicationServices.GetService<ITypeFinder>();
                    var startupConfigurations = typeFinder.FindClassesOfType<IJobStartup>();

                    var instances = startupConfigurations
                        .Select(startup => (IJobStartup)Activator.CreateInstance(startup))
                        .OrderBy(startup => startup.Order);

                    foreach (var instance in instances)
                        instance.Configure(configuration);
                }
            }
        }
    }
}
