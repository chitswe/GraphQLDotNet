using System.Reflection;
using Data.Ef;
using GraphQL.Types;

namespace Data.Graph;

public class EntityWhereInputGraphType<E,W> : InputObjectGraphType where E : Entity where W:EntityWhereInputGraphType<E,W>
{

    private TypeInfo _typeInfo;
    public EntityWhereInputGraphType()
    {

        Type type = typeof(E);
        _typeInfo = type.GetTypeInfo();
        init();
    }
    private void init(){
        var entityGraphTypeAttribute = this._typeInfo.GetCustomAttribute<EntityGraphTypeAttribute>();
        var only = entityGraphTypeAttribute?.Only ?? new string[0];
        var except = entityGraphTypeAttribute?.Except ?? new string[0];
        foreach(var property in _typeInfo.GetProperties()){
            if (only.Contains(property.Name)) { }
            else if (!except.Contains(property.Name)) { }
            else
                continue;
            this.PopulateFilter(property);
        }        
        Field(type:typeof(ListGraphType<NonNullGraphType<W>>), name:"and");
        Field(type:typeof(ListGraphType<NonNullGraphType<W>>), name:"or");
    }
    private void PopulateFilter(PropertyInfo property)
    {
        var graphQLFieldAttribute = property.GetCustomAttribute<GraphQLFieldAttribute>();
        var m2OneFieldAttribute = property.GetCustomAttribute<Many2oneGraphQLFieldAttribute>();
        var fieldName = graphQLFieldAttribute?.Name ?? property.Name;   
        if(m2OneFieldAttribute?.WhereInputGraphType != null)
            Field(type:m2OneFieldAttribute.WhereInputGraphType, name:m2OneFieldAttribute?.Name??fieldName);
        if(property.PropertyType.IsAssignableTo(typeof(Entity))) {            
            return;
        }
        Type graphType = this.ResolveGraphType<E,W>(property);
        Type? enumGraphType = Helper.IsEnumerationGraphType(graphType);
        if (enumGraphType != null)
        {
            Field(type: enumGraphType, name: fieldName);
            Field(type: enumGraphType, name: fieldName + "_Not");
            Field(type: Helper.BuildListGraphType(Helper.BuildNonNullableGraphType(enumGraphType)), name: fieldName + "_In");
            Field(type: Helper.BuildListGraphType(Helper.BuildNonNullableGraphType(enumGraphType)), name: fieldName + "_NotIn");
            Field<BooleanGraphType>(fieldName + "_IsBlank");
        }
        else if (property.Name == "Id")
        {
            Field<IdGraphType>(fieldName);
            Field<IdGraphType>(fieldName + "_Not");
            Field<ListGraphType<NonNullGraphType<IdGraphType>>>(fieldName + "_In");
            Field<ListGraphType<NonNullGraphType<IdGraphType>>>(fieldName + "_NotIn");
            Field<IdGraphType>(fieldName + "_Lt");
            Field<IdGraphType>(fieldName + "_Lte");
            Field<IdGraphType>(fieldName + "_Gt");
            Field<IdGraphType>(fieldName + "_Gte");
            Field<BooleanGraphType>(fieldName + "_IsBlank");
        }
        else if (graphType == typeof(StringGraphType) || graphType == typeof(NonNullGraphType<StringGraphType>))
        {
            Field<StringGraphType>(fieldName);
            Field<StringGraphType>(fieldName + "_Not");
            Field<ListGraphType<NonNullGraphType<StringGraphType>>>(fieldName + "_In");
            Field<ListGraphType<NonNullGraphType<StringGraphType>>>(fieldName + "_NotIn");
            Field<StringGraphType>(fieldName + "_Lt");
            Field<StringGraphType>(fieldName + "_Lte");
            Field<StringGraphType>(fieldName + "_Gt");
            Field<StringGraphType>(fieldName + "_Gte");
            Field<StringGraphType>(fieldName + "_Contains");
            Field<StringGraphType>(fieldName + "_NotContains");
            Field<StringGraphType>(fieldName + "_StartsWith");
            Field<StringGraphType>(fieldName + "_NotStartsWith");
            Field<StringGraphType>(fieldName + "_EndsWith");
            Field<StringGraphType>(fieldName + "_NotEndsWith");
            Field<BooleanGraphType>(fieldName + "_IsBlank");
        }
        else if (graphType == typeof(IntGraphType) || graphType == typeof(NonNullGraphType<IntGraphType>))
        {
            Field<IntGraphType>(fieldName);
            Field<IntGraphType>(fieldName + "_Not");
            Field<ListGraphType<NonNullGraphType<IntGraphType>>>(fieldName + "_In");
            Field<ListGraphType<NonNullGraphType<IntGraphType>>>(fieldName + "_NotIn");
            Field<IntGraphType>(fieldName + "_Lt");
            Field<IntGraphType>(fieldName + "_Lte");
            Field<IntGraphType>(fieldName + "_Gt");
            Field<IntGraphType>(fieldName + "_Gte");
            Field<BooleanGraphType>(fieldName + "_IsBlank");
        }
        else if (graphType == typeof(FloatGraphType) || graphType == typeof(NonNullGraphType<FloatGraphType>))
        {
            Field<FloatGraphType>(fieldName);
            Field<FloatGraphType>(fieldName + "_Not");
            Field<ListGraphType<NonNullGraphType<FloatGraphType>>>(fieldName + "_In");
            Field<ListGraphType<NonNullGraphType<FloatGraphType>>>(fieldName + "_NotIn");
            Field<FloatGraphType>(fieldName + "_Lt");
            Field<FloatGraphType>(fieldName + "_Lte");
            Field<FloatGraphType>(fieldName + "_Gt");
            Field<FloatGraphType>(fieldName + "_Gte");
            Field<BooleanGraphType>(fieldName + "_IsBlank");
        }
        else if (graphType == typeof(BooleanGraphType) || graphType == typeof(NonNullGraphType<BooleanGraphType>))
        {
            Field<BooleanGraphType>(fieldName);
        }
        else if (graphType == typeof(DateTimeGraphType) || graphType == typeof(NonNullGraphType<DateTimeGraphType>))
        {
            Field<DateTimeGraphType>(fieldName);
            Field<DateTimeGraphType>(fieldName + "_Not");
            Field<DateTimeGraphType>(fieldName + "_Lt");
            Field<DateTimeGraphType>(fieldName + "_Lte");
            Field<DateTimeGraphType>(fieldName + "_Gt");
            Field<DateTimeGraphType>(fieldName + "_Gte");
            Field<DateTimeGraphType>(fieldName + "_IsBlank");
        }
        else if (graphType == typeof(DateTimeOffsetGraphType) || graphType == typeof(NonNullGraphType<DateTimeOffsetGraphType>))
        {
            Field<DateTimeOffsetGraphType>(fieldName);
            Field<DateTimeOffsetGraphType>(fieldName + "_Not");
            Field<DateTimeOffsetGraphType>(fieldName + "_Lt");
            Field<DateTimeOffsetGraphType>(fieldName + "_Lte");
            Field<DateTimeOffsetGraphType>(fieldName + "_Gt");
            Field<DateTimeOffsetGraphType>(fieldName + "_Gte");
            Field<DateTimeOffsetGraphType>(fieldName + "_IsBlank");
        }
        else if (graphType == typeof(DateGraphType) || graphType == typeof(NonNullGraphType<DateGraphType>))
        {
            Field<DateGraphType>(fieldName);
            Field<DateGraphType>(fieldName + "_Not");
            Field<DateGraphType>(fieldName + "_Lt");
            Field<DateGraphType>(fieldName + "_Lte");
            Field<DateGraphType>(fieldName + "_Gt");
            Field<DateGraphType>(fieldName + "_Gte");
            Field<DateGraphType>(fieldName + "_IsBlank");
        }
    }
}