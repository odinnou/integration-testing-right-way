using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Service.DrivenAdapters.DatabaseAdapters;
using Service.DrivenAdapters.DatabaseAdapters.Entities;
using Service.DrivingAdapters.RestAdapters.Dtos;
using System.Net;
using System.Net.Mime;
using System.Text;
using Tests.Configuration;
using Tests.Fixtures;
using Xunit;

namespace Tests.Integrations.RestAdapters.v1;

public class PandasRestAdapterIntegrationTest : BaseIntegrationTest
{
    #region Add

    [Fact]
    public async Task Add_should_returns_BadRequest_status_code_when_name_is_missing_and_coordinates_are_incorrect()
    {
        // arrange: force latitude, longitude and name to incorrect values
        InsertPandaDto dto = new() { Latitude = -91, Longitude = 181 };

        using (TestServer = HostConfiguration.Factory().Server)
        {
            await ResetAndInitDatabase(Dataset.Empty);

            using HttpClient httpClient = TestServer.CreateClient();

            // act
            HttpResponseMessage httpResponse = await httpClient.PostAsync($"/api/pandas", new StringContent(JsonConvert.SerializeObject(dto, new Newtonsoft.Json.Converters.StringEnumConverter()), Encoding.UTF8, MediaTypeNames.Application.Json));

            // assert: verify status code and check content as plain text
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            string result = await httpResponse.Content.ReadAsStringAsync();
            result.Should().ContainAll(nameof(InsertPandaDto.Name), nameof(InsertPandaDto.Latitude), nameof(InsertPandaDto.Longitude));
        }
    }

    [Fact]
    public async Task Add_should_returns_Ok_status_code_and_a_panda_with_specified_values()
    {
        // arrange: build a random DTO with AutoFixture (it relies on DataAnnotations attributes to set consistent values)
        InsertPandaDto dto = FixtureInstance.Create<InsertPandaDto>();

        using (TestServer = HostConfiguration.Factory().Server)
        {
            await ResetAndInitDatabase(Dataset.Empty);

            using HttpClient httpClient = TestServer.CreateClient();

            // act: make an http call to the POST /api/pandas endpoint, with DTO as JSON
            HttpResponseMessage httpResponse = await httpClient.PostAsync($"/api/pandas", new StringContent(JsonConvert.SerializeObject(dto, new Newtonsoft.Json.Converters.StringEnumConverter()), Encoding.UTF8, MediaTypeNames.Application.Json));

            // assert: with a DbContext, we could query the database to get the count or fetch the single panda
            using IServiceScope scope = TestServer.Services.CreateScope();
            using PandaContext dbContext = scope.ServiceProvider.GetRequiredService<PandaContext>();
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            PandaEntity pandaEntity = dbContext.Pandas.Single();
            PandaDto result = JsonConvert.DeserializeObject<PandaDto>(await httpResponse.Content.ReadAsStringAsync())!;
            result.Id.Should().NotBeEmpty();
            result.Id.Should().Be(pandaEntity.Id);
            result.Should().BeEquivalentTo(dto, options => options.ExcludingMissingMembers());
            pandaEntity.Should().BeEquivalentTo(dto, options => options.ExcludingMissingMembers());
        }
    }

    #endregion

    #region Get

    [Fact]
    public async Task Get_should_returns_Ok_status_code_and_a_panda_with_address_when_known_latitude_longitude()
    {
        // arrange
        using (TestServer = HostConfiguration.Factory().Server)
        {
            await ResetAndInitExpectations();
            await ResetAndInitDatabase(Dataset.FetchPanda);

            using HttpClient httpClient = TestServer.CreateClient();

            // act
            HttpResponseMessage httpResponse = await httpClient.GetAsync($"/api/pandas/{PandaData.Constants.PandaId}");

            // assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            PandaDto result = JsonConvert.DeserializeObject<PandaDto>(await httpResponse.Content.ReadAsStringAsync())!;
            result.Id.Should().Be(PandaData.Constants.PandaId);
            result.LastKnownAddress.Should().Be(PandaData.Constants.Address);
        }
    }

    [Fact]
    public async Task Get_should_returns_Ok_status_code_and_a_panda_without_address_when_unknown_latitude_longitude()
    {
        // arrange
        using (TestServer = HostConfiguration.Factory().Server)
        {
            await ResetAndInitExpectations();
            await ResetAndInitDatabase(Dataset.FetchPanda);

            using HttpClient httpClient = TestServer.CreateClient();

            // act
            HttpResponseMessage httpResponse = await httpClient.GetAsync($"/api/pandas/{PandaData.Constants.PandaId2}");

            // assert
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            PandaDto result = JsonConvert.DeserializeObject<PandaDto>(await httpResponse.Content.ReadAsStringAsync())!;
            result.Id.Should().Be(PandaData.Constants.PandaId2);
            result.LastKnownAddress.Should().BeNull();
        }
    }

    [Fact]
    public async Task Get_should_returns_NotFound_status_code_when_unknow_id()
    {
        // arrange: use a random uuid (not existing in the database)
        Guid pandaId = Guid.NewGuid();
        using (TestServer = HostConfiguration.Factory().Server)
        {
            await ResetAndInitExpectations();
            await ResetAndInitDatabase(Dataset.FetchPanda);

            using HttpClient httpClient = TestServer.CreateClient();

            // act
            HttpResponseMessage httpResponse = await httpClient.GetAsync($"/api/pandas/{pandaId}");

            // assert: verify status code and check content as plain text
            httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
            string result = await httpResponse.Content.ReadAsStringAsync();
            result.Should().Contain($"no panda found for id: {pandaId}");
        }
    }

    #endregion Get
}