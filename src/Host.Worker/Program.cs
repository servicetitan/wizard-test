using Microsoft.Extensions.Hosting;
using WizardTest.Core;

namespace WizardTest.Host.Worker;

public static class Program
{
    private static async Task Main(params string[] args) =>
        await Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder(args)
            .ConfigureServices((ctx, services)
                => services.AddRootModule<WizardTestWorkerHostModule>(ctx.Configuration))
            .Build().RunAsync();
}
