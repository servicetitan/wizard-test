namespace WizardTest.Diagnostics;

public record StartupProbeResult(IReadOnlyCollection<StartupStepResult> StepResults)
{
    public TimeSpan Duration => new(StepResults.Sum(x => x.Duration.Ticks));
    public IReadOnlyCollection<StartupStepResult> Problems => StepResults.Where(x => x.Exception is not null).ToArray();
}

public record StartupStepResult(Type StepType, Exception? Exception, TimeSpan Duration);
