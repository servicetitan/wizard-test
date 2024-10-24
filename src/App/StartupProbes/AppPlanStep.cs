using WizardTest.Core;

namespace WizardTest.App;

internal class AppPlanStep : IPlanStep
{
    public Task ValidateAsync() => Task.CompletedTask;
}
