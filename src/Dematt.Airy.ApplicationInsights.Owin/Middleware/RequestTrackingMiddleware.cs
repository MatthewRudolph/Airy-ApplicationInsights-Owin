using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.Owin;

namespace Dematt.Airy.ApplicationInsights.Owin.Middleware
{
    /// <summary>
    /// Owin middleware that captures request telemetry information and sends it to application insights.
    /// </summary>
    public class RequestTrackingMiddleware : OwinMiddleware
    {
        private readonly TelemetryClient _telemetryClient;
        private readonly string _sdkVersion;

        /// <summary>
        /// The only constructor for this class.
        /// The owin pipeline will pass the next parameter automatically.
        /// The telemetryClient parameter needs to passed either manually or using dependency injection, when this middleware is added to the pipeline.
        /// </summary>
        public RequestTrackingMiddleware(OwinMiddleware next, TelemetryClient telemetryClient)
            : base(next)
        {
            _telemetryClient = telemetryClient;
            _sdkVersion = SdkVersionUtils.GetSdkVersion("owin:");
        }

        /// <summary>
        /// The actual middleware async task that captures request telemetry information and sends it to application insights.
        /// </summary>
        public override async Task Invoke(IOwinContext context)
        {
            var requestTelemetry = new RequestTelemetry
            {
                Timestamp = DateTimeOffset.UtcNow
            };

            var sw = new Stopwatch();
            sw.Start();

            // Add the operation id to the owin context for others to use for application insights event correlation.
            context.Set("aiOwin:ContextOperationId", requestTelemetry.Context.Operation.Id);

            bool requestFailed = false;
            try
            {
                await Next.Invoke(context);
            }
            catch (Exception)
            {
                requestFailed = true;
                throw;
            }
            finally
            {
                sw.Stop();

                // If there is a aiOwin:ControllerAction value in the owin environment use it for the name,
                // otherwise leave it empty and let the application insights initialisers set it.
                object name;
                if (context.Environment.TryGetValue("aiOwin:ControllerAction", out name))
                {
                    requestTelemetry.Context.Operation.Name = context.Request.Method + " " + name;
                    requestTelemetry.Name = context.Request.Method + " " + name;
                }

                string path = context.Request.Path.ToString();

                requestTelemetry.Duration = sw.Elapsed;
                requestTelemetry.ResponseCode = context.Response.StatusCode.ToString();
                requestTelemetry.Success = !requestFailed && (context.Response.StatusCode < 400);
                requestTelemetry.HttpMethod = context.Request.Method;
                requestTelemetry.Url = context.Request.Uri;
                requestTelemetry.Context.GetInternalContext().SdkVersion = _sdkVersion;

                _telemetryClient.TrackRequest(requestTelemetry);
            }
        }
    }
}
