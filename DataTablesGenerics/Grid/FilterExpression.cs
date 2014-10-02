using System;
using System.Linq.Expressions;
using DataTablesGenerics.Grid.Extensions.Linq;

namespace DataTablesGenerics.Grid
{
    public class FilterExpression<T>
    {
        private bool _dirty = false;
        private Expression<Func<T, bool>> _expression;

        public FilterExpression()
        {
            _expression = (f) => true;
        }

        public void Or(Expression<Func<T, bool>> expr2)
        {
            if (_dirty) _expression = _expression.Or(expr2);
            else
            {
                _expression = expr2;
                _dirty = true;
            }
        }

        public void And(Expression<Func<T, bool>> expr2)
        {
            if (_dirty)
                _expression = _expression.And(expr2);
            else
            {
                _expression = expr2;
                _dirty = true;
            }
        }

        public Expression<Func<T, bool>> Expression
        {
            get { return _expression; }
        }
    }
}