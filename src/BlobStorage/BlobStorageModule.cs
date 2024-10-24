using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceTitan.Platform.BlobStorage.Azure;
using ServiceTitan.Platform.BlobStorage.AzureSdk;
using WizardTest.App;
using WizardTest.Core;

namespace WizardTest.BlobStorage;

public class BlobStorageModule : ModuleBase
{
    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(nameof(BlobStorageSettings)).Get<BlobStorageSettings>();
        if (settings == null) {
            throw new NotSupportedException("AzureBlobStorageSettings is absent");
        }

        services
            .AddScoped<IPlanStep, CheckBlobStoragePlanStep>()
            .AddSingleton<IAzureBlobStorageSettings>((AzureSdkBlobStorageSettings)settings)
            .AddSingleton<IAzureSdkBlobStorage, AzureSdkBlobStorage>();
    }

    protected override void ConfigureDependencies(IModuleDependencyBuilder moduleDependencyBuilder) =>
        moduleDependencyBuilder.DependOn<WizardTestAppModule>();
}
