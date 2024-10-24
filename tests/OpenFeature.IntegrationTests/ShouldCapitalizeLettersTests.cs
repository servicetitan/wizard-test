using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OpenFeature.Providers.Memory;
using WizardTest.App;
using WizardTest.OpenFeature;
using Xunit;

namespace OpenFeature.IntegrationTests;

public class CapitalizeUsernameFeatureTests : IClassFixture<OpenFeatureModuleFixture>, IAsyncLifetime
{
    private readonly InMemoryProvider _inMemoryProvider;
    private readonly IServiceProvider _serviceProvider;

    public CapitalizeUsernameFeatureTests(OpenFeatureModuleFixture fixture)
    {
        _serviceProvider = fixture.ServiceProvider.CreateScope().ServiceProvider;
        _inMemoryProvider = (InMemoryProvider)_serviceProvider.GetRequiredService<FeatureProvider>();
    }

    public async Task InitializeAsync() => await _inMemoryProvider.UpdateFlagsAsync(
        new Dictionary<string, Flag> {
            {
                CapitalizeUsernameFeature.FlagName,
                new Flag<bool>(
                    new Dictionary<string, bool> { { "Enabled", true }, { "Disabled", false } },
                    "Disabled",
                    ctx => {
                        return ctx.GetValue(nameof(User.Username)).AsString switch {
                            "id_with_true_value" => "Enabled",
                            "id_with_false_value" => "Disabled",
                            _ => "Disabled"
                        };
                    })
            }
        });


    public Task DisposeAsync() => Task.CompletedTask;

    [Theory]
    [InlineData("id_with_true_value", true)]
    [InlineData("id_with_false_value", false)]
    [InlineData("should use default value", false)]
    public async Task GetAsync_Check(string username, bool expectedFeatureValue)
    {
        _serviceProvider
            .GetRequiredService<Mock<IUserContext>>()
            .Setup(x => x.User)
            .Returns(new User(username));
        var features = _serviceProvider.GetRequiredService<ICapitalizeUsernameFeature>();

        var result = await features.GetValueAsync();

        result.Should().Be(expectedFeatureValue);
    }
}
