using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Mvc;
using Dematt.Airy.ApplicationInsights.Owin.ActionFilters;
using Dematt.Airy.ApplicationInsights.Owin.ExceptionTracking;
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

            // ***IT IS IMPORTANT TO ADD ANY CUSTOM EXCEPTION LOGGERS BEFORE ANY OTHER CONFIGURATION***
            httpConfiguration.Services.Add(typeof(IExceptionLogger), new WebApiExceptionLogger(telemetryClient));

            httpConfiguration.Filters.Add(new WebApiRouteFilterAttribute(options));
            GlobalFilters.Filters.Add(new MvcRouteFilterAttribute(options));
            builder.Use<RequestTrackingMiddleware>(telemetryClient);

            return builder;
        }
    }
}
