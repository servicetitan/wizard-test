using Refit;
using ServiceTitan.Platform.Diagnostics;
using ServiceTitan.WizardTest.Client.Contracts;

namespace ServiceTitan.WizardTest.Client;

internal class ServiceEntityRefitApiClientLoggingDecorator(
    IServiceEntityRefitApiClient apiClient,
    ILogger<ServiceEntityRefitApiClientLoggingDecorator> logger)
    : IServiceEntityRefitApiClient
{
    public Task<IApiResponse<ServiceEntityClientModel>> CreateAsync(ServiceEntityClientModel payload) =>
        logger.LogActionAsync<IApiResponse<ServiceEntityClientModel>>(
            () => apiClient.CreateAsync(payload),
            RefitLogOptions.Instance);

    public Task<IApiResponse<ServiceEntityClientModel>> ReadOneAsync(string key) =>
        logger.LogActionAsync<IApiResponse<ServiceEntityClientModel>>(
            () => apiClient.ReadOneAsync(key),
            RefitLogOptions.Instance);

    public Task<IApiResponse> DeleteAsync(string key) =>
        logger.LogActionAsync<IApiResponse>(() => apiClient.DeleteAsync(key), RefitLogOptions.Instance);
}
