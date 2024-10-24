using OpenTelemetry.Exporter;
using OpenTelemetry.Trace;

namespace WizardTest.Diagnostics;

public static class OtlpTracingExporterSettingsExtensions
{
    public static void Apply(this DiagnosticsSettings.OtlpTracingExporterSettings settings,
        TracerProviderBuilder builder)
    {
        var endpoint =
            settings.Endpoint
            ?? Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_TRACES_ENDPOINT")
            ?? Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT");
        var protocol =
            Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_TRACES_PROTOCOL")
            ?? Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_PROTOCOL");
        if (endpoint != null) {
            builder.AddOtlpExporter(options => {
                options.Endpoint = new Uri(endpoint);
                options.Protocol = protocol switch {
                    null => OtlpExportProtocol.Grpc,
                    "http/protobuf" => OtlpExportProtocol.HttpProtobuf,
                    "grpc" => OtlpExportProtocol.Grpc,
                    _ => OtlpExportProtocol.Grpc
                };
            });
        }

        if (settings.ConsoleExporterEnabled) {
            builder.AddConsoleExporter();
        }
    }
}
