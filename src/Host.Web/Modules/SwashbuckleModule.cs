using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using WizardTest.Core;

namespace WizardTest.Host.Web;

internal class SwashbuckleModule : ModuleBase, IWebAppModule
{
    public void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration) =>
        app
            .UseSwagger()
            .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WizardTest Host Web"));

    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration) =>
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "WizardTest Host Web", Version = "v1", Description = "Put your api info here"});
                options.TagActionsBy(api => new[] { api.GroupName });
                options.DocInclusionPredicate((_, _) => true);
            });

    protected override void ConfigureDependencies(IModuleDependencyBuilder moduleDependencyBuilder) =>
        moduleDependencyBuilder
            .DependOn<DiagnosticsWebModule>();
}
