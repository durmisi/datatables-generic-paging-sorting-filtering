using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DataTablesGenerics.Grid.Mapper
{
    public class SimplePropertyContributor : IMemberBindingContributor
    {
        public IEnumerable<MemberBinding> GetBindings<TFrom, TTo>(Expression parameter)
        {
            return from fromProperty in typeof(TFrom).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                   from toProperty in typeof(TTo).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                   where MatchProperty(fromProperty, toProperty)
                   let map = new
                   {
                       From = fromProperty,
                       To = toProperty
                   }
                   select Expression.Bind(map.To, Expression.Property(parameter, map.From));
        }

        protected virtual bool MatchProperty(PropertyInfo fromProperty, PropertyInfo toProperty)
        {
            return fromProperty.Name == toProperty.Name
                   && fromProperty.PropertyType == toProperty.PropertyType;
        }
    }
}