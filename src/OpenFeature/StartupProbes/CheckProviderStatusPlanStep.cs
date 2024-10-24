using OpenFeature.Constant;
using WizardTest.Core;

namespace WizardTest.OpenFeature;

internal class CheckProviderStatusPlanStep(IOpenFeatureApi openFeatureApi, OpenFeatureSettings openFeatureSettings)
    : IPlanStep
{
    public async Task ValidateAsync()
    {
        var initializationDelay = openFeatureSettings.LaunchDarkly?.StartWaitTime ?? TimeSpan.FromSeconds(1);
        var providerStatus = openFeatureApi.Api.GetClient().ProviderStatus;
        if (providerStatus != ProviderStatus.Ready) {
            await Task.Delay(initializationDelay.Add(TimeSpan.FromSeconds(Random.Shared.NextDouble())));
            providerStatus = openFeatureApi.Api.GetClient().ProviderStatus;
        }

        if (providerStatus != ProviderStatus.Ready) {
            throw new Exception($"Feature flag provider is not in ready state. Provider status: {providerStatus}");
        }
    }
}
