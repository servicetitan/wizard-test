using Microsoft.AspNetCore.Mvc;
using WizardTest.Diagnostics;

namespace WizardTest.Host.Web;

[ApiController]
[ApiExplorerSettings(GroupName = "Probes")]
public class StartupProbeController(IStartupProbePlan startupProbePlan) : ControllerBase
{
    [Route("startup-probe")]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var startupProbeResult = await startupProbePlan.ExecuteAsync();

        return !startupProbeResult.Problems.Any()
            ? Ok($"All probes succeeded. Duration: {startupProbeResult.Duration}{Environment.NewLine}" +
                 $"{string.Join(Environment.NewLine, startupProbeResult.StepResults.Select(x => $"{x.StepType}: {x.Duration}"))}")
            : Problem(
                $"{startupProbeResult.Problems.Count} probe(s) failed. Duration: {startupProbeResult.Duration}{Environment.NewLine}" +
                $"{string.Join(Environment.NewLine, startupProbeResult.Problems.Select(x => $"{x.StepType}: {x.Exception}"))}");
    }
}
