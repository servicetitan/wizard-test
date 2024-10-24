using AutoFixture.Xunit2;
using FluentAssertions;
using Host.Web.IntegrationTests;
using Microsoft.Extensions.DependencyInjection;
using ServiceTitan.Platform.Diagnostics;
using ServiceTitan.WizardTest.Client;
using ServiceTitan.WizardTest.Client.Contracts;
using Xunit;

namespace WizardTest.Client.IntegrationTests;

[Collection(nameof(WebHostCollection))]
public class WizardTestClientTests
{
    private readonly IWizardTestClient _sut;

    public WizardTestClientTests(WebHostFixture webApiClientFixture)
    {
        var client = webApiClientFixture.DefaultClient;
        _sut = new WizardTestClient(client, webApiClientFixture.ServiceProvider.GetRequiredService<ILogger>());
    }

    [Theory]
    [InlineAutoData]
    public Task StoreServiceEntityAsync_NotNullEntity_ShouldNotThrow(ServiceEntityClientModel model) =>
        _sut.ServiceEntityClient.StoreServiceEntityAsync(model);

    [Theory]
    [InlineAutoData]
    public async Task GetServiceEntityAsync_DoesntExist_ShouldThrow(string id)
    {
        var act = () => _sut.ServiceEntityClient.GetServiceEntityAsync(id);

        await act.Should().ThrowAsync<Exception>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetServiceEntityAsync_InvalidId_ShouldThrow(string id)
    {
        var act = () => _sut.ServiceEntityClient.GetServiceEntityAsync(id);

        await act.Should().ThrowAsync<ArgumentException>();
    }


    [Fact]
    public async Task StoreServiceEntityAsync_NullEntity_ShouldThrow()
    {
        var act = () => _sut.ServiceEntityClient.StoreServiceEntityAsync(null!);

        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Theory]
    [InlineAutoData]
    public async Task GetServiceEntityAsync_Exists_ShouldReturn(ServiceEntityClientModel model)
    {
        await _sut.ServiceEntityClient.StoreServiceEntityAsync(model);
        var result = await _sut.ServiceEntityClient.GetServiceEntityAsync(model.Id);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(model);
    }
}
