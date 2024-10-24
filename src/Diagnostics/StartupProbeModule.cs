using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WizardTest.Core;

namespace WizardTest.Diagnostics;

public class StartupProbeModule : ModuleBase
{
    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration) =>
        services.AddScoped<IStartupProbePlan, StartupProbePlan>();
}
