using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WizardTest.App;
using WizardTest.Core;
using WizardTest.Diagnostics;
using WizardTest.Host.Web;
using WizardTest.BlobStorage;

namespace WizardTest.Host.Worker;

internal class WizardTestWorkerHostModule : ModuleBase
{
    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration) =>
        services
            .AddSingleton<IApplicationInfo, WorkerApplicationInfo>()
            .AddHostedService<MetricServerWrapper>();

    protected override void ConfigureDependencies(IModuleDependencyBuilder moduleDependencyBuilder)
    {
        moduleDependencyBuilder
            .DependOn<WizardTestAppModule>()
            .DependOn<DiagnosticsModule>()
            .DependOn<StartupProbeModule>()
            .DependOn<BlobStorageModule>()
            ;
    }
}
