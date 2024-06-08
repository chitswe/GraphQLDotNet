using System.Reflection;
using Data.Ef;
using GraphQL.Types;

namespace Data.Graph;

public class EntityOrderByInputGraphType<E>: InputObjectGraphType where E:Entity
{
    private TypeInfo _typeInfo;
    public EntityOrderByInputGraphType(){
        var type = typeof(E);
        _typeInfo = type.GetTypeInfo();
        Field(type:typeof(IntGraphType),name: "Id");
        init();
    }

    private void init(){
        var entityGraphTypeAttribute = this._typeInfo.GetCustomAttribute<EntityGraphTypeAttribute>();
        var only = entityGraphTypeAttribute?.Only ?? new string[0];
        var except = entityGraphTypeAttribute?.Except ?? new string[0];
        foreach (PropertyInfo property in this._typeInfo.GetProperties())
        {
            if (property.Name == "Id")
                continue;
            var graphQLFieldAttribute = property.GetCustomAttribute<GraphQLFieldAttribute>();
            var m2OneFieldAttribute = property.GetCustomAttribute<Many2oneGraphQLFieldAttribute>();
            if (this.Fields.Find(property.Name) != null) { }            
            else if(property.PropertyType.IsAssignableTo(typeof(Entity))){continue;}
            else if (graphQLFieldAttribute != null) { }
            else if (m2OneFieldAttribute != null) { }
            else if (only.Contains(property.Name)) { }
            else if (!except.Contains(property.Name)) { }
            else
                continue;
            var fieldName = m2OneFieldAttribute?.Name??graphQLFieldAttribute?.Name??property.Name;
            if (m2OneFieldAttribute != null)
            {
                if(m2OneFieldAttribute.OrderByInputGraphType != null)
                    Field(type: m2OneFieldAttribute.OrderByInputGraphType, name:fieldName);
            }else{
                Field(type:typeof(IntGraphType), name:fieldName);
            }
        }
    }
}
