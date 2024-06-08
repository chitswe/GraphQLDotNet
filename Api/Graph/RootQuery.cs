using System.Reflection.Metadata.Ecma335;
using Api.Graph.Employee;
using Api.Graph.Department;
using GraphQL.Types;

namespace Api.Graph;

public class RootQuery:ObjectGraphType
{
    public RootQuery(){
        Field<EmployeeQuery>("employeeQuery").Resolve(context=>new {});
        Field<DepartmentQuery>("departmentQuery").Resolve(context=> new {});
        Field<IntGraphType>("test").Argument<EmployeeWhereInputGraphType>("where").Resolve(_=> 1);
    }
}
