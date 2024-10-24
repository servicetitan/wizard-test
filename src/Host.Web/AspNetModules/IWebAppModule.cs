using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using WizardTest.Core;

namespace WizardTest.Host.Web;

internal interface IWebAppModule : IModule
{
    void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration);
}
