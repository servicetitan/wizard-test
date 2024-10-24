using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceTitan.Platform.Diagnostics;
using ServiceTitan.Platform.Diagnostics.Testing;
using WizardTest.Core;
using Xunit;
using Xunit.Abstractions;
using Xunit.DependencyInjection;

namespace WizardTest.Testing;

public class ModuleFixture<T> : IAsyncLifetime where T : IModule
{
    private readonly Lazy<ServiceProvider> _serviceProviderLazy;
    private readonly ITestOutputHelperAccessor _testOutputHelperAccessor;

    protected ModuleFixture(
        ITestOutputHelperAccessor testOutputHelperAccessor,
        ITestConfigurationProvider testConfigurationProvider = null,
        Action<IServiceCollection, IConfiguration> registrationAction = null)
    {
        testConfigurationProvider ??= new TestConfigurationProvider();
        _testOutputHelperAccessor = testOutputHelperAccessor;

        _serviceProviderLazy = new Lazy<ServiceProvider>(() => {
            var configuration = testConfigurationProvider.Get();
            var serviceCollection = new ServiceCollection()
                .AddLogging()
                .AddRootModule<T>(configuration)
                .AddPlatformDiagnostics(cfg => cfg.AddXunitTestOutputAccessor(() => _testOutputHelperAccessor.Output))
                .ClearLoggingProviders()
                .AddPlatformLoggerProvider();
            registrationAction?.Invoke(serviceCollection, configuration);
            return serviceCollection.BuildServiceProvider();
        });
    }

    public ServiceProvider ServiceProvider => _serviceProviderLazy.Value;
    public ITestOutputHelper TestOutputHelper => _testOutputHelperAccessor.Output;

    public virtual async Task InitializeAsync() { }

    public virtual async Task DisposeAsync() => await ServiceProvider.DisposeAsync();
}
