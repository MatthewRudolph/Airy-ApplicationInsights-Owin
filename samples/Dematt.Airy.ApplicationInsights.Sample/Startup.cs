using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Dematt.Airy.ApplicationInsights.Owin.ActionFilters;
using Dematt.Airy.ApplicationInsights.Owin.ExceptionTracking;
using Dematt.Airy.ApplicationInsights.Owin.Middleware;
using Dematt.Airy.ApplicationInsights.Sample.Middleware;
using Microsoft.ApplicationInsights;
using Owin;

namespace Dematt.Airy.ApplicationInsights.Sample
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class Startup
    {
        // Having this as static allows use to register the MVC areas correctly avoiding the need to use GlobalConfiguration.Configuration.
        // And we really don't want to use GlobalConfiguration.Configuration when using Owin.
        // http://stackoverflow.com/questions/18921215/cant-get-asp-net-web-api-2-help-pages-working-when-using-owin
        public static HttpConfiguration HttpConfig { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            HttpConfig = new HttpConfiguration();

            HttpConfig.IncludeErrorDetailPolicy = GetWebApiErrorsMode();

            // Proposed nuget long/custom style.
            HttpConfig.Services.Add(typeof(IExceptionLogger), new WebApiExceptionLogger(new TelemetryClient()));
            HttpConfig.Filters.Add(new WebApiRouteFilterAttribute(new RouteFilterOptions { IncludeParamterNames = false }));
            GlobalFilters.Filters.Add(new MvcRouteFilterAttribute(new RouteFilterOptions { IncludeParamterNames = false }));
            app.Use<RequestTrackingMiddleware>(new TelemetryClient());

            // Proposed nuget extension style.
            //app.UseApplicationInsightsOwin(HttpConfig, new RouteFilterOptions(), new TelemetryClient());

            // Middleware for testing errors raised in middlewares.
            app.Use<ForceExceptionMiddleware>();

            app.Use(new Func<AppFunc, AppFunc>(next => (async env =>
            {
                await next.Invoke(env);
                Debug.WriteLine(HttpContext.Current.Request.Url + " : " + HttpContext.Current.Handler.ToString());
            })));

            // Configure owin to use WebApi.
            app.UseWebApi(HttpConfig);

            // Configure WebApi as required for the default Visual Studio Template.
            WebApiConfig.Register(HttpConfig);

            // Configure all the required MVC stuff for the default Visual Studio Template.
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            RouteConfig.RegisterMvcRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Gets the WepApi error detail policy to use based on the system.web/customErrors section of the web config.
        /// https://lostechies.com/jimmybogard/2012/04/18/custom-errors-and-error-detail-policy-in-asp-net-web-api/
        /// </summary>
        private IncludeErrorDetailPolicy GetWebApiErrorsMode()
        {
            var config = (CustomErrorsSection)ConfigurationManager.GetSection("system.web/customErrors");

            IncludeErrorDetailPolicy errorDetailPolicy;
            switch (config.Mode)
            {
                case CustomErrorsMode.RemoteOnly:
                    errorDetailPolicy = IncludeErrorDetailPolicy.LocalOnly;
                    break;
                case CustomErrorsMode.On:
                    errorDetailPolicy = IncludeErrorDetailPolicy.Never;
                    break;
                case CustomErrorsMode.Off:
                    errorDetailPolicy = IncludeErrorDetailPolicy.Always;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return errorDetailPolicy;
        }
    }
}
