using LaunchDarkly.OpenFeature.ServerProvider;
using LaunchDarkly.Sdk.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenFeature;
using WizardTest.App;
using WizardTest.Core;

namespace WizardTest.OpenFeature;

public class OpenFeatureModule : ModuleBase
{
    public override void ConfigureServices(IServiceCollection services, IConfiguration configuration) =>
        services
//These registrations demonstrate how to integrate your app with feature flag
            .AddScoped<UserEvaluationContext>()
            .AddScoped<ICapitalizeUsernameFeature, CapitalizeUsernameFeature>()
//These registrations are necessary for this module to work
            .AddSingleton(configuration.GetSection(nameof(OpenFeatureSettings)).Get<OpenFeatureSettings>() ??
                          throw new NotSupportedException("OpenFeatureSettings configuration section is absent"))
            .AddSingleton<LaunchDarklyConfiguration>()
            .AddSingleton<Configuration>(sp => sp.GetService<LaunchDarklyConfiguration>()?.Config ??
                                               throw new NotSupportedException("OpenFeatureSettings.LaunchDarkly configuration section is absent"))
            .AddSingleton<IOpenFeatureApi, OpenFeatureApi>()
            .AddSingleton<ProviderEventsHandler>()
            .AddSingleton<CheckProviderStatusPlanStep>()
            .AddSingleton<FeatureProvider, Provider>()
            .AddHostedService<OpenFeatureLifetime>();
}
