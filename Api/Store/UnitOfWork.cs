using Api.Model;
using Data.Reposiory;
using Microsoft.EntityFrameworkCore;

namespace Api.Store;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    private IEntityRepository<Department>? _departments;
    private IEntityRepository<Employee>? _employees;

    public UnitOfWork(DbContext context)
    {
        _context = context;
    }

    public IEntityRepository<Department> Departments => _departments ??= new EntityRepository<Department>(_context);
    public IEntityRepository<Employee> Employees => _employees ??= new EntityRepository<Employee>(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}