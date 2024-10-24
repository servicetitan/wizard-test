using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using WizardTest.Core;

namespace WizardTest.Host.Web;

public class Program
{
    public static async Task Main(string[] args) =>
        await Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => webBuilder
                .ConfigureServices((ctx, services) => services.AddRootModule<WizardTestHostWebModule>(ctx.Configuration))
                .Configure((ctx, appBuilder) => appBuilder.AddRootAppBuilder<WizardTestHostWebModule>(ctx.HostingEnvironment, ctx.Configuration)))
            .Build()
            .RunAsync();
}
