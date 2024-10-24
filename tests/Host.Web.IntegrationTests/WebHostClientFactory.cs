using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceTitan.Platform.Diagnostics;
using ServiceTitan.Platform.Diagnostics.Testing;
using Xunit.DependencyInjection;

namespace Host.Web.IntegrationTests;

public class WebHostClientFactory(ITestOutputHelperAccessor testOutputHelperAccessor) : WebApplicationFactory<Program>
{
    public static IServiceCollection AddRegistration(IServiceCollection services) =>
        services.AddSingleton<WebApplicationFactory<Program>, WebHostClientFactory>();

    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder
            .ConfigureAppConfiguration(configBuilder =>
                configBuilder.AddConfiguration(new TestConfigurationProvider().Get()))
            .ConfigureServices(services =>
                services.AddPlatformDiagnostics(x => x.AddDevTestOutput(testOutputHelperAccessor.Output!)));

    protected override void ConfigureClient(HttpClient client)
    {
        base.ConfigureClient(client);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
}
