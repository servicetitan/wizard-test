using Microsoft.Extensions.Hosting;
using Prometheus;
using ServiceTitan.Platform.Diagnostics;
using WizardTest.Diagnostics;

namespace WizardTest.Host.Worker;

/// <summary>
///     Exposes metrics endpoint. To run locally you should grant permission to the port 'netsh http add urlacl
///     url=http://+:[your port]/metrics user=[your user]'
/// </summary>
internal class MetricServerWrapper(
    DiagnosticsSettings.MetricsSettings metricsSettings,
    ILogger<MetricServerWrapper> logger)
    : IHostedService
{
    private IMetricServer? _metricServer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        try {
            _metricServer = new MetricServer(metricsSettings.PrometheusScrapePort);
            _metricServer.Start();
            return Task.CompletedTask;
        }
        catch (Exception e) {
            logger.Error("Failed to start metrics http listener.", e);
            return Task.CompletedTask;
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_metricServer != null) {
            await _metricServer.StopAsync();
        }
    }
}
