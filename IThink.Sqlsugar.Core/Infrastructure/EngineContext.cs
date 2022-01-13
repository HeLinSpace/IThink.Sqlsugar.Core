/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using System.Runtime.CompilerServices;

namespace IThink.Sqlsugar.Core
{
    /// <summary>
    /// 引擎上下文
    /// </summary>
    public class EngineContext
    {
        #region Methods

        /// <summary>
        /// 创建单例引擎
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Create()
        {
            //create NLSAPEngine as engine
            return Singleton<IEngine>.Instance ?? (Singleton<IEngine>.Instance = new Engine());
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// 获取当前引擎
        /// </summary>
        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Create();
                }

                return Singleton<IEngine>.Instance;
            }
        }

        #endregion
    }
}
