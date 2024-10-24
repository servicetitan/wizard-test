using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceTitan.Platform.Diagnostics;

namespace ServiceTitan.WizardTest.Client;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWizardTestClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var allClientsSettings = configuration.GetSection(nameof(WizardTestClientSettings))
            .Get<WizardTestClientSettings>();
        ArgumentNullException.ThrowIfNull(allClientsSettings);
        var clientSetting = allClientsSettings.TryGetServiceEntityClientSettings();
        ArgumentNullException.ThrowIfNull(clientSetting);
        ArgumentException.ThrowIfNullOrEmpty(clientSetting.Endpoint);

        var retryWithinTimeoutPolicy = DefaultPolicies.RetryWithinTimeoutPolicy<HttpResponseMessage>(clientSetting.RequestBudget);

        services.AddHttpClient(nameof(ServiceEntityClient),
                client => { client.BaseAddress = new Uri(clientSetting.Endpoint); })
            .AddPolicyHandler(retryWithinTimeoutPolicy);
        services.AddSingleton<IWizardTestClient, WizardTestClient>(sp =>
            new WizardTestClient(sp.GetRequiredService<IHttpClientFactory>(), sp.GetRequiredService<ILogger>()));

        return services;
    }
}
