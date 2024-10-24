using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WizardTest.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRootModule<T>(
        this IServiceCollection serviceCollection, IConfiguration configuration)
        where T : IModule
    {
        IModule Factory(Type type) => (IModule)Activator.CreateInstance(type)!;

        foreach (var module in KvTarjan.GetOrdered(typeof(T), Factory, m => m.Dependencies)) {
            module.ConfigureServices(serviceCollection, configuration);
        }

        return serviceCollection;
    }
}
