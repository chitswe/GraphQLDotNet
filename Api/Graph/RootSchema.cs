using GraphQL.Types;

namespace Api.Graph;


public class RootSchema:Schema
{
    public RootSchema(IServiceProvider provider, RootQuery query, RootMutation mutation)
        : base(provider)
    {
        Query = query;
        Mutation = mutation;
    }
}
