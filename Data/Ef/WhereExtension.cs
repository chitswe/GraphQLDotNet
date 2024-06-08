using System.Linq.Expressions;
using System.Reflection;

namespace Data.Ef;

public static class WhereExtension
{
    public static IQueryable<E> Where<E>(this IQueryable<E> source, Dictionary<string, object> where) where E : Entity
    {
        return source.Where(BuildPredicate<E>(where));
    }

    public static Expression<Func<E, bool>> BuildPredicate<E>(Dictionary<string, object> where) where E : Entity
    {
        ParameterExpression parameter = Expression.Parameter(typeof(E), "Entity");
        var expression = BuildExpression<E>(where, parameter);
        var predicate = Expression.Lambda<Func<E, Boolean>>(expression, new ParameterExpression[] { parameter });
        return predicate;
    }
  

   private static Expression BuildExpression<E>(Dictionary<string, object> where, Expression parameter) where E : Entity
    {
        if (where == null)
        {
            return Expression.Constant(true);
        }

        var expressions = where.Keys.Select(key=>{
            var value = where[key];
            if(key == "and")
                return BuildExpression<E>(value as object[], parameter, true);
            if(key == "or")
                return BuildExpression<E>(value as object[], parameter, false); 
            if(value == null)
                return null;
            var entityTypeInfo = typeof(E);
            key = char.ToUpper(key[0]) + key.Substring(1);
            var splitted = key.Split('_');
            var originPropertyName = splitted.Length == 1? splitted[0]: splitted.SkipLast(1).Aggregate("",(all,current)=>(all == ""?"": "_") + current);          
            var originProperty =  entityTypeInfo.GetProperty(originPropertyName);
            if(originProperty == null)
                throw new ArgumentException(String.Format("Cannot find Property named {0} in Entity {1}", originPropertyName, entityTypeInfo.Name));
            
            var valueType = value.GetType();
            if(valueType == typeof(string) && value as string =="")
                return null;
            Expression result;
            if (value is Dictionary<string,object>)
                result = BuildExpression<E>((Dictionary<string,object>)value, Expression.Property(parameter,originPropertyName));
            else
                result = BuildExpression(originProperty, key,  value,  parameter);
            return result;
        });        
        return BuildExpression(expressions, true);
    }

    private static Expression BuildExpression(IEnumerable<Expression?> expressions, bool and)
        {
            Expression? combined = expressions?.FirstOrDefault();
            if (combined == null)
                return Expression.Constant(true);
            bool first = true;
            combined = expressions?.Aggregate(combined, (current, e) =>
            {
                if (first)
                {
                    first = false;
                    return combined;
                }else if(e== null)
                    return combined;
                else if (and)
                    return Expression.And(current, e);
                else
                    return Expression.Or(current, e);
            });
            return combined!;
        }

    private static Expression? BuildExpression<E>(object[]? wheres, Expression parameter, bool and) where E : Entity
        {
            if(wheres==null)
                return null;
            var expressions = wheres.Select(w =>
            {
                return BuildExpression<E>((Dictionary<string,object>)w , parameter);
            });
            return BuildExpression(expressions, and);
        }

    private static Expression BuildExpression(PropertyInfo originProperty,string filterKey, object filterValue, Expression parameter)
        {
            Type type = originProperty.PropertyType;
            string name = originProperty.Name;
            string op = name== filterKey?"": filterKey.Replace(name + "_", "");
            Expression fieldExpression = Expression.Property(parameter, name);
            if(name == "Id")
                filterValue = int.Parse(filterValue?.ToString()!);
            Expression valueExpression = Expression.Constant(filterValue);

            if (op == "IsBlank")
            {
                if (type == typeof(int))
                    fieldExpression = Expression.Convert(fieldExpression, typeof(Nullable<int>));
                else if (type == typeof(long))
                    fieldExpression = Expression.Convert(fieldExpression, typeof(Nullable<long>));
                else if (type == typeof(float))
                    fieldExpression = Expression.Convert(fieldExpression, typeof(Nullable<float>));
                else if (type == typeof(double))
                    fieldExpression = Expression.Convert(fieldExpression, typeof(Nullable<double>));
                else if (type == typeof(decimal))
                    fieldExpression = Expression.Convert(fieldExpression, typeof(Nullable<decimal>));
                else if (type == typeof(bool))
                    fieldExpression = Expression.Convert(fieldExpression, typeof(Nullable<bool>));
                else if (type == typeof(DateOnly))
                    fieldExpression = Expression.Convert(fieldExpression, typeof(Nullable<DateOnly>));
                else if (type == typeof(DateTime))
                    fieldExpression = Expression.Convert(fieldExpression, typeof(Nullable<DateTime>));
                else if (type == typeof(DateTimeOffset))
                    fieldExpression = Expression.Convert(fieldExpression, typeof(Nullable<DateTimeOffset>));

            }
            else if (type == typeof(String))
            {
                fieldExpression = Expression.Call(fieldExpression, type.GetMethod("ToUpper", new Type[] { })!);
                valueExpression = Expression.Call(valueExpression, type.GetMethod("ToUpper", new Type[] { })!);
            }
            else if (type == typeof(Nullable<int>) || type == typeof(Nullable<long>) || type == typeof(Nullable<float>) || type == typeof(Nullable<double>) || type == typeof(Nullable<decimal>) || type == typeof(Nullable<bool>) || type == typeof(Nullable<DateOnly>) || type == typeof(Nullable<DateTime>) || type == typeof(Nullable<DateTimeOffset>))
            {
                fieldExpression = Expression.Convert(fieldExpression, type);
                valueExpression = Expression.Convert(valueExpression, type);
            }
            Expression result = Expression.Constant(true);
            switch (op)
            {
                case "":
                    result = Expression.Equal(fieldExpression, valueExpression);
                    break;
                case "Not":
                    result = Expression.NotEqual(fieldExpression, valueExpression);
                    break;
                case "Lt":
                    if (type == typeof(String))
                    {
                        fieldExpression = Expression.Call(fieldExpression, type.GetMethod("CompareTo", new Type[] { typeof(String) })!, valueExpression);
                        result = Expression.LessThan(fieldExpression, Expression.Constant(0));
                    }
                    else
                        result = Expression.LessThan(fieldExpression, valueExpression);
                    break;
                case "Lte":
                    if (type == typeof(String))
                    {
                        fieldExpression = Expression.Call(fieldExpression, type.GetMethod("CompareTo", new Type[] { typeof(String) })!, valueExpression);
                        result = Expression.GreaterThanOrEqual(fieldExpression, Expression.Constant(0));
                    }
                    else
                        result = Expression.LessThanOrEqual(fieldExpression, valueExpression);
                    break;
                case "Gt":
                    if (type == typeof(String))
                    {
                        fieldExpression = Expression.Call(fieldExpression, type.GetMethod("CompareTo", new Type[] { typeof(String) })!, valueExpression);
                        result = Expression.GreaterThan(fieldExpression, Expression.Constant(0));
                    }
                    else
                        result = Expression.GreaterThan(fieldExpression, valueExpression);
                    break;
                case "Gte":
                    if (type == typeof(String))
                    {
                        fieldExpression = Expression.Call(fieldExpression, type.GetMethod("CompareTo", new Type[] { typeof(String) })!, valueExpression);
                        result = Expression.GreaterThanOrEqual(fieldExpression, Expression.Constant(0));
                    }
                    else
                        result = Expression.GreaterThanOrEqual(fieldExpression, valueExpression);
                    break;
                case "In":
                    result = Expression.Call(typeof(Enumerable), "Contains", new Type[] { fieldExpression.Type }, new Expression[] { valueExpression, fieldExpression });
                    break;
                case "NotIn":
                    result = Expression.Call(typeof(Enumerable), "Contains", new Type[] { fieldExpression.Type }, new Expression[] { valueExpression, fieldExpression });
                    result = Expression.Equal(result, Expression.Constant(false));
                    break;
                case "Contains":
                    result = Expression.Call(fieldExpression, typeof(String).GetMethod("Contains", new Type[] { typeof(String) })!, valueExpression);
                    break;
                case "NotContains":
                    result = Expression.Call(fieldExpression, typeof(String).GetMethod("Contains", new Type[] { typeof(String) })!, valueExpression);
                    result = Expression.Equal(result, Expression.Constant(false));
                    break;
                case "StartsWith":
                    result = Expression.Call(fieldExpression, typeof(String).GetMethod("StartsWith", new Type[] { typeof(String) })!, valueExpression);
                    break;
                case "NotStartsWith":
                    result = Expression.Call(fieldExpression, typeof(String).GetMethod("StartsWith", new Type[] { typeof(String) })!, valueExpression);
                    result = Expression.Equal(result, Expression.Constant(false));
                    break;
                case "EndsWith":
                    result = Expression.Call(fieldExpression, typeof(String).GetMethod("EndsWith", new Type[] { typeof(String) })!, valueExpression);
                    break;
                case "NotEndsWith":
                    result = Expression.Call(fieldExpression, typeof(String).GetMethod("EndsWith", new Type[] { typeof(String) })!, valueExpression);
                    result = Expression.Equal(result, Expression.Constant(false));
                    break;
                case "IsBlank":
                    result = Expression.Equal(fieldExpression, Expression.Constant(null));
                    if (fieldExpression.Type == typeof(string))
                    {
                        result = Expression.Or(result, Expression.Equal(fieldExpression, Expression.Constant("")));
                    }
                    result = ((bool)filterValue) ? result : Expression.Not(result);
                    break;
            }
            return result;
        }
}
