namespace WizardTest.App.Services;

public class UserService(IUserContext userContext, ICapitalizeUsernameFeature capitalizeUsernameFeature)
{
    public async Task<string> GetUserNameAsync() =>
        await capitalizeUsernameFeature.GetValueAsync()
            ? userContext.User?.Username.ToUpper() ?? string.Empty
            : userContext.User?.Username ?? string.Empty;
}
