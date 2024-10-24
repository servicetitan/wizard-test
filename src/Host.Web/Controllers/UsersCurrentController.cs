using Microsoft.AspNetCore.Mvc;
using WizardTest.App.Services;

namespace WizardTest.Host.Web;

[ApiController]
[Route("api/users/current")]
[ExcludeFromCodeCoverage]
public class UsersCurrentController(UserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<string>> GetCurrentUserName([FromHeader(Name = "X-Service-User")] string userName) =>
        await userService.GetUserNameAsync();
}
