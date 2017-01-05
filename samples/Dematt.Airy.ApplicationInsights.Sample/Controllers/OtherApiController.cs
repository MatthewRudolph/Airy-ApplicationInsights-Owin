using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Dematt.Airy.ApplicationInsights.Sample.Controllers
{
    [RoutePrefix("api/other")]
    public class OtherApiController : ApiController
    {
        /// <summary>
        /// Web API attribute based route with array parameter in url.
        /// </summary>
        /// <remarks>GET api/other/attribute/5</remarks>
        [AcceptVerbs("GET")]
        [Route("attribute/{id:int}")]
        public string AttributeGet(int id, [FromUri]List<int> notId)
        {
            return id.ToString();
        }

        /// <summary>
        /// Web API attribute based route with array parameter in url that errors.
        /// </summary>
        /// <remarks>GET api/other/attribute/error/6</remarks>
        [AcceptVerbs("GET")]
        [Route("attribute/error/{id:int}")]
        public string AttributeError(int id, [FromUri]List<int> notId)
        {
            return new Guid(id.ToString()).ToString();
        }
    }
}