/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace IThink.Sqlsugar.Core
{
    /// <summary>
    /// 工作单元上下文
    /// </summary>
    public class UnitObjectContext : DbContext, IDbContext
    {
        #region Ctor
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="options"></param>
        public UnitObjectContext(DbContextOptions<UnitObjectContext> options) : base(options)
        {
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 进一步配置模型
        /// </summary>
        /// <param name="modelBuilder">用于构造此上下文的模型的生成器</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        /// <summary>
        /// 通过添加传递的参数修改输入SQL查询
        /// </summary>
        /// <param name="sql">The raw SQL query</param>
        /// <param name="parameters">要分配给参数的值</param>
        /// <returns>修改的原始SQL查询</returns>
        protected virtual string CreateSqlWithParameters(string sql, params object[] parameters)
        {
            //add parameters to sql
            for (var i = 0; i <= (parameters?.Length ?? 0) - 1; i++)
            {
                if (!(parameters[i] is DbParameter parameter))
                    continue;

                sql = $"{sql}{(i > 0 ? "," : string.Empty)} {parameter.ParameterName}";

                //whether parameter is output
                if (parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Output)
                    sql = $"{sql} output";
            }

            return sql;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 创建可用于查询和保存实体实例的数据库集
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>给定实体类型的集合</returns>
        public virtual new DbSet<TEntity> Set<TEntity>() where TEntity : class
            => base.Set<TEntity>();

        /// <summary>
        /// 生成脚本以创建当前模型的所有表
        /// </summary>
        /// <returns>A SQL script</returns>
        public virtual string GenerateCreateScript()
            => Database.GenerateCreateScript();

        #endregion
    }
}
