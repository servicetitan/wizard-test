namespace WizardTest.Diagnostics;

public interface IStartupProbePlan
{
    Task<StartupProbeResult> ExecuteAsync();
}
