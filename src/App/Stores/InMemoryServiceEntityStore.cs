using System.Collections.Concurrent;

namespace WizardTest.App;

/// <summary>
///     This store serves as stub for a port. It should be replaced with proper port adapter, i.e.
///     MongoDbServiceEntityStore
/// </summary>
public class InMemoryServiceEntityStore : IServiceEntityStore
{
    private readonly ConcurrentDictionary<string, IServiceEntity> _serviceEntities = new();

    public async Task<IServiceEntity?> FindAsync(string id) =>
        _serviceEntities.TryGetValue(id, out var result) ? result : null;

    public async Task<IServiceEntity> InsertAsync(IServiceEntity entity)
    {
        _serviceEntities.TryAdd(entity.Id, entity);
        return entity;
    }

    public async Task DeleteAsync(string id) => _serviceEntities.TryRemove(id, out var _);
}
