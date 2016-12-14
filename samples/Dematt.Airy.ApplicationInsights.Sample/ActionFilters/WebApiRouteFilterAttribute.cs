using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Dematt.Airy.ApplicationInsights.Sample.ActionFilters
{
    public class WebApiRouteFilterAttribute : ActionFilterAttribute
    {
        private readonly WebApiRouteFilterOptions _options;

        public WebApiRouteFilterAttribute(WebApiRouteFilterOptions options)
        {
            _options = options;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string name;

            string controllerName = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            string actionName = actionContext.ActionDescriptor.ActionName;
            var parameters = actionContext.ActionDescriptor.ActionBinding.ParameterBindings;

            name = "api/" + controllerName + "/" + actionName;
            var routeData = actionContext.Request.GetRouteData();

            if (_options.IncludeParamterNames)
            foreach (var paramter in parameters)
            {
                    name += "/{" + paramter.Descriptor.ParameterName + "}";
            }

            //foreach (var value in routeData.Values)
            //{
            //    if (value.Key != "controller" && value.Key != "action")
            //    {
            //        name += "/{" + value.Key + "}";
            //    }
            //}

            var owinContect = actionContext.Request.GetOwinContext();
            owinContect.Environment.Add("aiOwin:ControllerAction", name);
        }
    }
}