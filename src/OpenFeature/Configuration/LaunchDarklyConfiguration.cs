using LaunchDarkly.Logging;
using LaunchDarkly.Sdk.Server;
using ServiceTitan.Platform.Diagnostics;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

namespace WizardTest.OpenFeature;

[LogCategory("OpenFeatureModule.LaunchDarkly")]
internal class LaunchDarklyConfiguration
{
    public LaunchDarklyConfiguration(OpenFeatureSettings settings, ILoggerFactory loggerFactory)
    {
        var ldSettings = settings.LaunchDarkly;
        if (ldSettings == null) {
            Config = null;
            return;
        }

        var builder = Configuration.Builder(ldSettings.SdkKey);
        if (ldSettings.StartWaitTime.HasValue) {
            builder.StartWaitTime(ldSettings.StartWaitTime.Value);
        }

        builder.Logging(Logs.CoreLogging(loggerFactory));

        Config = builder.Build();
    }

    public Configuration? Config { get; }
}
