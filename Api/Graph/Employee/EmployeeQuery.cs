using Api.Model;
using Data;
using Data.Graph;
using GraphQL;
using GraphQL.Types;
using Api.Service;

namespace Api.Graph.Employee;

public class EmployeeQuery:ObjectGraphType{
    public EmployeeQuery(EmployeeService service){
        Field<EmployeeGraphType>("employee")
            .Argument<IdGraphType>("id")
            .ResolveAsync( async context=>{
                var id = context.GetArgument<int>("id");
                return await service.GetEmployeeAsync(id);
            });
        Field<NonNullGraphType<ListGraphType<NonNullGraphType<EmployeeGraphType>>>>("employees")
            .Argument<EmployeeWhereInputGraphType>("where")
            .Argument<ListGraphType<NonNullGraphType<EmployeeOrderByInputGraphType>>>("orderBy")
            .ResolveAsync(async context=>{
                var where = context.GetArgument<Dictionary<string, object>>("where");
                var orderBy = context.GetArgument<Dictionary<string,object>[]>("orderBy");
                return await service.SearchEmployeeAsync(where:where, orderBy:orderBy);
            });
        Field<NonNullGraphType<EmployeeConnectionGraphType>>("connection")
            .Argument<PaginationInputGraphType>("pagination")
            .Argument<EmployeeWhereInputGraphType>("where")
            .Argument<ListGraphType<NonNullGraphType<EmployeeOrderByInputGraphType>>>("orderBy")
            .ResolveAsync(async context=>{
                var pagination = context.GetArgument<PaginationParameter>("pagination");
                var where = context.GetArgument<Dictionary<string, object>>("where");
                var orderBy = context.GetArgument<Dictionary<string,object>[]>("orderBy");
                return await service.PaginateEmployeeAsync(where:where, pagination:pagination, orderBy:orderBy);
            });
    }
}