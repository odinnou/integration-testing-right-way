using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Service;

namespace Tests.Configuration;

public static class HostConfiguration
{
    public static WebApplicationFactory<Program> Factory()
    {
        return new WebApplicationFactory<Program>()
        .WithWebHostBuilder(builder =>
        {
            builder
            .UseContentRoot(".")
            .UseEnvironment(AppSettings.TestEnvironment)
            .ConfigureAppConfiguration((builderContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
            })
            .ConfigureTestServices(s =>
            {
            });
        });
    }
}
