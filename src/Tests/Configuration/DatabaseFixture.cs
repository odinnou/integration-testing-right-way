using TestEnvironment.Docker;
using TestEnvironment.Docker.Containers.Postgres;

namespace Tests.Configuration;

public class DatabaseFixture : IDisposable
{
    private const string PostgresDatabaseName = "panda-service-test";
    private const string DbImageName = "db";
    private const string MockServerName = "mock-server-test";
    private const string MockServerImageName = "thirdparties";
    public IDockerEnvironment PostgresDatabase { get; private set; }
    public IDockerEnvironment MockServer { get; private set; }

    public DatabaseFixture()
    {
        PostgresDatabase = new DockerEnvironmentBuilder()
               .SetName(PostgresDatabaseName)
               .AddPostgresContainer(p => p with
               {
                   Name = DbImageName,
                   Ports = new Dictionary<ushort, ushort> { { 5432, 35432 } }
               })
               .Build();

        MockServer = new DockerEnvironmentBuilder()
              .SetName(MockServerName)
              .AddContainer(p => p with
              {
                  Name = MockServerImageName,

                  ImageName = "mockserver/mockserver",
                  Ports = new Dictionary<ushort, ushort> { { 1080, 1090 } }
              })
              .Build();

        // Up it.
        PostgresDatabase.UpAsync().Wait();
        MockServer.UpAsync().Wait();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            try
            {
                PostgresDatabase.DisposeAsync().AsTask().Wait();
                MockServer.DisposeAsync().AsTask().Wait();
            }
            catch
            {
                // Occurs sometimes on local dev, during debug
            }
        }
    }
}

