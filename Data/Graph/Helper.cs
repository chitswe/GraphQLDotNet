using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Data.Ef;
using GraphQL.Types;

namespace Data.Graph;

public static class Helper
{
    public static Type ResolveGraphType<E>(this EntityGraphType<E> e, PropertyInfo property) where E : Entity
    {
        return Helper.ResolveGraphType(property, typeof(E), false);
    }

    public static Type ResolveGraphType<E, W>(this EntityWhereInputGraphType<E, W> e, PropertyInfo property) where E : Entity where W : EntityWhereInputGraphType<E, W>
    {
        return Helper.ResolveGraphType(property, typeof(E), false);
    }

    public static Type ResolveGraphType<D>(this EntityInputGraphType<D> e, PropertyInfo property) where D: EntityDto{
        return Helper.ResolveGraphType(property, typeof(D), false);
    }

    public static Type BuildNonNullableGraphType(Type type)
    {
        Type generic = typeof(NonNullGraphType<>);
        return generic.MakeGenericType(type);
    }

    public static Type BuildListGraphType(Type type)
    {
        Type generic = typeof(ListGraphType<>);
        return generic.MakeGenericType(type);
    }

    public static Type? IsEnumerationGraphType(Type type)
    {
        if (type.BaseType == typeof(NonNullGraphType))
        {
            var genericType = type.GetGenericArguments()[0];
            if (genericType.BaseType?.IsAssignableTo(typeof(EnumerationGraphType))??false)
            {
                return genericType;
            }
        }
        return null;
    }

    private static Type ResolveGraphType(PropertyInfo property, Type _typeInfo, bool input) {
        var requiredAttribute = property.GetCustomAttributes<RequiredAttribute>();
        var m2OneFieldAttribute = property.GetCustomAttribute<Many2oneGraphQLFieldAttribute>();
        var graphQLFieldAttribute = property.GetCustomAttribute<GraphQLFieldAttribute>();
        var type = property.PropertyType;
        var intTypes = new Type[] { typeof(long), typeof(int), typeof(short), typeof(byte), typeof(Int16), typeof(Int32), typeof(Int64), typeof(Int128) };
        var floatTypes = new Type[] { typeof(float), typeof(double), typeof(decimal) };
        Type? graphType = null;
        if (graphQLFieldAttribute?.GraphType != null)
        {
            graphType = graphQLFieldAttribute.GraphType;
        }
        else if (m2OneFieldAttribute != null)
            if (input)
            {
                //TODO: 
            }
            else
                if (m2OneFieldAttribute.GraphType.BaseType?.GetGenericTypeDefinition() == typeof(EntityGraphType<>).GetGenericTypeDefinition())
                graphType = m2OneFieldAttribute.GraphType;
            else
                throw new ArgumentException("Type of Many2one field must be instance of EntityGraphType");
        else if (type == typeof(string))
            graphType = typeof(StringGraphType);
        else if (intTypes.Contains(type))
            graphType = typeof(IntGraphType);
        else if (floatTypes.Contains(type))
            graphType = typeof(FloatGraphType);
        else if (type == typeof(bool))
            graphType = typeof(BooleanGraphType);
        else if (type == typeof(DateOnly))
            graphType = typeof(DateGraphType);
        else if (type == typeof(DateTime))
            graphType = typeof(DateTimeGraphType);
        else if (type == typeof(DateTimeOffset))
            graphType = typeof(DateTimeOffsetGraphType);
        
        if(graphType== null)
            throw new ArgumentOutOfRangeException(string.Format("Cannot determine GraphType of field named {0} in EntityGraphType named {1}", property.Name, _typeInfo.Name));
        if (requiredAttribute == null)
            return graphType;
        return typeof(NonNullGraphType<>).MakeGenericType([graphType]);
    }
}
