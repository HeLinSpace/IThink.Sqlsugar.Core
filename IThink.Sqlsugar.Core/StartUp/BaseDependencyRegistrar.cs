/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace IThink.Sqlsugar.Core
{
    public class BaseDependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 0;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<WorkContext>().As<IWorkContext>().InstancePerLifetimeScope();
            builder.RegisterType<FileProvider>().As<IThinkFileProvider>().InstancePerLifetimeScope();
        }
    }
}
