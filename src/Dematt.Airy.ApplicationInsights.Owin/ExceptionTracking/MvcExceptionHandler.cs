using System.Web;
using System.Web.Mvc;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility.Implementation;

namespace Dematt.Airy.ApplicationInsights.Owin.ExceptionTracking
{
    /// <summary>
    /// MVC error handler that logs error details to Application Insights.
    /// If custom errors is off then the Application Insights HTTP Module will log the error, so we only log errors when custom errors is on.
    /// </summary>
    public class MvcExceptionHandler : HandleErrorAttribute
    {
        private readonly TelemetryClient _telemetryClient;
        private readonly string _sdkVersion;

        /// <summary>
        /// Creates an instance of the <see cref="MvcExceptionHandler"/> using the <see cref="TelemetryClient"/> passed in.
        /// </summary>
        public MvcExceptionHandler(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
            _sdkVersion = SdkVersionUtils.GetSdkVersion("owin:");
        }

        /// <summary>
        /// Overrides the default OnException method to send details of the error to Application Insights.
        /// </summary>
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext != null && filterContext.HttpContext != null && filterContext.Exception != null)
            {
                //If customError is Off, then AI HTTPModule will report the exception.
                if (filterContext.HttpContext.IsCustomErrorEnabled)
                {
                    var exceptionTelemetry = new ExceptionTelemetry(filterContext.Exception);
                    exceptionTelemetry.Context.GetInternalContext().SdkVersion = _sdkVersion;

                    // If there is a aiOwin:ControllerAction value in the owin environment use it for the name,
                    // otherwise leave it empty and let the application insights initialisers set it.
                    object name;
                    if (filterContext.HttpContext.GetOwinContext().Environment.TryGetValue("aiOwin:ControllerAction", out name))
                    {
                        exceptionTelemetry.Context.Operation.Name = filterContext.HttpContext.Request.HttpMethod + " " + name;
                    }
                    _telemetryClient.TrackException(exceptionTelemetry);
                }
            }
            base.OnException(filterContext);
        }
    }
}
