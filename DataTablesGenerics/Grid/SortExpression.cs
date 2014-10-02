using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataTablesGenerics.Grid
{
    public class SortExpression<T>
    {
        private readonly Dictionary<Expression<Func<T, object>>, OrderDirection> _sortExpressions;

        public SortExpression()
        {
            _sortExpressions = new Dictionary<Expression<Func<T, object>>, OrderDirection>();
        }

        public virtual void AddSortExpression(Expression<Func<T, object>> expression, OrderDirection direction)
        {
            _sortExpressions.Add(expression, direction);
        }

        public IDictionary<Expression<Func<T, object>>, OrderDirection> SortExpressions
        {
            get { return _sortExpressions; }
        }
    }
}
