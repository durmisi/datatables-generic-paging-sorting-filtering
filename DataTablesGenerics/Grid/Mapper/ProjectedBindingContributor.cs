using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DataTablesGenerics.Grid.Mapper
{
    public class ProjectedBindingContributor : IMemberBindingContributor
    {
        public IEnumerable<MemberBinding> GetBindings<TFrom, TTo>(Expression parameter)
        {
            return from fromProperty in typeof(TFrom).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                   from toProperty in typeof(TTo).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                   where fromProperty.Name != toProperty.Name && toProperty.Name.StartsWith(fromProperty.Name)
                   let path = GetPath(parameter, fromProperty, toProperty, toProperty.Name)
                   where path != null
                   let map = new
                   {
                       To = toProperty,
                       Path = path
                   }
                   select Expression.Bind(map.To, map.Path);
        }

        private static Expression GetPath(Expression parameter, PropertyInfo fromProperty, PropertyInfo toProperty, string propertyPathString)
        {
            var newPropertyPath = propertyPathString.Substring(fromProperty.Name.Length);
            var expression = Expression.Property(parameter, fromProperty);
            if (string.IsNullOrEmpty(newPropertyPath)) return expression;

            var candidates = fromProperty.PropertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => newPropertyPath.StartsWith(p.Name));

            foreach (var propertyInfo in candidates)
            {
                var path = GetPath(expression, propertyInfo, toProperty, newPropertyPath);
                if (path != null)
                {
                    var pathAsMemberExpression = path as MemberExpression;
                    if (pathAsMemberExpression != null)
                    {
                        return GuardExpression(pathAsMemberExpression.Expression,
                            (PropertyInfo)pathAsMemberExpression.Member,
                            toProperty.PropertyType);
                    }
                    return path;
                }
            }

            return null;
        }

        private static ConditionalExpression GuardExpression(Expression expression, PropertyInfo member, Type outputType)
        {
            var condition = Expression.Equal(expression, Expression.Constant(null));
            var defaultValue = Expression.Constant(GetDefaultValue(outputType));
            var defaultValueCasted = Expression.TypeAs(defaultValue, outputType);

            return Expression.Condition(condition, defaultValueCasted, Expression.Property(expression, member));
        }

        static object GetDefaultValue(Type t)
        {
            return t.IsValueType ? Activator.CreateInstance(t) : null;
        }
    }
}