using System.Reflection;

namespace WizardTest.Core;

public interface IApplicationInfo
{
    string ApplicationName { get; }
    string Version { get; }
}

public class AssemblyApplicationInfo(string applicationName) : IApplicationInfo
{
    public string Version { get; } = Assembly
                                         .GetEntryAssembly()
                                         ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                                         ?.InformationalVersion
                                     ?? "0.0.0";

    public string ApplicationName { get; } = applicationName;
}
