using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using WizardTest.Core;

namespace WizardTest.Host.Web;

internal static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder AddRootAppBuilder<T>(
        this IApplicationBuilder applicationBuilder, IWebHostEnvironment env, IConfiguration configuration)
        where T : IWebAppModule
    {
        IModule Factory(Type type) => (IModule)Activator.CreateInstance(type)!;

        foreach (var module in KvTarjan.GetOrdered(typeof(T), Factory, m => m.Dependencies)
                     .Where(x => x is IWebAppModule)
                     .Cast<IWebAppModule>()) {
            module.ConfigureApp(applicationBuilder, env, configuration);
        }

        return applicationBuilder;
    }
}
