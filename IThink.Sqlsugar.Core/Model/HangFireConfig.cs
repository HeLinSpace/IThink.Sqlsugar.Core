/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

namespace IThink.Sqlsugar.Core
{
    public class HangFireConfig
    {
        /// <summary>
        /// 持久化方式
        /// </summary>
        public Endurance Endurance { get; set; }

        /// <summary>
        /// 连接字符串（持久化db）
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 初始化周期任务
        /// </summary>
        public bool InitRecurringJob { get; set; }

        /// <summary>
        /// 客户端/服务端
        /// </summary>
        public HangfireIdentity Identity { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string[] JobQueues { get; set; }

        /// <summary>
        /// 并发任务数量
        /// </summary>
        public int? WorkerCount { get; set; }

        /// <summary>
        /// 重试次数
        /// </summary>
        public int? RetryTimes { get; set; }

        /// <summary>
        /// 管理界面路由
        /// </summary>
        public string DashboardPath { get; set; }
    }

    public enum Endurance
    {
        Memory = 0,
        Redis = 1,
        PostgreSQL = 2,
        SqlServer = 3,
        MySql = 4,
    }

    public enum HangfireIdentity
    {
        Off = 0,
        Client = 1,
        Server = 2
    }
}
