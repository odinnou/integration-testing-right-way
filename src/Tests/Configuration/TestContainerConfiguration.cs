using TestEnvironment.Docker;
using TestEnvironment.Docker.Containers.Postgres;

namespace Tests.Configuration;

public class TestContainerConfiguration : IDisposable
{
    public IDockerEnvironment PostgresDatabase { get; private set; }
    public IDockerEnvironment MockServer { get; private set; }

    public TestContainerConfiguration()
    {
        PostgresDatabase = new DockerEnvironmentBuilder().SetName("panda-database-test")
               .AddPostgresContainer(container => container with
               {
                   Name = "db",
                   Ports = new Dictionary<ushort, ushort> { { 5432, 35432 } }
               }).Build();

        MockServer = new DockerEnvironmentBuilder().SetName("panda-mockserver-test")
              .AddContainer(container => container with
              {
                  Name = "mockserver",
                  ImageName = "mockserver/mockserver",
                  Ports = new Dictionary<ushort, ushort> { { 1080, 1090 } }
              }).Build();

        // Up them
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

