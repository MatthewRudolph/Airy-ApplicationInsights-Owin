using System.Web.Mvc;

namespace Dematt.Airy.ApplicationInsights.Sample.Controllers
{
    public class OtherController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult ForceError()
        {
            int conversionError = int.Parse("Bunch of bananas");
            return null;
        }

        [Route("strange/route")]
        public ActionResult MvcAttributeRouteWithError()
        {
            int conversionError = int.Parse("Why do this?");
            return null;
        }
    }
}