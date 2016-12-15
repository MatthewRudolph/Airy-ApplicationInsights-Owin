using System.Web.Http;
using System.Web.Mvc;
using Dematt.Airy.ApplicationInsights.Owin.ActionFilters;
using Dematt.Airy.ApplicationInsights.Owin.Middleware;
using Microsoft.ApplicationInsights;
using Owin;

namespace Dematt.Airy.ApplicationInsights.Owin
{
    /// <summary>
    /// Extension methods for <see cref="IAppBuilder"/>
    /// </summary>
    public static class AppBuilderExtension
    {
        /// <summary>
        /// Extension method for <see cref="IAppBuilder"/> that configures Application Insights for Owin.
        /// </summary>
        public static IAppBuilder UseApplicationInsightsOwin(this IAppBuilder builder, HttpConfiguration httpConfiguration,
            RouteFilterOptions options, TelemetryClient telemetryClient = null)
        {
            telemetryClient = telemetryClient ?? new TelemetryClient();

            builder.Use<RequestTrackingMiddleware>(telemetryClient);

            httpConfiguration.Filters.Add(new WebApiRouteFilterAttribute(options));
            GlobalFilters.Filters.Add(new MvcRouteFilterAttribute(options));

            return builder;
        }
    }
}
