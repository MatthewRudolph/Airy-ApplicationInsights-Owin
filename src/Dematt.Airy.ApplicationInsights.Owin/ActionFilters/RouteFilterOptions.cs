using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dematt.Airy.ApplicationInsights.Owin.ActionFilters
{
    public class RouteFilterOptions
    {
        public RouteFilterOptions()
        {
            IncludeParamterNames = false;
            WebApiRoutePrefix = "api/";
        }

        public bool IncludeParamterNames { get; set; }

        public string WebApiRoutePrefix { get; set; }
    }
}
