using System.Collections.Generic;
using System.Web.Http;

namespace Dematt.Airy.ApplicationInsights.Sample.Controllers
{
    public class OtherApiController : ApiController
    {
        // GET api/values/attribute/5
        [AcceptVerbs("GET")]
        [Route("api/other/{id:int}")]
        public string AttributeGet(int id, [FromUri]List<int> notId)
        {
            return id.ToString();
        }
    }
}