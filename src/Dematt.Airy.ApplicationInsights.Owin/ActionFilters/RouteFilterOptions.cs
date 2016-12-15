namespace Dematt.Airy.ApplicationInsights.Owin.ActionFilters
{
    /// <summary>
    /// Stores options for the configuring the MVC and Web API Route Filter Attribute classes,
    /// </summary>
    public class RouteFilterOptions
    {
        /// <summary>
        /// Creates an instance of the <see cref="RouteFilterOptions"/>
        /// </summary>
        public RouteFilterOptions()
        {
            IncludeParamterNames = false;
            WebApiRoutePrefix = "api/";
        }

        /// <summary>
        /// Gets or sets if the parameter names of the action used should be appended to the ControllerAction string that is stored in the owin context.
        /// </summary>
        public bool IncludeParamterNames { get; set; }

        /// <summary>
        /// Gets or sets the prefix to pre-append to the ControllerAction string when a Web Api controller is used.
        /// </summary>
        public string WebApiRoutePrefix { get; set; }
    }
}
