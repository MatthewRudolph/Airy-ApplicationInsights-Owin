using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Dematt.Airy.ApplicationInsights.Owin.ActionFilters
{
    public class WebApiRouteFilterAttribute : ActionFilterAttribute
    {
        private readonly RouteFilterOptions _options;

        public WebApiRouteFilterAttribute(RouteFilterOptions options)
        {
            _options = options;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string controllerName = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            string actionName = actionContext.ActionDescriptor.ActionName;
            HttpParameterBinding[] parameters = actionContext.ActionDescriptor.ActionBinding.ParameterBindings;

            string name = _options.WebApiRoutePrefix + controllerName + "/" + actionName;

            if (_options.IncludeParamterNames)
                foreach (var paramter in parameters)
                {
                    name += "/{" + paramter.Descriptor.ParameterName + "}";
                }

            var owinContect = actionContext.Request.GetOwinContext();
            owinContect.Environment.Add("aiOwin:ControllerAction", name);
        }
    }
}
