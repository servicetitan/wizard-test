namespace ServiceTitan.WizardTest.Client;

internal class WizardTestClientSettings
{
    public Dictionary<string, ClientSetting> Clients { get; init; } = new();
}

internal class ClientSetting
{
    public string? Endpoint { get; set; }
    public TimeSpan RequestBudget { get; set; } = TimeSpan.FromSeconds(30);
}

internal static class WizardTestClientSettingsExtensions
{
    public static ClientSetting? TryGetServiceEntityClientSettings(this WizardTestClientSettings? settings) =>
        settings?.Clients?.TryGetValue(nameof(ServiceEntityClient), out var clientSetting) == true
            ? clientSetting
            : null;
}
