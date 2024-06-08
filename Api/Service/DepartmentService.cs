using Api.Graph.Department;
using Api.Model;
using Api.Store;
using Data;

namespace Api.Service;

public class DepartmentService
{
    private readonly IUnitOfWork _unitOfWork;
    public DepartmentService(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }

    public ValueTask<Department?> GetDepartmentAsync(int id)
    {
        return _unitOfWork.Departments.GetAsync(id);
    }

    public Task<List<Department>> SearchDepartmentAsync(IEnumerable<string>? includes = null, Dictionary<string, object>? where = null, Dictionary<string, object>[]? orderBy = null)
    {
        return _unitOfWork.Departments.SearchAsync(includes, where, orderBy);
    }

    public Task<EntityConnection<Department>> PaginateDepartmentAsync(IEnumerable<string>? includes = null, Dictionary<string, object>? where = null, Dictionary<string, object>[]? orderBy = null, PaginationParameter? pagination = null)
    {
        return _unitOfWork.Departments.PaginateAsync(includes, where, orderBy, pagination);
    }

    public async Task<Department> CreateDepartmentAsync(DepartmentDto dto)
    {
        var department = new Department() { Name = dto.Name };
        await this._unitOfWork.Departments.AddAsync(department);
        await this._unitOfWork.SaveChangesAsync();
        return department;
    }

}
