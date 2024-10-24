using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WizardTest.Core;

public abstract class ModuleBase :
    IModule
{

    public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
    }

    public IEnumerable<Type> Dependencies {
        get {
            var moduleDependencyBuilder = new ModuleDependencyBuilder();
            ConfigureDependencies(moduleDependencyBuilder);
            return moduleDependencyBuilder.Dependencies;
        }
    }

    protected virtual void ConfigureDependencies(IModuleDependencyBuilder moduleDependencyBuilder)
    {
    }

    public interface IModuleDependencyBuilder
    {
        IModuleDependencyBuilder DependOn<T>() where T : IModule, new();
    }

    private class ModuleDependencyBuilder : IModuleDependencyBuilder
    {
        private readonly List<Type> _modules = new();

        public IEnumerable<Type> Dependencies => _modules;

        public IModuleDependencyBuilder DependOn<T>() where T : IModule, new()
        {
            _modules.Add(typeof(T));
            return this;
        }
    }
}
