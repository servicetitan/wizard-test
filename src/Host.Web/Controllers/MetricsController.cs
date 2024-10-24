using Microsoft.AspNetCore.Mvc;
using ServiceTitan.Platform.Diagnostics.Prometheus;

namespace WizardTest.Host.Web;

[ApiController]
[ApiExplorerSettings(GroupName = "Diagnostics")]
[Route("metrics")]
[ExcludeFromCodeCoverage]
public class MetricsController(IMetricsExporter metricsExporter) : ControllerBase
{
    [HttpGet]
    public async Task ExportMetricsAsync()
    {
        Response.StatusCode = 200;
        Response.ContentType = "text/plain; version=0.0.4; charset=utf-8";
        await metricsExporter.ExportToAsync(Response.Body);
    }
}
