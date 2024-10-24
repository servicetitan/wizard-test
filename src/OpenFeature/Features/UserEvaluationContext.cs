using OpenFeature.Model;
using WizardTest.App;

namespace WizardTest.OpenFeature;

internal class UserEvaluationContext(IUserContext userContext)
{
    private EvaluationContext? EvaluationContext { get; } = userContext.User != null
        ? EvaluationContext
            .Builder()
            .Set(nameof(userContext.User.Username), userContext.User.Username)
            .SetKind(nameof(User))
            .Build()
        : null;

    public static implicit operator EvaluationContext?(UserEvaluationContext context) =>
        context.EvaluationContext;
}
