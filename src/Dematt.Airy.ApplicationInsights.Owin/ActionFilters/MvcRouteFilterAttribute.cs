using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Dematt.Airy.ApplicationInsights.Owin.ActionFilters
{
    public class MvcRouteFilterAttribute : ActionFilterAttribute
    {
        private readonly RouteFilterOptions _options;

        public MvcRouteFilterAttribute(RouteFilterOptions options)
        {
            _options = options;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;

            string name = controllerName + "/" + actionName;

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
