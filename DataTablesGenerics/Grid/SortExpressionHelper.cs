using System;
using System.Linq;
using System.Linq.Expressions;

namespace DataTablesGenerics.Grid
{
    public class SortExpressionHelper
    {
        public static Expression<Func<T, object>> CreateSortExpression<T>(string sortColumn)
        {
            var param = Expression.Parameter(typeof(T), "item");

            var memberAccess = sortColumn.Split('.')
                .Aggregate<string, MemberExpression>(null, (current, property) =>
                    Expression.Property(current ?? (param as Expression), property));

            var sortKey = memberAccess.Member.Name;

            var body = Expression.Convert(Expression.Property(param, sortKey), typeof(object));
            var sortExpression = Expression.Lambda<Func<T, object>>(body, param);

            return sortExpression;
        }
    }
}
