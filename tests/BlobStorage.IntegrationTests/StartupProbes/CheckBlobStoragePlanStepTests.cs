using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using WizardTest.Core;
using Xunit;

namespace WizardTest.BlobStorage.IntegrationTests.StartupProbes;

public class CheckBlobStoragePlanStepTests : IClassFixture<BlobStorageModuleFixture>
{
    private readonly CheckBlobStoragePlanStep _sut;

    public CheckBlobStoragePlanStepTests(BlobStorageModuleFixture fixture) =>
        _sut = fixture.ServiceProvider
                .GetServices(typeof(IPlanStep))
                .FirstOrDefault(x => x?.GetType() == typeof(CheckBlobStoragePlanStep))
            as CheckBlobStoragePlanStep;

    [Fact]
    public void CheckConsumePlanStep_ShouldBeRegistered() => _sut.Should().NotBeNull();

    [Fact]
    public async Task ValidateAsync_ShouldNotThrow()
    {
        var act = () => _sut!.ValidateAsync();
        await act.Should().NotThrowAsync<Exception>();
    }
}
