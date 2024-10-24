namespace WizardTest.App;

public interface IServiceEntityStore
{
    Task<IServiceEntity?> FindAsync(string id);
    Task<IServiceEntity> InsertAsync(IServiceEntity entity);
    Task DeleteAsync(string id);
}
