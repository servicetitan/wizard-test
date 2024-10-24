using Microsoft.Extensions.DependencyInjection;
using ServiceTitan.Platform.BlobStorage.AzureSdk;
using WizardTest.Testing;
using Xunit.DependencyInjection;

namespace WizardTest.BlobStorage.IntegrationTests;

public class BlobStorageModuleFixture(ITestOutputHelperAccessor testOutputHelperAccessor)
    : ModuleFixture<BlobStorageModule>(testOutputHelperAccessor, new TestConfigurationProvider(
        new Dictionary<string, string> {
            { "BlobStorageSettings:ContainerName", "blob-storage-test" }
        }))
{
    public override async Task DisposeAsync()
    {
        var containerClient = await ServiceProvider.GetRequiredService<IAzureSdkBlobStorage>().GetContainerClientAsync();
        await containerClient.DeleteAsync();
    }
}
