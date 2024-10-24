using Host.Web.IntegrationTests;
using Xunit;

namespace WizardTest.Client.IntegrationTests;

[CollectionDefinition(nameof(WebHostCollection))]
public class WebHostCollection : ICollectionFixture<WebHostFixture>
{
}
