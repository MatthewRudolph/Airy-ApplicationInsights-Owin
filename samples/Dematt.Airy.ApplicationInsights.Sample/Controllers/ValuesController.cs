using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;


namespace Dematt.Airy.ApplicationInsights.Sample.Controllers
{
    using System.Collections.Generic;

    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        // GET api/values
        [Route("")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // GET api/values/attribute/5
        [AcceptVerbs("GET")]
        [Route("attribute/{id:int}")]
        public string AttributeGet(int id)
        {
            //var attributedRoutesData = Request.GetRouteData().GetSubRoutes();
            //var subRouteData = attributedRoutesData.FirstOrDefault();
            //var actions = (ReflectedHttpActionDescriptor[])subRouteData.Route.DataTokens["actions"];

            var webapiRouteData = Request.GetRouteData();
            var actions = (HttpActionDescriptor[])webapiRouteData.Route.DataTokens["actions"];

            var controllerName = actions[0].ControllerDescriptor.ControllerName;
            var controllerAction = actions[0].ActionName;
            var routeTemplate = webapiRouteData.Route.RouteTemplate;

            //var routeData = Request.GetRouteData();
            //string name = routeData.Route.RouteTemplate;
            ////var subroutes = (IEnumerable<IHttpRouteData>)routeData.Values["MS_SubRoutes"];
            ////var route = subroutes.First().Route;
            return id.ToString();
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
