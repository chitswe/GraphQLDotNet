using GraphQL.Types;

namespace Data.Graph;

[AttributeUsage(AttributeTargets.Property)]
public class GraphQLFieldAttribute:Attribute
{
    public string? Name{get;private set;}
    public Type? GraphType{get;private set;}
    public string? Description{get;private set;}
    public GraphQLFieldAttribute(string? name=null, Type? graphqlType=null, string? description = null){
        this.Name = name;
        this.GraphType = graphqlType;
        this.Description = description;
    }
}
