using OpenFeature;
using WizardTest.App;

namespace WizardTest.OpenFeature;

internal class CapitalizeUsernameFeature(UserEvaluationContext userEvaluationContext, IOpenFeatureApi api)
    : ICapitalizeUsernameFeature
{
    public const string FlagName = "capitalize-letters";
    private readonly FeatureClient _featureClient = api.Api.GetClient(context: userEvaluationContext);
    public Task<bool> GetValueAsync() => _featureClient.GetBooleanValueAsync(FlagName, true);
}
