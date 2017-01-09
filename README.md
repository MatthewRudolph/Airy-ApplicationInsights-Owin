# Dematt.Airy.ApplicationInsights.Owin #

### Summary ###
This library enables Application Insights request and error tracking for ASP.Net MVC and Web API using OWIN when running on **IIS**.

The default Request Tracking HttpModule for Application Insights does not resolve the operation name for attribute based routing correctly it defaults to using part of the Url.  This library fixes that issue so that requests to attribute based routes send application insights requests with the same name format as those made to convention based routes.

For example requests to:

```C#
public class ValuesController : ApiController
{
    public string Get(int id)
    {
        return "value";
    }

    [AcceptVerbs("GET")]
    [Route("api/attribute/{id:int}")]
    public string AttributeGetWithParameter(int id)
    {
        return returnValue.ToString();
    }
}
```
Will be sent the Operation Name values of:  
**GET api/Values/Get** and **GET api/Values/AttributeGetWithParameter**  
instead of:  
**GET api/values** and **GET api/attribute**



If you are self hosting Web API using OWIN then take a look at the [applicationinsights-owinextensions](https://github.com/marcinbudny/applicationinsights-owinextensions) project, as this project only works when IIS or IISExpress is used for the Owin host.

### Features ###
  - Correct Operation Name resolution for attribute based routing in both ASP.NET MVC & WebApi (with and without parameter names.)
  - Tracking of errors in the ASP.Net MVC, Web API and OWIN pipelines. Including correlation with tracked requests.

### Getting Started ###
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

### Options ###

  - #### Registering individual components ####
    If you do not want to change the Operation Name values used by Application Insights for request tracking or you do not want to log exceptions then you can register the ASP.Net MVC and Web API filters individually instead of using the AppBuilder extension method.
    
    Custom style for registering ApplicationInsights.Owin that allows registering of individual components.

    ```C#
    // Create telemtry client (or obtain from your IoC container of choice.)
    var telemetryClient = new TelemetryClient();
    // Web API application insights error logging.
    HttpConfig.Services.Add(typeof(IExceptionLogger), new WebApiExceptionLogger(telemetryClient));
    // Web API application insights controller and action name capture.
    HttpConfig.Filters.Add(new WebApiRouteFilterAttribute(new RouteFilterOptions { IncludeParamterNames = false }));
     // ASP.Net MVC application insights controller and action name capture.
    GlobalFilters.Filters.Add(new MvcRouteFilterAttribute(new RouteFilterOptions { IncludeParamterNames = false }));
    // ASP.Net MVC application insights error logging.
    GlobalFilters.Filters.Add(new MvcExceptionHandler(telemetryClient));
    // Request tracking middleware is required for both Web API and ASP.Net
    app.Use<RequestTrackingMiddleware>(new TelemetryClient());
    ```

    You'll need to register the **RequestTrackingMiddleware** at minimum to allow sending of Request Telemetries but the name capture and error logging components are optional. 

  - #### Configuration Options ####
    There are two options that can be supplied to the controller and action name capture filters that are passed using an instance of the RouteFilterOptions class.
    - ##### IncludeParamterNames  #####
      Sets if the parameter names of the action used should be appended to the Operation Name value used by Application Insights requests.
      
      Default value is false

      For example with the following web api controller method in the ValuesController class:
      
      ```C#
      public string Get(int id)
      {
          return "value";
      }
      ```
       With IncludeParamterNames set to false the Operation Name is set to:
       **GET api/Values/Get**  
       With IncludeParamterNames set to true the Operation Name is set to:
       **GET api/Values/Get/\{id\}**

       This is useful if you have MVC or API controllers with the same method name but different parameters, as it allows them to be grouped correctly in the Application Insights portal.

    - ##### WebApiRoutePrefix  #####
      Sets the prefix to pre-append to the Operation Name value used by Application Insights requests when a Web Api controller is used.
      
      Default value is "api"