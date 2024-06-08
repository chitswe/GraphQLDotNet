using System.ComponentModel;
using System.Linq.Expressions;

namespace Data.Ef;

public static class OrderByExtension
{
    public static IQueryable<E> OrderBy<E>(this IQueryable<E> source, Dictionary<string, object>[] orderBy)
    {
        if (orderBy == null || orderBy.Length == 0)
        {
            return source;
        }
        else
        {
            var flatterneds = orderBy.Select(order => FlatternKeys(order));
            ParameterExpression parameter = Expression.Parameter(typeof(E), "Entity");
            bool first = true;
            foreach (var flatterned in flatterneds)
            {
                source = flatterned.Aggregate(source, (current, order) =>
                {
                    bool thenBy = !first;
                    if(first)
                        first = false;
                    return AppendOrderBy<E>(source, order.Key, order.Value, parameter,thenBy);
                });
            }
            return source;
        }

    }

    private static Dictionary<string, int> FlatternKeys(Dictionary<string, object> source, string? path = null, Dictionary<string, int>? result = null)
    {
        if (result == null)
            result = new Dictionary<string, int>();
        foreach (var s in source)
        {
            var key = path == null ? s.Key : (path + "." + s.Key);
            if (s.Value is Dictionary<string, object>)
            {
                FlatternKeys((Dictionary<string, object>)s.Value, key, result);
            }
            else
            {
                var order = (int)s.Value;
                if (order != 0)
                    result.Add(key, (int)s.Value);
            }
        }
        return result;
    }


    private static IQueryable<E> AppendOrderBy<E>(IQueryable<E> source, string key, int orderBy, ParameterExpression parameter, bool thenBy = true)
    {
        var fields = key.Split(".");
        Expression fieldExpression = parameter;
        fieldExpression = fields.Aggregate(fieldExpression, (current, field) =>
        {
            return Expression.Property(current, field);
        });
        fieldExpression = Expression.Convert(fieldExpression, typeof(object));
        var predicate = Expression.Lambda<Func<E, object>>(fieldExpression, new ParameterExpression[] { parameter });
        if (orderBy >= 1)
            if (thenBy)
                return ((IOrderedQueryable<E>)source).ThenByDescending(predicate);
            else
                return source.OrderByDescending(predicate);
        else
            if (thenBy)
            return ((IOrderedQueryable<E>)source).ThenBy(predicate);
        else
            return source.OrderBy(predicate);
    }
}