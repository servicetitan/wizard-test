using Microsoft.Extensions.DependencyInjection;

namespace Host.Web.IntegrationTests;

public class Startup : WizardTest.Testing.Startup
{
    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        WebHostClientFactory.AddRegistration(services);
    }
}
