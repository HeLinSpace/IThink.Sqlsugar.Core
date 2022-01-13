
using Autofac;
using IThink.Sqlsugar.Core;

namespace IThink.Demo.WebApi
{
    /// <summary>
    /// 全局解析依赖注入
    /// </summary>
    public class BusinessDependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// 加载顺序
        /// </summary>
        public int Order => 2;

        /// <summary>
        /// 业务实现
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="typeFinder"></param>
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //builder.RegisterType<XXXXService>().As<IXXXXService>().InstancePerLifetimeScope();
        }
    }
}
