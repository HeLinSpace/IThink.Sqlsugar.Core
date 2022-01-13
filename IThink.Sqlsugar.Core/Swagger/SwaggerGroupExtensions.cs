/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;

namespace IThink.Sqlsugar.Core.Swagger
{
    /// <summary>
    /// 
    /// </summary>
    public class SwaggerGroupAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">模块名称</param>
        /// <param name="desc">模块描述</param>
        public SwaggerGroupAttribute(string name, string desc)
        {
            GroupName = name;
            Description = desc;
        }

        /// <summary>
        /// 
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
    }
}
