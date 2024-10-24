using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using OpenFeature.Providers.Memory;
using WizardTest.App;
using WizardTest.OpenFeature;
using WizardTest.Testing;
using Xunit.DependencyInjection;

namespace OpenFeature.IntegrationTests;

public class OpenFeatureModuleFixture(ITestOutputHelperAccessor testOutputHelperAccessor)
    : ModuleFixture<OpenFeatureModule>(testOutputHelperAccessor, new TestConfigurationProvider(
            new Dictionary<string, string> {
                { "OpenFeatureSettings:LaunchDarkly:SdkKey", "sdk-key" }
            }),
        AddRegistrations)
{
    private static void AddRegistrations(IServiceCollection serviceCollection, IConfiguration configuration) =>
        serviceCollection
            .AddSingleton<FeatureProvider, InMemoryProvider>()
            .AddScoped<IUserContext>(sp => sp.GetRequiredService<Mock<IUserContext>>().Object)
            .AddScoped<Mock<IUserContext>>();

    public override async Task InitializeAsync()
    {
        foreach (var hostedService in ServiceProvider.GetServices<IHostedService>()) {
            await hostedService.StartAsync(CancellationToken.None);
        }
    }

    public override async Task DisposeAsync()
    {
        foreach (var hostedService in ServiceProvider.GetServices<IHostedService>()) {
            await hostedService.StopAsync(CancellationToken.None);
        }
    }
}
