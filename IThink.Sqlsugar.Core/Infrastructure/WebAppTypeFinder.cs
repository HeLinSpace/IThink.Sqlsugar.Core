/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Reflection;

namespace IThink.Sqlsugar.Core
{
    /// <summary>
    /// web类型查找器
    /// </summary>
    public class WebAppTypeFinder : AppDomainTypeFinder
    {
        #region Fields

        private bool _binFolderAssembliesLoaded;

        #endregion

        #region Ctor
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="fileProvider"></param>
        public WebAppTypeFinder(IThinkFileProvider fileProvider = null) : base(fileProvider)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// 获取\Bin目录的物理磁盘路径
        /// </summary>
        /// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
        public virtual string GetBinDirectory()
        {
            return AppContext.BaseDirectory;
        }

        /// <summary>
        /// 获取程序集
        /// </summary>
        /// <returns>Result</returns>
        public override IList<Assembly> GetAssemblies()
        {
            if (_binFolderAssembliesLoaded) 
                return base.GetAssemblies();

            _binFolderAssembliesLoaded = true;
            var binPath = GetBinDirectory();
            //binPath = _webHelper.MapPath("~/bin");
            LoadMatchingAssemblies(binPath);

            return base.GetAssemblies();
        }

        #endregion
    }
}
