/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using System;

namespace IThink.Sqlsugar.Core
{
    /// <summary>
    /// DB模型基类
    /// </summary>
    public abstract class BaseDBEntity : BaseEntity
    {
        #region Properties
        /// <summary>
        /// 删除标识
        public virtual bool IsDelete { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreatedTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime UpdatedTime { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }
        #endregion
    }
}
