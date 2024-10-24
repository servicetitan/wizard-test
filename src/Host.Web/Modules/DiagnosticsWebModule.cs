using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using ServiceTitan.Platform.Diagnostics;
using WizardTest.Core;
using WizardTest.Diagnostics;

namespace WizardTest.Host.Web;

internal class DiagnosticsWebModule : ModuleBase, IWebAppModule
{
    public void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
    {
        var httpLoggingConfig = new HttpLoggingConfig {
            HiddenKeysInRequestLogs = { "st-apikey", "servicetitanapikey", "x-http-servicetitan-api-key" },
            EnableMetrics = true,
            Buckets = [0.005, 0.01, 0.025, 0.05, 0.1, 0.25, 0.5, 1, 2, 4, 8, 16, 32, 64, 128, 256]
        };
        app.UseRequestIdLogging();
        app.UseRequestInfoLogging(httpLoggingConfig);
        app.UseExceptionLogging(httpLoggingConfig);
    }

    protected override void ConfigureDependencies(IModuleDependencyBuilder moduleDependencyBuilder) =>
        moduleDependencyBuilder.DependOn<DiagnosticsModule>();
}
