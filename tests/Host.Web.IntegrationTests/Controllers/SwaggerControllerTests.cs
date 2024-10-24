using System.Net;
using FluentAssertions;
using Xunit;

namespace Host.Web.IntegrationTests.Controllers;

[Collection(nameof(WebHostCollection))]
public class SwaggerControllerTests(WebHostFixture webHostFixture)
{
    private readonly HttpClient _client = webHostFixture.DefaultClient;

    [Fact]
    public async Task GetSwagger_ShouldReturnOk()
    {
        var message = await _client.GetAsync("/swagger/index.html");

        message.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
