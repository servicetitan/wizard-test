using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceTitan.Platform.Diagnostics;
using ServiceTitan.Platform.Diagnostics.Prometheus;
using WizardTest.Core;

namespace WizardTest.Diagnostics;

public class DiagnosticsModule : ModuleBase
{
    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var diagnosticsSettings = configuration.GetSection(nameof(DiagnosticsSettings)).Get<DiagnosticsSettings>();
        services
            .AddSingleton<IMetricsExporter, PrometheusMetricsExporter>()
            .AddSingleton(diagnosticsSettings?.Metrics ?? new DiagnosticsSettings.MetricsSettings())
            .AddPlatformDiagnostics((diagnostics, sp) => {
                var applicationInfo = sp.GetRequiredService<IApplicationInfo>();
                diagnostics.GlobalLogData.SetAppInfo(applicationInfo.ApplicationName, applicationInfo.Version);
                foreach (var truncatePrefix in diagnosticsSettings?.TruncatePrefixes ?? []) {
                    diagnostics.TruncateLogCategoryPrefixes.Add(truncatePrefix);
                }

                diagnostics.DefaultLoggerCategory = applicationInfo.ApplicationName;
                diagnostics
                    .EnablePrometheusReporting()
                    .AddPrometheusDotNetRuntimeMetrics();

                var settings = diagnosticsSettings?.Logging;

                if (settings?.ConsoleLoggerType == ConsoleLoggerType.Development) {
                    diagnostics.SetDevelopmentDefaultSettings();
                }
            })
            .ClearLoggingProviders()
            .AddPlatformLoggerProvider()
            ;
    }
}
