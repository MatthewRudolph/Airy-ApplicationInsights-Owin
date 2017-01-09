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
        /// Extension method for <see cref="IAppBuilder"/> that configures Application Insights for ASP.NET MVC and Web API using Owin running on IIS.
        /// </summary>
        public static IAppBuilder UseApplicationInsightsOwin(this IAppBuilder builder, HttpConfiguration httpConfiguration,
            RouteFilterOptions options, TelemetryClient telemetryClient = null)
        {
            telemetryClient = telemetryClient ?? new TelemetryClient();

            // ***IT IS IMPORTANT TO ADD ANY CUSTOM EXCEPTION LOGGERS BEFORE ANY OTHER CONFIGURATION***
            // Web API application insights error logging.
            httpConfiguration.Services.Add(typeof(IExceptionLogger), new WebApiExceptionLogger(telemetryClient));

            // Web API application insights controller and action name capture.
            httpConfiguration.Filters.Add(new WebApiRouteFilterAttribute(options));

            // ASP.Net MVC application insights controller and action name capture.
            GlobalFilters.Filters.Add(new MvcRouteFilterAttribute(options));

            // ASP.Net MVC application insights error logging.
            GlobalFilters.Filters.Add(new MvcExceptionHandler(telemetryClient));

            // Add the request tracking middleware that actual sends the TelemtryRequests for all owin requests both Web API and ASP.Net MVC.
            // This also tracks any errors that occur in the owin pipeline, however it will not capture errors that occur in any middleware that is added to the owin pipeline before it.
            builder.Use<RequestTrackingMiddleware>(telemetryClient);

            return builder;
        }
    }
}
