using Microsoft.AspNetCore.Http;
using WizardTest.App;

namespace WizardTest.Host.Web;

internal class HttpContextUserContext : IUserContext
{
    public HttpContextUserContext(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext?.Request.Headers.TryGetValue("X-Service-User", out var username) == true) {
            var value = username.ToString();
            if (!string.IsNullOrEmpty(value)) {
                User = new User(value);
            }
        }
    }

    public User? User { get; }
}
