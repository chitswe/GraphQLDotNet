using GraphQL.Types;
using Api.Graph.Department;
namespace Api.Graph;

public class RootMutation : ObjectGraphType
{
    public RootMutation()
    {
        Field<IdGraphType>("id").Resolve((context) => 1);
        Field<DepartmentMutation>("departmentMutation").Resolve(context=> new {});
    }
}
