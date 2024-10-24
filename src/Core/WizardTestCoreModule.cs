using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WizardTest.Core;

public class WizardTestCoreModule : ModuleBase
{
    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration) =>
        services
            .AddSingleton(TimeProvider.System);

    protected override void ConfigureDependencies(IModuleDependencyBuilder moduleDependencyBuilder)
    {
    }
}
