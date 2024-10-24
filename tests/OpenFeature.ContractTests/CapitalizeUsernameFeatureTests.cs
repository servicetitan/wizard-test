using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ServiceTitan.Platform.Diagnostics;
using WizardTest.App;
using WizardTest.OpenFeature;
using Xunit;

namespace OpenFeature.ContractTests;

public class CapitalizeUsernameFeatureTests(OpenFeatureModuleFixture fixture)
    : IClassFixture<OpenFeatureModuleFixture>
{
    private readonly Mock<ILogger<OpenFeatureApi>> _loggerMock = fixture.ServiceProvider.GetRequiredService<Mock<ILogger<OpenFeatureApi>>>();
    private readonly Mock<IUserContext> _userContextMock = fixture.ServiceProvider.GetRequiredService<Mock<IUserContext>>();
    private readonly ICapitalizeUsernameFeature _capitalizeUsernameFeature = fixture.ServiceProvider.GetRequiredService<ICapitalizeUsernameFeature>();

    [Fact]
    public async Task GetAsync_FlagExists_ResolveShouldntHaveErrors()
    {
        const string username = "some-name";
        var logEvents = new List<LogEvent>();

        _loggerMock.Setup(x => x.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
        _loggerMock.Setup(x => x.Log(It.IsAny<LogEvent>()))
            .Callback<LogEvent>(x => logEvents.Add(x));
        _userContextMock
            .Setup(x => x.User)
            .Returns(new User(username));

        await _capitalizeUsernameFeature.GetValueAsync();

        logEvents.Should().NotContain(x=>x.Level == LogLevel.Error);
    }
}
