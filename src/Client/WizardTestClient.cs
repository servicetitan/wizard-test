using Refit;
using ServiceTitan.Platform.Diagnostics;

namespace ServiceTitan.WizardTest.Client;

public interface IWizardTestClient
{
    IServiceEntityClient ServiceEntityClient { get; }
}

internal class WizardTestClient(IServiceEntityClient serviceEntityClient) : IWizardTestClient
{
    /// <summary>
    /// DI constructor
    /// </summary>
    /// <param name="clientFactory"></param>
    /// <param name="logger"></param>
    public WizardTestClient(IHttpClientFactory clientFactory, ILogger logger)
        : this(
            clientFactory.CreateClient(nameof(ServiceEntityClient)),
            logger)
    {
    }

    public WizardTestClient(HttpClient serviceEntityHttpClient, ILogger logger)
        : this(CreateServiceEntityClient(serviceEntityHttpClient, logger))
    {
    }

    public IServiceEntityClient ServiceEntityClient { get; } = serviceEntityClient;

    private static IServiceEntityClient CreateServiceEntityClient(HttpClient serviceEntityHttpClient, ILogger logger)
        => new ServiceEntityClient(new ServiceEntityRefitApiClientLoggingDecorator(
            RestService.For<IServiceEntityRefitApiClient>(serviceEntityHttpClient),
            logger.CreateLogger<ServiceEntityRefitApiClientLoggingDecorator>()));
}
