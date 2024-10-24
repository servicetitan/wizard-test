using Microsoft.Extensions.Hosting;

namespace WizardTest.OpenFeature;

internal class OpenFeatureLifetime(IOpenFeatureApi openFeatureApi) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken) => openFeatureApi.InitializeAsync(cancellationToken);

    public Task StopAsync(CancellationToken cancellationToken) => openFeatureApi.ShutdownAsync();
}
