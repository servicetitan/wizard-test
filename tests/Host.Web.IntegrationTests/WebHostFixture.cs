using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;
using Xunit.DependencyInjection;

namespace Host.Web.IntegrationTests;

public class WebHostFixture(WebApplicationFactory<Program> factory, ITestOutputHelperAccessor testOutputHelperAccessor)
{
    public HttpClient DefaultClient { get; } = factory.CreateClient();
    public IServiceProvider ServiceProvider { get; } = factory.Services;
    public ITestOutputHelper Output => testOutputHelperAccessor.Output;

    public HttpClient CreateClient(WebApplicationFactoryClientOptions options) => factory.CreateClient(options);
}
