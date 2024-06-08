using Data.Ef;
using GraphQL.DataLoader;
using Microsoft.EntityFrameworkCore;
namespace Data.Ef;
public class EntityBatchDataLoader<E> : DataLoaderBase<int, E> where E : Entity
{
    private readonly DbContext _dbContext;
    public EntityBatchDataLoader(DbContext dataContext)
    {
        _dbContext = dataContext;
    }

    protected override async Task FetchAsync(IEnumerable<DataLoaderPair<int, E>> list, CancellationToken cancellationToken)
    {
        IEnumerable<int> ids = list.Select(pair => pair.Key);
        IDictionary<int, E> data = await _dbContext.Set<E>().Where(e => ids.Contains(e.Id)).ToDictionaryAsync(x => x.Id, cancellationToken);
        foreach (DataLoaderPair<int, E> entry in list)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            entry.SetResult(data.TryGetValue(entry.Key, out E? order) ? order : null);
#pragma warning restore CS8604 // Possible null reference argument.
        }
    }
}