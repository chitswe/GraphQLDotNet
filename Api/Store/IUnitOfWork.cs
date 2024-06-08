using Api.Model;
using Data.Reposiory;

namespace Api.Store;

public interface IUnitOfWork : IDisposable
{
    IEntityRepository<Department> Departments { get; }
    IEntityRepository<Employee> Employees { get; }
    Task<int> SaveChangesAsync();
}