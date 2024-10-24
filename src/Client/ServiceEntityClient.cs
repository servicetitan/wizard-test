using ServiceTitan.WizardTest.Client.Contracts;

namespace ServiceTitan.WizardTest.Client;

public interface IServiceEntityClient
{
    Task StoreServiceEntityAsync(ServiceEntityClientModel entity);
    Task<ServiceEntityClientModel> GetServiceEntityAsync(string id);
}

internal class ServiceEntityClient(IServiceEntityRefitApiClient apiClient) : IServiceEntityClient
{
    public async Task StoreServiceEntityAsync(ServiceEntityClientModel entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var apiResponse = await apiClient.CreateAsync(entity);
        if (!apiResponse.IsSuccessStatusCode) {
            throw new Exception($"Failed to store entity: {entity}");
        }
    }

    public async Task<ServiceEntityClientModel> GetServiceEntityAsync(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        var apiResponse = await apiClient.ReadOneAsync(id);
        if (!apiResponse.IsSuccessStatusCode || apiResponse.Content == null) {
            throw new Exception($"Failed to get entity by id: {id}");
        }

        return apiResponse.Content;
    }
}
