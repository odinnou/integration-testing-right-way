using FluentAssertions;
using System.Net;
using Tests.Configuration;
using Xunit;

namespace Tests.Integrations.RestAdapters.hc;

public class HealthCheckRestAdapterIntegrationTest : BaseIntegrationTest
{
    [Fact]
    public async Task HealthCheck_route_should_returns_OK_and_Healthy_word()
    {
        // arrange: setup a new TestServer
        using (TestServer = HostConfiguration.Factory().Server)
        {
            using HttpClient httpClient = TestServer.CreateClient();

            // act: make an http call to the GET /hc endpoint
            HttpResponseMessage httpResponse = await httpClient.GetAsync("/hc");

            // assert: verify HTTP status code and content as plain text
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            string result = await httpResponse.Content.ReadAsStringAsync()!;
            result.Should().Be("Healthy");
        }
    }
}
