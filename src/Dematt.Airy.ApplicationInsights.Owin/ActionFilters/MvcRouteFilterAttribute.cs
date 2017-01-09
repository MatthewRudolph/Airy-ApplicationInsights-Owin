using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Dematt.Airy.ApplicationInsights.Owin.ActionFilters
{
    /// <summary>
    /// MVC Filter Attribute that stores information about the controller and action used (including optionally the parameter names) in the owin context.
    /// </summary>
    public class MvcRouteFilterAttribute : ActionFilterAttribute
    {
        private readonly RouteFilterOptions _options;

        /// <summary>
        /// Creates an instance of the <see cref="MvcRouteFilterAttribute"/> using the <see cref="RouteFilterOptions"/> passed.
        /// </summary>
        public MvcRouteFilterAttribute(RouteFilterOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Overrides the on action executing storing the controller and action used (including optionally the parameter names) in the owin context.
        /// </summary>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;

            // For convention based routing the ActionName for non-default actions is returned in all lowercase.
            // We need to use reflection to get the correctly cased action/method name.
            MethodInfo controllerMethod = filterContext.Controller.GetType().GetMethods().First(x => x.Name.Equals(actionName, StringComparison.OrdinalIgnoreCase));
            string methodName = controllerMethod.Name;

            string name = controllerName + "/" + methodName;

            if (_options.IncludeParamterNames)
            {
                ParameterDescriptor[] parameters = filterContext.ActionDescriptor.GetParameters();
                foreach (var paramter in parameters)
                {
                    name += "/{" + paramter.ParameterName + "}";
                }
            }

            var owinContect = filterContext.HttpContext.GetOwinContext();
            owinContect.Environment.Add("aiOwin:ControllerAction", name);
        }
    }
}
