using Data.Ef;
using Microsoft.EntityFrameworkCore;

namespace Data.Reposiory;

public class EntityRepository<E> : IEntityRepository<E> where E : Entity
{
    private DbContext _db;
    public EntityRepository(DbContext db)
    {
        this._db = db;
    }
    public ValueTask<E?> GetAsync(int id)
    {
        return this._db.Set<E>().FindAsync(id);
    }

    public E? Get(int id)
    {
        return this._db.Set<E>().Find(id);
    }
    public Task<List<E>> SearchAsync(IEnumerable<string>? includes=null, Dictionary<string,object>? where=null, Dictionary<string,object>[]? orderBy = null)
    {
        IQueryable<E> query = _db.Set<E>().AsQueryable();
        if (includes !=null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        if(where != null)
            query = query.Where(where);
        if(orderBy != null)
            query = query.OrderBy(orderBy);
        return query.ToListAsync();
    }

    public Task<EntityConnection<E>> PaginateAsync(IEnumerable<string>? includes=null, Dictionary<string,object>? where=null, Dictionary<string,object>[]? orderBy = null, PaginationParameter? pagination = null){
        if (pagination == null)
            pagination = new PaginationParameter(){Page=1,PageSize=80};
        IQueryable<E> query = _db.Set<E>().AsQueryable();
        if (includes !=null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        if(where != null)
            query = query.Where(where);
        return query.GetConnection<E>(pagination,orderBy);
    }

    public async Task AddAsync(E entity)
    {
        await _db.AddAsync(entity);
    }

    public void Update(E entity)
    {
        _db.Update(entity);
    }

    public void Delete(E entity)
    {
        _db.Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}