using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WizardTest.Host.Web;

internal class StatusHealthCheck(IWebHostEnvironment environment) : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new())
    {
        var assemblyName = Assembly.GetEntryAssembly()!.GetName();
        return Task.FromResult(HealthCheckResult.Healthy("Ok", new Dictionary<string, object> {
            { "Timestamp", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture) },
            { "AssemblyVersion", assemblyName.Version?.ToString() ?? "not-set" },
            { "AssemblyName", assemblyName.Name ?? "not-set" },
            { "Environment", environment.EnvironmentName }
        }));
    }
}
