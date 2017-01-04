using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility.Implementation;

namespace Dematt.Airy.ApplicationInsights.Owin.ExceptionTracking
{
    public class WebApiExceptionLogger : ExceptionLogger
    {
        private readonly TelemetryClient _telemetryClient;
        private readonly string _sdkVersion;

        public WebApiExceptionLogger(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient ?? new TelemetryClient();
            _sdkVersion = SdkVersionUtils.GetSdkVersion("owin:");
        }

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
