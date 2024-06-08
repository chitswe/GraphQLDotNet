using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Data.Ef;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Graph;

public class EntityGraphType<E> : ObjectGraphType<E> where E : Entity
{
    private TypeInfo _typeInfo;
    public EntityGraphType()
    {
        Field(x => x.Id, type: typeof(IdGraphType));
        Type type = typeof(E);
        _typeInfo = type.GetTypeInfo();
        init();
    }
    private void init()
    {
        var entityGraphTypeAttribute = this._typeInfo.GetCustomAttribute<EntityGraphTypeAttribute>();
        var only = entityGraphTypeAttribute?.Only ?? new string[0];
        var except = entityGraphTypeAttribute?.Except ?? new string[0];
        foreach (PropertyInfo property in this._typeInfo.GetProperties())
        {
            if (property.Name == "Id")
                continue;
            var graphQLFieldAttribute = property.GetCustomAttribute<GraphQLFieldAttribute>();
            var m2OneFieldAttribute = property.GetCustomAttribute<Many2oneGraphQLFieldAttribute>();
            if (this.Fields.Find(property.Name) != null) { continue;}
            
            else if(property.PropertyType.IsAssignableTo(typeof(Entity))){continue;}
            else if (graphQLFieldAttribute != null) { }
            else if (m2OneFieldAttribute != null) { }
            else if (only.Contains(property.Name)) { }
            else if (!except.Contains(property.Name)) { }
            else
                continue;
            var fieldBuilder = graphQLFieldAttribute?.GraphType != null ? Field(graphQLFieldAttribute.Name ?? property.Name, graphQLFieldAttribute.GraphType) : Field(m2OneFieldAttribute?.Name ?? property.Name, this.ResolveGraphType(property));
            var descriptionAttribute = property.GetCustomAttribute<DescriptionAttribute>();
            if (graphQLFieldAttribute?.Description != null)
                fieldBuilder.Description(graphQLFieldAttribute.Description);
            else if (descriptionAttribute != null)
                fieldBuilder.Description(descriptionAttribute.Description);
            if (m2OneFieldAttribute != null)
            {
                fieldBuilder.Resolve(context =>
                {
                    var dataLoaderType = typeof(EntityBatchDataLoader<>).MakeGenericType([m2OneFieldAttribute.EntityType]);
                    var loader = context.RequestServices?.GetRequiredService(dataLoaderType);
                    if (loader == null)
                        throw new ArgumentException("Could not determine EntityBatchDataLoader for field " + property.Name);
                    var id = (int?)property.GetValue(context.Source);
                    if (!id.HasValue)
                        return null;
                    else
                    {
                        var methodInfo = dataLoaderType.GetMethod("LoadAsync");
                        return methodInfo?.Invoke(loader, [id.Value]);
                    }
                });
            }
        }
    }    
}
