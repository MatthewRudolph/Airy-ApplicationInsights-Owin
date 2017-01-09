using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Dematt.Airy.ApplicationInsights.Owin.ActionFilters
{
    /// <summary>
    /// Web API Filter Attribute that stores information about the controller and action used (including optionally the parameter names) in the owin context.
    /// </summary>
    public class WebApiRouteFilterAttribute : ActionFilterAttribute
    {
        private readonly RouteFilterOptions _options;

        /// <summary>
        /// Creates an instance of the <see cref="WebApiRouteFilterAttribute"/> using the <see cref="RouteFilterOptions"/> passed.
        /// </summary>
        public WebApiRouteFilterAttribute(RouteFilterOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Overrides the on action executing storing the controller and action used (including optionally the parameter names) in the owin context.
        /// </summary>
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
