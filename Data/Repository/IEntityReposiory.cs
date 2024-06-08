using Data.Ef;

namespace Data.Reposiory;
public interface IEntityRepository<T> where T : Entity
{
    ValueTask<T?> GetAsync(int id);
    T? Get(int id);
    Task<List<T>> SearchAsync(IEnumerable<string>? includes=null, Dictionary<string,object>? where=null, Dictionary<string,object>[]? orderBy = null);
    Task<EntityConnection<T>> PaginateAsync(IEnumerable<string>? includes=null, Dictionary<string,object>? where=null, Dictionary<string,object>[]? orderBy = null, PaginationParameter? pagination = null);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task SaveChangesAsync();
}
