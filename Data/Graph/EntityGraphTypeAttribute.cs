namespace Data.Graph;

[AttributeUsage(AttributeTargets.Class)]
public class EntityGraphTypeAttribute:Attribute
{
    public string[]? Except{get;private set;}
    public string[]? Only{get;private set;}
    public EntityGraphTypeAttribute(string[]? except =null, string[]? only=null){
        this.Except = except;
        this.Only = only;
    }
    public EntityGraphTypeAttribute(params string[] except){
        this.Except = except;
    }
}
