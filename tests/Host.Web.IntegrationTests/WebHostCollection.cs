using Xunit;

namespace Host.Web.IntegrationTests;

[CollectionDefinition(nameof(WebHostCollection))]
public class WebHostCollection : ICollectionFixture<WebHostFixture>
{
}
