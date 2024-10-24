using System.Diagnostics;
using ServiceTitan.Platform.Diagnostics;
using WizardTest.Core;

namespace WizardTest.Diagnostics;

internal class StartupProbePlan(IEnumerable<IPlanStep> steps, ILogger<StartupProbePlan> logger)
    : IStartupProbePlan
{
    private readonly IReadOnlyCollection<IPlanStep> _steps = steps.ToArray();

    public async Task<StartupProbeResult> ExecuteAsync()
    {
        var stepResults = await Task.WhenAll(_steps.Select(ValidateStepAsync));
        return new StartupProbeResult(stepResults);

        async Task<StartupStepResult> ValidateStepAsync(IPlanStep planStep)
        {
            // To guarantee that the task is not executed synchronously
            await Task.Yield();

            var sw = Stopwatch.StartNew();
            try {
                await planStep.ValidateAsync();
                return new StartupStepResult(planStep.GetType(), null, sw.Elapsed);
            }
            catch (Exception e) {
                logger.Error($"Startup probe {planStep.GetType()} failed", e);
                return new StartupStepResult(planStep.GetType(), e, sw.Elapsed);
            }
        }
    }
}
