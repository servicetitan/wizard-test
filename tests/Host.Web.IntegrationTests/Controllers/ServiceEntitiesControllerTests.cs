using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace Host.Web.IntegrationTests.Controllers;

[Collection(nameof(WebHostCollection))]
public class ServiceEntitiesControllerTests(WebHostFixture webHostFixture)
{
    private const string ControllerEndpoint = "api/service-entities";
    private readonly HttpClient _client = webHostFixture.DefaultClient;

    [Theory]
    [InlineAutoData]
    public async Task CreateEntity_ShouldBeOk(ServiceEntityApiModel entity)
    {
        var response = await _client.PostAsync(ControllerEndpoint, JsonContent.Create(entity));

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location?.ToString().Should().EndWith($"{ControllerEndpoint}/{entity.Id}");
    }

    [Theory]
    [InlineAutoData]
    public async Task GetEntity_EntityExists_ShouldBeOk(ServiceEntityApiModel entity)
    {
        await _client.PostAsync(ControllerEndpoint, JsonContent.Create(entity));

        var getResponse = await _client.GetAsync($"{ControllerEndpoint}/{entity.Id}");

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        (await getResponse.Content.ReadFromJsonAsync<ServiceEntityApiModel>()).Should().BeEquivalentTo(entity);
    }

    [Theory]
    [InlineAutoData]
    public async Task GetEntity_EntityDoesntExist_ShouldBeNotFound(string id)
    {
        var getResponse = await _client.GetAsync($"{ControllerEndpoint}/{id}");

        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineAutoData]
    public async Task DeleteEntity_EntityExists_ShouldBeOk(ServiceEntityApiModel entity)
    {
        await _client.PostAsync(ControllerEndpoint, new StringContent(JsonSerializer.Serialize(entity)));

        var deleteResponse = await _client.DeleteAsync($"{ControllerEndpoint}/{entity.Id}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineAutoData]
    public async Task DeleteEntity_EntityDoesntExist_ShouldBeOk(string id)
    {
        var deleteResponse = await _client.DeleteAsync($"{ControllerEndpoint}/{id}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
