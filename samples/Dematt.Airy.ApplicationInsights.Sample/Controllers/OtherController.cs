using System.Web.Mvc;

namespace Dematt.Airy.ApplicationInsights.Sample.Controllers
{
    public class OtherController : Controller
    {
        /// <summary>
        /// Convention based route.
        /// </summary>
        public ActionResult Index()
        {
            ViewBag.Title = "Other Home Page";

            return View();
        }

        /// <summary>
        /// Convention based route.
        /// </summary>
        public ActionResult AnotherMvcMethod()
        {
            ViewBag.Title = "Other Home Page";

            return View("Index");
        }

        /// <summary>
        /// Attribute based route.
        /// </summary>
        [Route("other/attribute")]
        public ActionResult AttributeHome()
        {
            ViewBag.Title = "Home Page";

            return View("Index");
        }

        /// <summary>
        /// Convention based route that errors.
        /// </summary>
        public ActionResult ForceError()
        {
            int conversionError = int.Parse("Bunch of bananas");
            return null;
        }

        /// <summary>
        /// Attribute based route that errors.
        /// </summary>
        [Route("strange/route")]
        public ActionResult MvcAttributeRouteWithError()
        {
            int conversionError = int.Parse("Why do this?");
            return null;
        }
    }
}