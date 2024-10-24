using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceTitan.Platform.Diagnostics;
using ServiceTitan.WizardTest.Client;
using WizardTest.Testing;
using Xunit;

namespace WizardTest.Client.UnitTests;

public class ServiceCollectionExtensionsTests
{
    [Theory]
    [InlineData("Data.AddWizardTestClient_HasSettings_ShouldRegisterClient.json")]
    public void AddWizardTestClient_HasSettings_ShouldRegisterClient(string resourceName)
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddJsonStream(new TestResourceReader(GetType()).Read(resourceName))
            .Build();

        var sp = services
            .AddSingleton(NullLogger.Instance)
            .AddWizardTestClient(configuration)
            .BuildServiceProvider();

        var webApplicationClient = sp.GetRequiredService<IWizardTestClient>();

        webApplicationClient.Should().NotBeNull();
        webApplicationClient.ServiceEntityClient.Should().NotBeNull();
    }

    [Fact]
    public void AddWizardTestClient_NoSettings_ShouldThrow()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        Action act = () => services.AddSingleton(NullLogger.Instance).AddWizardTestClient(configuration);

        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData("Data.AddWizardTestClient_NoServiceEntityClientSettings_ShouldThrow.json")]
    public void AddWizardTestClient_NoServiceEntityClientSettings_ShouldThrow(string resourceName)
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        Action act = () => services.AddSingleton(NullLogger.Instance).AddWizardTestClient(configuration);

        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData("Data.AddWizardTestClient_NoEndpoint_ShouldThrow.json")]
    public void AddWizardTestClient_NoEndpoint_ShouldThrow(string resourceName)
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        Action act = () => services.AddSingleton(NullLogger.Instance).AddWizardTestClient(configuration);

        act.Should().Throw<ArgumentException>();
    }
}
