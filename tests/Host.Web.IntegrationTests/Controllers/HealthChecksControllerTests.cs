using System.Net;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Host.Web.IntegrationTests.Controllers;

[Collection(nameof(WebHostCollection))]
public class HealthChecksControllerTests(WebHostFixture webHostFixture)
{
    private readonly HttpClient _client = webHostFixture.DefaultClient;
    private readonly ITestOutputHelper _output = webHostFixture.Output;

    [Fact]
    public async Task GetStatus_ShouldReturnOk()
    {
        var statusResponse = await _client.GetAsync("/status");

        _output.WriteLine(await statusResponse.Content.ReadAsStringAsync());
        statusResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
