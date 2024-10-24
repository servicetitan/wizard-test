using Refit;
using ServiceTitan.WizardTest.Client.Contracts;

namespace ServiceTitan.WizardTest.Client;

internal interface IServiceEntityRefitApiClient
{
    public const string ResourcePath = "/api/service-entities";

    [Post(ResourcePath)]
    Task<IApiResponse<ServiceEntityClientModel>> CreateAsync([Body] ServiceEntityClientModel payload);

    [Get(ResourcePath + "/{key}")]
    Task<IApiResponse<ServiceEntityClientModel>> ReadOneAsync(string key);

    [Delete(ResourcePath + "/{key}")]
    Task<IApiResponse> DeleteAsync(string key);
}
