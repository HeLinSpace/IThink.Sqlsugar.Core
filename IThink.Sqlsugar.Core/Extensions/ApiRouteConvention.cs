/* ---------------------------------------------------------------------    
 * 
 * Comment 					        Revision	Date        Author
 * -----------------------------    --------    --------    -----------
 * Created							1.0		    2022-1-13   mailhelin@qq.com
 *
 * ------------------------------------------------------------------------------*/

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace IThink.Sqlsugar.Core
{
    /// <summary>
    /// 路由约定
    /// </summary>
    public class ApiRouteConvention : IApplicationModelConvention
    {
        /// <summary>
        /// 前缀
        /// </summary>
        private readonly string prefix;

        /// <summary>
        /// 控制器选择器
        /// </summary>
        private readonly Func<ControllerModel, bool> controllerSelector;

        /// <summary>
        /// 前缀路由
        /// </summary>
        private readonly AttributeRouteModel onlyPrefixRoute;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="controllerSelector"></param>
        public ApiRouteConvention(string prefix, Func<ControllerModel, bool> controllerSelector)
        {
            this.prefix = prefix;
            this.controllerSelector = controllerSelector;

            // Prepare AttributeRouteModel local instances, ready to be added to the controllers

            //  This one is meant to be combined with existing route attributes
            onlyPrefixRoute = new AttributeRouteModel(new RouteAttribute(prefix));
        }

        /// <summary>
        /// 应用路由约定
        /// </summary>
        /// <param name="application"></param>
        public void Apply(ApplicationModel application)
        {
            // Loop through any controller matching our selector
            foreach (var controller in application.Controllers.Where(controllerSelector))
            {
                // Either update existing route attributes or add a new one
                if (controller.Selectors.Any(x => x.AttributeRouteModel != null))
                {
                    AddPrefixesToExistingRoutes(controller);
                }
                else
                {
                    AddNewRoute(controller);
                }
            }
        }

        /// <summary>
        /// 添加前缀当已经定义路由
        /// </summary>
        /// <param name="controller"></param>
        private void AddPrefixesToExistingRoutes(ControllerModel controller)
        {
            foreach (var selectorModel in controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList())
            {
                // Merge existing route models with the api prefix
                var originalAttributeRoute = selectorModel.AttributeRouteModel;
                selectorModel.AttributeRouteModel =
                    AttributeRouteModel.CombineAttributeRouteModel(onlyPrefixRoute, originalAttributeRoute);
            }
        }

        /// <summary>
        /// 添加新路由
        /// </summary>
        /// <param name="controller"></param>
        private void AddNewRoute(ControllerModel controller)
        {
            // The controller has no route attributes, lets add a default api convention 
            var defaultSelector = controller.Selectors.First(s => s.AttributeRouteModel == null);
            var nameSpace = prefix;
            if (string.IsNullOrWhiteSpace(prefix))
            {
                nameSpace = controller.ControllerType.Namespace.Split(".")[1].ToLower();
            }
            
            defaultSelector.AttributeRouteModel = new AttributeRouteModel(
                new RouteAttribute($"api/{nameSpace}/[controller]")); ;
        }
    }
}
