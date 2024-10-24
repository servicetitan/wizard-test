using ServiceTitan.Platform.Diagnostics;

namespace WizardTest.Diagnostics;

public record DiagnosticsSettings(
    string[]? TruncatePrefixes = null,
    DiagnosticsSettings.MetricsSettings? Metrics = null,
    DiagnosticsSettings.OpenTelemetrySettings? OpenTelemetry = null,
    DiagnosticsSettings.LoggingSettings? Logging = null
)
{
    public record OpenTelemetrySettings(
        TracingSettings? Tracing = null
    );

    public record TracingSettings(
        OtlpTracingExporterSettings? OtlpExporter = null
    );

    public record OtlpTracingExporterSettings(
        string? Endpoint = null,
        bool ConsoleExporterEnabled = false
    );

    public record LoggingSettings(
        ConsoleLoggerType? ConsoleLoggerType = null
    );

    public record MetricsSettings(
        int PrometheusScrapePort = 5000
    );
}
