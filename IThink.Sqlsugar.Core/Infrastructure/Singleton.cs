/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Threading;

namespace IThink.Sqlsugar.Core
{
    /// <summary>
    /// 单件管理(泛型)
    /// </summary>
    public class Singleton<T> : BaseSingleton
    {
        /// <summary>
        /// 实例
        /// </summary>
        private static T instance;

        /// <summary>
        /// 指定类型t的单一实例。对于每种类型的t，仅此对象的一个实例（每次）。
        /// </summary>
        public static T Instance
        {
            get => instance;
            set
            {
                instance = value;
                AllSingletons[typeof(T)] = value;
            }
        }
    }

    /// <summary>
    /// 基类单例
    /// </summary>
    public class BaseSingleton
    {
        /// <summary>
        /// ctor
        /// </summary>
        static BaseSingleton()
        {
            AllSingletons = new Dictionary<Type, object>();
        }

        /// <summary>
        /// 类型为singleton实例的字典。
        /// </summary>
        public static IDictionary<Type, object> AllSingletons { get; }
    }
}
