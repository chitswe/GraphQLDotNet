using System.Reflection;
using Data.Ef;
using GraphQL.Types;
using Data.Graph;
using System.ComponentModel;

public class EntityInputGraphType<D>:InputObjectGraphType<D> where D : EntityDto{
    private TypeInfo _typeInfo;
    public EntityInputGraphType()
    {
        Type type = typeof(D);
        _typeInfo = type.GetTypeInfo();
        init();
    }
    private void init()
    {        
        foreach (PropertyInfo property in this._typeInfo.GetProperties())
        {           
            if (this.Fields.Find(property.Name) != null)             
                continue;
            var fieldBuilder = Field(property.Name, this.ResolveGraphType(property));
            var descriptionAttribute = property.GetCustomAttribute<DescriptionAttribute>();
            if (descriptionAttribute != null)
                fieldBuilder.Description(descriptionAttribute.Description);
        }
    }    
}