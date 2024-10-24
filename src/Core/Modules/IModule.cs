using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WizardTest.Core;

public interface IModule
{
    IEnumerable<Type> Dependencies { get; }
    void ConfigureServices(IServiceCollection services, IConfiguration configuration);
}
