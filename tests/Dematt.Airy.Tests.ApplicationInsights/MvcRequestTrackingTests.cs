using System.Net;
using System.Net.Http;
using System.Threading;
using NUnit.Framework;

namespace Dematt.Airy.Tests.ApplicationInsights
{
    public class MvcRequestTrackingTests
    {
        public static string SiteBaseUrl = "http://localhost:11156/";
        private static readonly HttpClient TestClient = new HttpClient();

        [Test]
        public void SucessfulHomePageRequest()
        {
            TestClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Bot");
            using (var request = new HttpRequestMessage(HttpMethod.Get, SiteBaseUrl))
            using (var response = TestClient.SendAsync(request, CancellationToken.None).Result)
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
        }

        [Test]
        public void SucessfulConventionBasedRouteRequest()
        {
            TestClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Bot");
            using (var request = new HttpRequestMessage(HttpMethod.Get, SiteBaseUrl + "other"))
            using (var response = TestClient.SendAsync(request, CancellationToken.None).Result)
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
        }

        [Test]
        public void SucessfulConventionBasedRouteRequestWithMethod()
        {
            TestClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Bot");
            using (var request = new HttpRequestMessage(HttpMethod.Get, SiteBaseUrl + "other/anothermvcmethod"))
            using (var response = TestClient.SendAsync(request, CancellationToken.None).Result)
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
        }

        [Test]
        public void SucessfulAttributeBasedRouteRequest()
        {
            TestClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Bot");
            using (var request = new HttpRequestMessage(HttpMethod.Get, SiteBaseUrl + "other/attribute"))
            using (var response = TestClient.SendAsync(request, CancellationToken.None).Result)
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
        }

        [Test]
        public void ServerErrorConventionBasedRouteRequest()
        {
            TestClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Bot");
            using (var request = new HttpRequestMessage(HttpMethod.Get, SiteBaseUrl + "other/forceerror"))
            using (var response = TestClient.SendAsync(request, CancellationToken.None).Result)
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
            }
        }

        [Test]
        public void ServerErrorAttributeBasedRouteRequest()
        {
            TestClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Bot");
            using (var request = new HttpRequestMessage(HttpMethod.Get, SiteBaseUrl + "strange/route"))
            using (var response = TestClient.SendAsync(request, CancellationToken.None).Result)
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
            }
        }
    }
}
