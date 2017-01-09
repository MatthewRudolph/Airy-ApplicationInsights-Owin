using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility.Implementation;

namespace Dematt.Airy.ApplicationInsights.Owin.ExceptionTracking
{
    /// <summary>
    /// Web API exception logger that logs exception details to Application Insights.
    /// </summary>
    public class WebApiExceptionLogger : ExceptionLogger
    {
        private readonly TelemetryClient _telemetryClient;
        private readonly string _sdkVersion;

        /// <summary>
        /// Creates an instance of the <see cref="WebApiExceptionLogger"/> using the <see cref="TelemetryClient"/> passed in.
        /// </summary>
        public WebApiExceptionLogger(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient ?? new TelemetryClient();
            _sdkVersion = SdkVersionUtils.GetSdkVersion("owin:");
        }

        /// <summary>
        /// Overrides the default Log method to send details of the error to Application Insights.
        /// </summary>
        public override void Log(ExceptionLoggerContext context)
        {
            if (context != null && context.Exception != null)
            {
                var exceptionTelemetry = new ExceptionTelemetry(context.Exception);
                exceptionTelemetry.Context.GetInternalContext().SdkVersion = _sdkVersion;

                // If there is a aiOwin:ControllerAction value in the owin environment use it for the name,
                // otherwise leave it empty and let the application insights initialisers set it.
                object name;
                if (context.Request.GetOwinContext().Environment.TryGetValue("aiOwin:ControllerAction", out name))
                {
                    exceptionTelemetry.Context.Operation.Name = context.Request.Method + " " + name;
                }
                _telemetryClient.TrackException(exceptionTelemetry);
            }
        }
    }
}
