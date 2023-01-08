using FluentAssertions;
using System.Net;
using Tests.Configuration;
using Xunit;

namespace Tests.Integrations.RestAdapters.hc;

public class HealthCheckRestAdapterIntegrationTest : BaseIntegrationTest
{
    public HealthCheckRestAdapterIntegrationTest(DatabaseFixture databaseFixture)
    {
        Init(databaseFixture);
    }

    [Fact]
    public async Task HealthCheck_route_should_returns_OK_and_Healthy_word()
    {
        // arrange
        using (TestServer = HostConfiguration.Factory().Server)
        {
            await ResetDatabase(Dataset.Empty);
            HttpClient httpClient = TestServer.CreateClient();
            string route = "/hc";

            // act
            HttpResponseMessage httpResponse = await httpClient.GetAsync(route);

            // assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            string result = await httpResponse.Content.ReadAsStringAsync()!;
            result.Should().Be("Healthy");
        }
    }
}
