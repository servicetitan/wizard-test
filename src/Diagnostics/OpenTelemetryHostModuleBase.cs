using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using WizardTest.Core;

namespace WizardTest.Diagnostics;

public class OpenTelemetryHostModuleBase : ModuleBase
{
    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        var settings = configuration.GetSection("Diagnostics").Get<DiagnosticsSettings?>();
        services
            .AddSingleton<ApplicationInfoResourceDetector>()
            .AddOpenTelemetry()
            .WithTracing(builder => {
                builder
                    .SetSampler(new AlwaysOnSampler())
                    .AddHttpClientInstrumentation()
                    .SetResourceBuilder(ResourceBuilder
                        .CreateDefault()
                        .AddDetector(sp => sp.GetRequiredService<ApplicationInfoResourceDetector>())
                    );
                settings?.OpenTelemetry?.Tracing?.OtlpExporter?.Apply(builder);
                ConfigureTracerBuilder(builder);
            });
    }

    protected virtual void ConfigureTracerBuilder(TracerProviderBuilder tracerProviderBuilder)
    {
    }

    internal class ApplicationInfoResourceDetector(IApplicationInfo applicationInfo) : IResourceDetector
    {
        public Resource Detect() =>
            new(new Dictionary<string, object> {
                { "service.name", applicationInfo.ApplicationName },
                { "service.version", applicationInfo.Version }
            });
    }
}
