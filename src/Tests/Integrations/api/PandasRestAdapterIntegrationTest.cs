using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Service.DrivingAdapters.RestAdapters.Dtos;
using System.Net;
using Tests.Configuration;
using Tests.Fixtures;
using Xunit;

namespace Tests.Integrations.RestAdapters.v1;

public class PandasRestAdapterIntegrationTest : BaseIntegrationTest
{
    #region Get

    [Fact]
    public async Task Get_Panda_should_returns_Ok_status_code_and_a_panda_with_address_when_known_latitude_longitude()
    {
        // arrange
        using (TestServer = HostConfiguration.Factory().Server)
        {
            await ResetAndInitExpectations();
            await ResetAndInitDatabase(Dataset.FetchPanda);

            HttpClient httpClient = TestServer.CreateClient();
            string route = $"/api/pandas/{PandaData.Constants.PandaId}";

            // act
            HttpResponseMessage httpResponse = await httpClient.GetAsync(route);

            // assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            PandaDto result = JsonConvert.DeserializeObject<PandaDto>(httpResponse.Content.ReadAsStringAsync().Result)!;
            result.Id.Should().Be(PandaData.Constants.PandaId);
            result.LastKnownAddress.Should().Be(PandaData.Constants.Address);
        }
    }

    [Fact]
    public async Task Get_Panda_should_returns_Ok_status_code_and_a_panda_without_address_when_unknown_latitude_longitude()
    {
        // arrange
        using (TestServer = HostConfiguration.Factory().Server)
        {
            await ResetAndInitExpectations();
            await ResetAndInitDatabase(Dataset.FetchPanda);

            HttpClient httpClient = TestServer.CreateClient();
            string route = $"/api/pandas/{PandaData.Constants.PandaId2}";

            // act
            HttpResponseMessage httpResponse = await httpClient.GetAsync(route);

            // assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            PandaDto result = JsonConvert.DeserializeObject<PandaDto>(httpResponse.Content.ReadAsStringAsync().Result)!;
            result.Id.Should().Be(PandaData.Constants.PandaId2);
            result.LastKnownAddress.Should().BeNull();
        }
    }

    [Fact]
    public async Task Get_Panda_should_returns_NotFound_status_code_when_unknow_id()
    {
        // arrange
        Guid pandaId = Guid.NewGuid();
        using (TestServer = HostConfiguration.Factory().Server)
        {
            await ResetAndInitExpectations();
            await ResetAndInitDatabase(Dataset.FetchPanda);

            HttpClient httpClient = TestServer.CreateClient();
            string route = $"/api/pandas/{pandaId}";

            // act
            HttpResponseMessage httpResponse = await httpClient.GetAsync(route);

            // assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            string result = httpResponse.Content.ReadAsStringAsync().Result;
            result.Should().Contain($"no panda found for id: {pandaId}");
        }
    }

    #endregion Get
}