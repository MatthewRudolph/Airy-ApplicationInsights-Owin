using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
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
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //RouteConfig.RegisterMvcRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
