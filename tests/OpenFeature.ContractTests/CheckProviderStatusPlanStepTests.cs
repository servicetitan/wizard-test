using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using WizardTest.OpenFeature;
using Xunit;

namespace OpenFeature.ContractTests;

public class CheckProviderStatusPlanStepTests(OpenFeatureModuleFixture fixture)
    : IClassFixture<OpenFeatureModuleFixture>
{
    [Fact]
    public async Task ValidateAsync_SdkKeyCorrect_ShouldNotThrow()
    {
        var planStep = fixture.ServiceProvider.GetRequiredService<CheckProviderStatusPlanStep>();

        var act = () => planStep.ValidateAsync();

        await act.Should().NotThrowAsync<Exception>();
    }
}
