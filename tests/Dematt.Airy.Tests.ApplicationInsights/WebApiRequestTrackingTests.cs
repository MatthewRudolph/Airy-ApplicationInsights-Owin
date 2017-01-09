using System.Net;
using System.Net.Http;
using System.Threading;
using NUnit.Framework;

namespace Dematt.Airy.Tests.ApplicationInsights
{
    public class WebApiRequestTrackingTests
    {
        public static string SiteBaseUrl = "http://localhost:11156/";
        private static readonly HttpClient TestClient = new HttpClient();

        [Test]
        public void SucessfulConventionBasedRouteRequest()
        {
            TestClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Bot");
            using (var request = new HttpRequestMessage(HttpMethod.Get, SiteBaseUrl + "api/values"))
            using (var response = TestClient.SendAsync(request, CancellationToken.None).Result)
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
        }

        [Test]
        public void SucessfulConventionBasedRouteRequestWithParameter()
        {
            TestClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Bot");
            using (var request = new HttpRequestMessage(HttpMethod.Get, SiteBaseUrl + "api/values/5"))
            using (var response = TestClient.SendAsync(request, CancellationToken.None).Result)
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
        }

        [Test]
        public void SucessfulAttributeBasedRouteRequest()
        {
            TestClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Bot");
            using (var request = new HttpRequestMessage(HttpMethod.Get, SiteBaseUrl + "api/attribute"))
            using (var response = TestClient.SendAsync(request, CancellationToken.None).Result)
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
        }

        [Test]
        public void SucessfulAttributeBasedRouteRequestWithParameter()
        {
            TestClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Bot");
            using (var request = new HttpRequestMessage(HttpMethod.Get, SiteBaseUrl + "api/attribute/5"))
            using (var response = TestClient.SendAsync(request, CancellationToken.None).Result)
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
        }

        [Test]
        public void SucessfulAttributeBasedRouteRequestWithMultipleParameters()
        {
            TestClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Bot");
            using (var request = new HttpRequestMessage(HttpMethod.Get, SiteBaseUrl + "api/other/attribute/5?notId=4&notId=3"))
            using (var response = TestClient.SendAsync(request, CancellationToken.None).Result)
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
        }

        [Test]
        public void ServerErrorConventionBasedRouteRequest()
        {
            TestClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Bot");
            using (var request = new HttpRequestMessage(HttpMethod.Delete, SiteBaseUrl + "api/values/5"))
            using (var response = TestClient.SendAsync(request, CancellationToken.None).Result)
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
            }
        }

        [Test]
        public void ServerErrorAttributeBasedRouteRequest()
        {
            TestClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Bot");
            using (var request = new HttpRequestMessage(HttpMethod.Get, SiteBaseUrl + "api/attribute/forceerror"))
            using (var response = TestClient.SendAsync(request, CancellationToken.None).Result)
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
            }
        }
    }
}
