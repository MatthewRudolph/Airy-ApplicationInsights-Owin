using System.Web;
using System.Web.Routing;

namespace Dematt.Airy.ApplicationInsights.Sample
{
    /// <summary>
    ///  http://stackoverflow.com/questions/36152468/mvc-site-using-webapi-ignore-api-routes-in-mvc-route-config
    /// </summary>
    public class NoApiControllerConstraint : IRouteConstraint
    {
        public bool Match
            (
                HttpContextBase httpContext,
                Route route,
                string parameterName,
                RouteValueDictionary values,
                RouteDirection routeDirection
            )
        {
            bool notApi = values["controller"].ToString() != "api";
            return notApi;
        }
    }
}