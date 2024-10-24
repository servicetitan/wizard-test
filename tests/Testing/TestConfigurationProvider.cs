using Microsoft.Extensions.Configuration;

namespace WizardTest.Testing;

public interface ITestConfigurationProvider
{
    IConfiguration Get();
}

public class TestConfigurationProvider(IEnumerable<KeyValuePair<string, string>> customParameters = null)
    : ITestConfigurationProvider
{
    public IConfiguration Get()
    {
        var rr = new TestResourceReader(typeof(TestConfigurationProvider));
        var configurationBuilder = new ConfigurationBuilder()
            .AddJsonStream(rr.Read("appsettings.Test.json"));
        if (customParameters != null) {
            configurationBuilder.AddInMemoryCollection(customParameters);
        }

        configurationBuilder.AddEnvironmentVariables();
        return configurationBuilder.Build();
    }
}
