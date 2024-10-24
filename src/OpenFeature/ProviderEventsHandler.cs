using OpenFeature.Model;
using ServiceTitan.Platform.Diagnostics;

namespace WizardTest.OpenFeature;

internal class ProviderEventsHandler(ILogger<ProviderEventsHandler> logger)
{
    public void OnProviderError(ProviderEventPayload? eventDetails) =>
        logger.Error($"Provider {eventDetails?.ProviderName} error. Message: {eventDetails?.Message}, ErrorType: {eventDetails?.ErrorType}");

    public void OnProviderReady(ProviderEventPayload? eventDetails) =>
        logger.Info($"Provider {eventDetails?.ProviderName} is ready. Message: {eventDetails?.Message}");

    public void OnProviderStale(ProviderEventPayload? eventDetails) =>
        logger.Info($"Provider {eventDetails?.ProviderName} stale. Message: {eventDetails?.Message}");

    public void OnProviderConfigurationChanged(ProviderEventPayload? eventDetails) =>
        logger.Info($"Provider {eventDetails?.ProviderName} configuration changed. Message: {eventDetails?.Message}");
}
