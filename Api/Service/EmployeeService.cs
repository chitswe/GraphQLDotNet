using Api.Model;
using Api.Store;
using Data;

namespace Api.Service;

public class EmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    public EmployeeService(IUnitOfWork unitOfWork){
        this._unitOfWork = unitOfWork;
    }

    public  ValueTask<Employee?> GetEmployeeAsync(int id){
        return _unitOfWork.Employees.GetAsync(id);
    }

    public Task<List<Employee>> SearchEmployeeAsync(IEnumerable<string>? includes=null, Dictionary<string,object>? where=null, Dictionary<string,object>[]? orderBy = null){
        return _unitOfWork.Employees.SearchAsync(includes, where, orderBy);
    }

    public Task<EntityConnection<Employee>> PaginateEmployeeAsync(IEnumerable<string>? includes=null, Dictionary<string,object>? where=null, Dictionary<string,object>[]? orderBy = null, PaginationParameter? pagination = null){
        return _unitOfWork.Employees.PaginateAsync(includes, where, orderBy, pagination);
    }

}
