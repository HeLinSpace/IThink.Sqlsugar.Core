/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IThink.Sqlsugar.Core
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public partial interface IDbContext : IDisposable
    {
        #region Methods

        /// <summary>
        /// 创建可用于查询和保存实体实例的数据库集
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>给定实体类型的集合</returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        /// 生成脚本以创建当前模型的所有表
        /// </summary>
        /// <returns>A SQL script</returns>
        string GenerateCreateScript();

        /// <summary>
        /// 数据库对象
        /// </summary>
        DatabaseFacade Database { get; }

        /// <summary>
        /// 将此上下文中所做的所有更改保存到数据库
        /// </summary>
        /// <returns>写入数据库的状态条目数</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 将此上下文中所做的所有更改保存到数据库
        /// </summary>
        /// <returns>写入数据库的状态条目数</returns>
        int SaveChanges();

        #endregion
    }
}
