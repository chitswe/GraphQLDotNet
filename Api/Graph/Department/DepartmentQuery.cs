using Api.Model;
using Api.Service;
using Data;
using GraphQL;
using GraphQL.Types;


namespace Api.Graph.Department;

public class DepartmentQuery : ObjectGraphType
{
    public DepartmentQuery(DepartmentService service)
    {
        Field<DepartmentGraphType>("department")
            .Argument<IdGraphType>("id")
            .ResolveAsync(async context =>
            {
                var id = context.GetArgument<int>("id");
                return await service.GetDepartmentAsync(id);
            });
        Field<NonNullGraphType<DepartmentConnectionGraphType>>("connection")
            .Argument<PaginationInputGraphType>("pagination")
            .Argument<DepartmentWhereInputGraphType>("where")
            .Argument<ListGraphType<NonNullGraphType<DepartmentOrderByInputGraphType>>>("orderBy")
            .ResolveAsync(async context=>{
                var pagination = context.GetArgument<PaginationParameter>("pagination");
                var where = context.GetArgument<Dictionary<string, object>>("where");
                var orderBy = context.GetArgument<Dictionary<string,object>[]>("orderBy");
                return await service.PaginateDepartmentAsync(where:where, pagination:pagination, orderBy:orderBy);
            });
    }
}