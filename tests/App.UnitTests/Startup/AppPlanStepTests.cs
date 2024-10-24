using FluentAssertions;
using Xunit;

namespace WizardTest.App.UnitTests.Startup;

public class AppPlanStepTests
{
    private readonly AppPlanStep _sut = new();

    [Fact]
    public async Task ValidateAsync_WhenCalled_ShouldNotThrow()
    {
        var act = () => _sut.ValidateAsync();

        await act.Should().NotThrowAsync();
    }
}
