using ServiceTitan.Platform.Diagnostics;

namespace ServiceTitan.WizardTest.Client;

internal static class RefitLogOptions
{
    public static LogOptions Instance { get; } = new() {
        LogException = true,
        LogSuccess = true,
        AsActivitySource = true,
        ExceptionLogLevel = LogLevel.Error
    };
}
