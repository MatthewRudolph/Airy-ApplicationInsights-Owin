using System.Web.Mvc;
using System.Web.Routing;

namespace Dematt.Airy.ApplicationInsights.Sample
{
    public class RouteConfig
    {
        /// <summary>
        /// The default routes as set up by the Visual Studio template.
        /// </summary>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        /// <summary>
        /// This methods demonstrates that if you don't register any routes and only enable attribute routing for MVC
        /// then web api requests will have a Handler type of System.Web.Handlers.TransferRequestHandler which is the same as for a WebApi project without MVC in it.
        /// However if you register a default route as in the above method (the Visual Studio default template)
        /// then WepApi requests ill have a handler type of System.Web.Mvc.MvcHandler.  (This is of course only for IIS hosted OWIN deployments not self hosted.)
        /// This is relevant because the default configuration for Application Insights is set to ignore requests with a handler type of System.Web.Handlers.TransferRequestHandler
        /// Therefore WebApi requests in a MVC project with only MVC Attribute routing enabled will not be captured by ApplicationInsights with it's default configuration.
        /// </summary>
        public static void RegisterMvcRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
        }
    }
}
