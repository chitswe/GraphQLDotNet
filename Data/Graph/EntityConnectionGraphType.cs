using Data.Ef;
using Data.Graph;
using GraphQL.Types;
public class EntityConnectionGraphType<E,G>: ObjectGraphType<EntityConnection<E>> where E:Entity where G: EntityGraphType<E>{
    public EntityConnectionGraphType(){
        Field<NonNullGraphType<ListGraphType<NonNullGraphType<G>>>>("edges").Resolve(context=> context.Source.Edges);
        Field<NonNullGraphType<PageInfoGraphType>>("pageInfo").Resolve(context=>context.Source.PageInfo);
    }
}
