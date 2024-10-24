using System.Net;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Host.Web.IntegrationTests.Controllers;

[Collection(nameof(WebHostCollection))]
public class MetricsControllerTests(WebHostFixture webHostFixture)
{
    private readonly HttpClient _client = webHostFixture.DefaultClient;
    private readonly ITestOutputHelper _output = webHostFixture.Output;

    [Fact]
    public async Task GetMetrics_ShouldReturnOk()
    {
        var message = await _client.GetAsync("/metrics");

        _output.WriteLine(await message.Content.ReadAsStringAsync());
        message.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
