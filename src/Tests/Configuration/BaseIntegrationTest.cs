using AutoFixture;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MockServerClientNet;
using Service.DrivenAdapters.DatabaseAdapters;
using Tests.Fixtures;
using Xunit;

#nullable disable warnings
namespace Tests.Configuration;

/// <summary>
/// All integration tests will be played sequentially (not parallelized) thanks to this collection name: avoiding unique constraint issues.
/// </summary>
[Collection("INTEGRATION_TEST_COLLECTION")]
public abstract class BaseIntegrationTest
{
    protected TestServer TestServer { get; set; }
    protected IFixture FixtureInstance { get; private set; }

    protected BaseIntegrationTest()
    {
        FixtureInstance = new Fixture();
    }

    protected static async Task ResetAndInitExpectations()
    {
        MockServerClient mockServerClient = new("localhost", 1090);

        // reset expectations
        await mockServerClient.ResetAsync();

        // init expectations
        await RoutesExpectation.SetExpectationsForGeocoding(mockServerClient);
    }

    protected async Task ResetAndInitDatabase(Dataset dataset = Dataset.Empty)
    {
        IServiceProvider iServiceProvider = TestServer.Services;

        using IServiceScope scope = iServiceProvider.CreateScope();
        using PandaContext dbContext = scope.ServiceProvider.GetRequiredService<PandaContext>();

        // reset datas
        await dbContext.Database.ExecuteSqlRawAsync("DELETE from panda WHERE 1 = 1 ");

        // init datas
        switch (dataset)
        {
            case Dataset.FetchPanda:
                {
                    await PandaData.PopulateDataTestForFetching(dbContext, FixtureInstance);
                    break;
                }
        }
    }
}

public enum Dataset
{
    Empty,
    FetchPanda
}