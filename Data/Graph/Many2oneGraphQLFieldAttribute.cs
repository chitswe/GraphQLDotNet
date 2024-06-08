using GraphQL.Types;

namespace Data.Graph;

[AttributeUsage(AttributeTargets.Property)]
public class Many2oneGraphQLFieldAttribute:Attribute
{
    public string? Name{get;private set;}
    public Type GraphType{get;private set;}
    public Type EntityType{get;private set;}
    public string? Description{get;private set;}
    public Type? InputGraphType{get; private set;}
    public Type? WhereInputGraphType{get; private set;}
    public Type? OrderByInputGraphType{get;private set;}
    public Many2oneGraphQLFieldAttribute(Type graphType, Type entityType, Type? inputGraphType=null, Type? whereInputGraphType = null, Type? orderByInputGraphType = null,string? name=null,  string? description = null){
        this.Name = name;
        this.GraphType = graphType;
        this.EntityType = entityType;
        this.Description = description;
        this.InputGraphType = inputGraphType;
        this.WhereInputGraphType = whereInputGraphType;
        this.OrderByInputGraphType = orderByInputGraphType;
    }
}
