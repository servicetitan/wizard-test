using Microsoft.Extensions.DependencyInjection;

namespace WizardTest.Testing;

public class Startup
{
    public virtual void ConfigureServices(IServiceCollection services) =>
        services.AddSingleton<ITestConfigurationProvider>(new TestConfigurationProvider());
}
