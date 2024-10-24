using Microsoft.AspNetCore.Http;
using OpenTelemetry.Trace;
using WizardTest.Diagnostics;

namespace WizardTest.Host.Web;

internal class OpenTelemetryWebModule : OpenTelemetryHostModuleBase
{
    protected override void ConfigureTracerBuilder(TracerProviderBuilder tracerProviderBuilder) =>
        tracerProviderBuilder
            .AddAspNetCoreInstrumentation(opt => {
                var cachedFilteredArray = new PathString[] { "/favicon.ico", "/metrics", "/status" };
                opt.Filter = httpContext =>
                    !cachedFilteredArray.Any(x => httpContext.Request.Path.StartsWithSegments(x));
            });
}
