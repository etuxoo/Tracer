using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TraceService.Application.Extensions;

public static class IQuerableExtensions
{
    public static IQueryable<T> If<T>(this IQueryable<T> source, bool condition, Func<IQueryable<T>, IQueryable<T>> action)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        if (condition)
        {
            return action(source);
        }

        return source;
    }

    public static IQueryable<T> If<T>(this IQueryable<T> source, bool? condition, Func<IQueryable<T>, IQueryable<T>> action)
    {
        return source.If(condition ?? false, action);
    }

    public static IQueryable<T> OrderIf<T, TU>(this IQueryable<T> source, bool condition, string direction, Expression<Func<T, TU>> action)
    {
        if (condition)
        {
            if (direction == "desc")
            {
                return source.OrderByDescending(action);
            }
            else
            {
                return source.OrderBy(action);
            }
        }

        return source;
    }
}
