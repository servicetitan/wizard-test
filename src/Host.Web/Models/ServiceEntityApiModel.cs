using WizardTest.App;
using ServiceTitan.WizardTest.Client.Contracts;

namespace WizardTest.Host.Web;

public record ServiceEntityApiModel(string Id)
    : ServiceEntityClientModel(Id), IServiceEntity
{
    public static ServiceEntityApiModel FromInterface(IServiceEntity serviceEntity) => new(serviceEntity.Id);
}
