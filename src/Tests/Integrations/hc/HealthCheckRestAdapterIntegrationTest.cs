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
        // arrange
        using (TestServer = HostConfiguration.Factory().Server)
        {
            HttpClient httpClient = TestServer.CreateClient();

            // act
            HttpResponseMessage httpResponse = await httpClient.GetAsync("/hc");

            // assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            string result = await httpResponse.Content.ReadAsStringAsync()!;
            result.Should().Be("Healthy");
        }
    }
}
