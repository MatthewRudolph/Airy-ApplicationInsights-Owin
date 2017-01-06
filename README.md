# Airy-ApplicationInsights-Owin #

Use this library when you need Application Insights request and error tracking for **both** ASP.Net MVC and Web API using OWIN running on **IIS**.

If you are self hosting Web API using OWIN then the [applicationinsights-owinextensions](https://github.com/marcinbudny/applicationinsights-owinextensions) project may be a better fit as it works for both IIS and self hosted configurations.

#### Features: ####
  - Correct Operation Name resolution for attribute based routing in both ASP.NET MVC & WebApi (with and without parameter names.)
  - Tracking of errors in the ASP.Net MVC, Web API and OWIN pipelines. Including correlation with tracked requests.

#### Getting Started ####
  - Enable Application Insights for your web project. 
    https://docs.microsoft.com/en-us/azure/application-insights/app-insights-asp-net
  - Install using nuget.
    ```Powershell
    Install-Package Dematt.Airy.ApplicationInsights.Owin
    ```
  - Remove or comment out the following section from the ApplicationInsights.config file.
    It is no longer needed as the requests will now be tracked by the Owin middleware.
    ```xml
        <Add Type="Microsoft.ApplicationInsights.Web.RequestTrackingTelemetryModule, Microsoft.AI.Web">
      <Handlers>
        <!-- 
        Add entries here to filter out additional handlers: 
        
        NOTE: handler configuration will be lost upon NuGet upgrade.
        -->
        <Add>System.Web.Handlers.TransferRequestHandler</Add>
        <Add>Microsoft.VisualStudio.Web.PageInspector.Runtime.Tracing.RequestDataHttpHandler</Add>
        <Add>System.Web.StaticFileHandler</Add>
        <Add>System.Web.Handlers.AssemblyResourceLoader</Add>
        <Add>System.Web.Optimization.BundleHandler</Add>
        <Add>System.Web.Script.Services.ScriptHandlerFactory</Add>
        <Add>System.Web.Handlers.TraceHandler</Add>
        <Add>System.Web.Services.Discovery.DiscoveryRequestHandler</Add>
        <Add>System.Web.HttpDebugHandler</Add>
      </Handlers>
    </Add>
    ```
  - Add the following code to you Startup.cs file.
    ```C#
    public void Configuration(IAppBuilder app)
    {
        var httpConfig = new HttpConfiguration();        
        app.UseApplicationInsightsOwin(httpConfig, new RouteFilterOptions(), new TelemetryClient());
        // Other startup code would follow.
    }
    ```