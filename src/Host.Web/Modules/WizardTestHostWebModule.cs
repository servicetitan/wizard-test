using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WizardTest.App;
using WizardTest.BlobStorage;
using WizardTest.Core;
using WizardTest.Diagnostics;
using WizardTest.OpenFeature;

namespace WizardTest.Host.Web;

internal class WizardTestHostWebModule : ModuleBase, IWebAppModule
{
    public void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
    {
        app
            .UseRouting()
            .UseEndpoints(endpoints => { endpoints.MapControllers(); })
            .UseHealthChecks("/status", new HealthCheckOptions {
                ResponseWriter = HealthCheckResponseWriter.WriteResponse
            });
    }

    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSingleton<IApplicationInfo, WizardTestInfo>()
            .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
            .AddScoped<IUserContext, HttpContextUserContext>()
            .AddControllers()
            .AddValidationHandling();
        services
            .AddHealthChecks()
            .AddCheck<StatusHealthCheck>("General Information");
    }

    protected override void ConfigureDependencies(IModuleDependencyBuilder moduleDependencyBuilder)
    {
        moduleDependencyBuilder
            .DependOn<OpenFeatureModule>()
            .DependOn<WizardTestAppModule>()
            .DependOn<StartupProbeModule>()
            .DependOn<SwashbuckleModule>()
            .DependOn<OpenTelemetryWebModule>()
            .DependOn<DiagnosticsWebModule>()
            .DependOn<BlobStorageModule>()
            ;
    }
}
