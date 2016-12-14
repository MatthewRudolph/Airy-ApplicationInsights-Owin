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
        public static IAppBuilder UseApplicationInsightsOwin(this IAppBuilder builder, TelemetryClient telemetryClient = null)
        {
            telemetryClient = telemetryClient ?? new TelemetryClient();

            builder.Use<RequestTrackingMiddleware>(telemetryClient);

            return builder;
        }
    }
}
