namespace WizardTest.OpenFeature;

public record OpenFeatureSettings(
    OpenFeatureSettings.LaunchDarklySettings? LaunchDarkly = null
)
{
    public record LaunchDarklySettings(
        string? SdkKey = null,
        TimeSpan? StartWaitTime = null
    );
}
