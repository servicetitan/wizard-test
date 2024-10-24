using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WizardTest.App.Services;
using WizardTest.Core;

namespace WizardTest.App;

public class WizardTestAppModule : ModuleBase
{
    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration) =>
        services
            .AddScoped<UserService>()
            .AddScoped<IPlanStep, AppPlanStep>()
            .AddSingleton<IServiceEntityStore, InMemoryServiceEntityStore>()
    ;

    protected override void ConfigureDependencies(IModuleDependencyBuilder deps) =>
        deps.DependOn<WizardTestCoreModule>();
}
