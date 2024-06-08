using Api.Model;
using Api.Service;
using Api.Store;
using GraphQL;
using GraphQL.Types;
namespace Api.Graph.Department;

public class DepartmentMutation:ObjectGraphType{
    public  DepartmentMutation(DepartmentService service){
        Field<DepartmentGraphType>("createDepartment")
            .Argument<DepartmentInput>("department")
            .ResolveAsync(async context=>{
                var dto = context.GetArgument<DepartmentDto>("department");
                return await service.CreateDepartmentAsync(dto);
            });
    }
}