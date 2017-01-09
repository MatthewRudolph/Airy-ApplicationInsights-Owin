using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Dematt.Airy.ApplicationInsights.Sample.Controllers
{
    public class ValuesController : ApiController
    {
        /// <summary>
        /// Web API convention based route.
        /// </summary>
        /// <remarks>GET api/values</remarks>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Web API convention based route with parameter.
        /// </summary>
        /// <remarks>GET api/values/5</remarks>
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// Web API attribute based route with parameter.
        /// </summary>
        /// <remarks>GET api/values/attribute/5</remarks>
        [AcceptVerbs("GET")]
        [Route("api/attribute")]
        public string AttributeGet()
        {
            return "attribute value";
        }

        /// <summary>
        /// Web API attribute based route with parameter.
        /// </summary>
        /// <remarks>GET api/values/attribute/5</remarks>
        [AcceptVerbs("GET")]
        [Route("api/attribute/{id:int}")]
        public string AttributeGetWithParameter(int id)
        {
            var webapiRouteData = Request.GetRouteData();
            var actions = (HttpActionDescriptor[])webapiRouteData.Route.DataTokens["actions"];
            var controllerName = actions[0].ControllerDescriptor.ControllerName;
            var controllerAction = actions[0].ActionName;
            var routeTemplate = webapiRouteData.Route.RouteTemplate;

            var returnValue = new StringBuilder("Id " + id + " was sent to the ");
            returnValue.Append(controllerName + " controller ");
            returnValue.AppendLine("and handled by the " + controllerAction + " method.");
            returnValue.Append("The following route template was used: " + routeTemplate);

            return returnValue.ToString();
        }

        /// <summary>
        /// Web API attribute based route that errors.
        /// </summary>
        /// <remarks>GET api/attribute/forceerror</remarks>
        [AcceptVerbs("GET")]
        [Route("api/attribute/forceerror")]
        public string AttributeError()
        {
            int conversionError = int.Parse("Oranges in a net.");
            return null;
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        /// <summary>
        /// Web API convention based route that errors
        /// </summary>
        /// <remarks>DELETE api/values/5</remarks>
        public void Delete(int id)
        {
            int conversionError = int.Parse("Box of apples");
        }
    }
}
