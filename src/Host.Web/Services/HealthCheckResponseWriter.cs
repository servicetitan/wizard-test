using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WizardTest.Host.Web;

internal static class HealthCheckResponseWriter
{
    public static Task WriteResponse(HttpContext context, HealthReport healthReport)
    {
        context.Response.ContentType = "application/json; charset=utf-8";

        return context.Response.WriteAsync(JsonSerializer.Serialize(healthReport, new JsonSerializerOptions {
            WriteIndented = true
        }));
    }
}
