using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DataTablesGenerics.Grid
{
    public class FilterExpressionHelper
    {
        private static readonly MethodInfo ToLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
        private static readonly MethodInfo ContainsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        private static readonly MethodInfo StartsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
        private static readonly MethodInfo EndsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });

        public static Expression<Func<T, bool>> CreateFilterExpression<T>(string sortColumn, object searchValue, SearchOperator searchOperator = SearchOperator.StartsWith, bool caseInsensitive = true)
        {
            var parameter = Expression.Parameter(typeof(T), "item");

            var memberAccess = sortColumn.Split('.')
                .Aggregate<string, MemberExpression>(null, (current, property) =>
                    Expression.Property((Expression)(current ?? (parameter as Expression)), (string)property));

            // The value we want to evaluate
            ConstantExpression filter;
            if (memberAccess.Type.IsEnum)
            {
                var obj = Enum.Parse(memberAccess.Type, searchValue.ToString());
                filter = Expression.Constant(obj);
            }
            else
            {
                var t = memberAccess.Type;
                string s = searchValue.ToString();
                object d;

                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    if (String.IsNullOrEmpty(s))
                        d = null;
                    else
                        d = Convert.ChangeType(s, t.GetGenericArguments()[0]);
                }
                else
                {
                    d = Convert.ChangeType(s, t);
                }
                filter = Expression.Constant(d);
            }

            Expression<Func<T, bool>> lambda;
            MethodCallExpression condition;
            switch (searchOperator)
            {
                case SearchOperator.Equals:
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.Equal(memberAccess, filter), parameter);
                    return lambda;
                case SearchOperator.NotEqual:
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.NotEqual(memberAccess, filter), parameter);
                    return lambda;
                case SearchOperator.LessThan:
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.LessThan(memberAccess, filter), parameter);
                    return lambda;
                case SearchOperator.LessThanOrEqualTo:
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(memberAccess, filter), parameter);
                    return lambda;
                case SearchOperator.GreaterThan:
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(memberAccess, filter), parameter);
                    return lambda;
                case SearchOperator.GreaterThanOrEqual:
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(memberAccess, filter), parameter);
                    return lambda;
                case SearchOperator.Contains:

                    if (caseInsensitive)
                    {
                        var toLower = Expression.Call(memberAccess, ToLowerMethod);
                        condition = Expression.Call(toLower,
                            ContainsMethod,
                            new List<Expression> { Expression.Constant(searchValue.ToString().ToLower()) }
                          );
                    }
                    else
                    {
                        condition = Expression.Call(memberAccess, ContainsMethod, new List<Expression> {
                            filter
                        });
                    }

                    lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
                    return lambda;

                case SearchOperator.StartsWith:

                    if (caseInsensitive)
                    {
                        var toLower = Expression.Call(memberAccess, ToLowerMethod);
                        condition = Expression.Call(toLower,
                            StartsWithMethod,
                            new List<Expression> { Expression.Constant(searchValue.ToString().ToLower()) }
                          );
                    }
                    else
                    {
                        condition = Expression.Call(memberAccess, StartsWithMethod, new List<Expression> {
                            filter
                        });
                    }

                    lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
                    return lambda;
                case SearchOperator.EndsWith:

                    if (caseInsensitive)
                    {
                        var toLower = Expression.Call(memberAccess, ToLowerMethod);
                        condition = Expression.Call(toLower,
                            EndsWithMethod,
                            new List<Expression> { Expression.Constant(searchValue.ToString().ToLower()) }
                          );
                    }
                    else
                    {
                        condition = Expression.Call(memberAccess, EndsWithMethod, new List<Expression> {
                            filter
                        });
                    }

                    lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
                    return lambda;

                //Date
                case SearchOperator.DateEqual:
                    filter = Expression.Constant(Convert.ToDateTime(searchValue), typeof(DateTime));
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.Equal(memberAccess, filter), parameter);
                    return lambda;

                case SearchOperator.DateLessThan:
                    filter = Expression.Constant(Convert.ToDateTime(searchValue), typeof(DateTime));
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.LessThan(memberAccess, filter), parameter);
                    return lambda;

                case SearchOperator.DateGreaterThan:
                    filter = Expression.Constant(Convert.ToDateTime(searchValue), typeof(DateTime));
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(memberAccess, filter), parameter);
                    return lambda;

                case SearchOperator.DateGreaterThanOrEqual:
                    filter = Expression.Constant(Convert.ToDateTime(searchValue), typeof(DateTime));
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(memberAccess, filter), parameter);
                    return lambda;

                case SearchOperator.DateLessThanOrEqual:
                    filter = Expression.Constant(Convert.ToDateTime(searchValue), typeof(DateTime));
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(memberAccess, filter), parameter);
                    return lambda;

                //Nullable Date
                case SearchOperator.NullableDateEqual:
                    filter = Expression.Constant(Convert.ToDateTime(searchValue), typeof(DateTime));
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.Equal(memberAccess, filter), parameter);
                    return lambda;

                case SearchOperator.NullableDateLessThan:
                    filter = Expression.Constant(Convert.ToDateTime(searchValue), typeof(DateTime?));
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.LessThan(memberAccess, filter), parameter);
                    return lambda;

                case SearchOperator.NullableDateGreaterThan:
                    filter = Expression.Constant(Convert.ToDateTime(searchValue), typeof(DateTime?));
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(memberAccess, filter), parameter);
                    return lambda;

                case SearchOperator.NullableDateGreaterThanOrEqual:
                    filter = Expression.Constant(Convert.ToDateTime(searchValue), typeof(DateTime?));
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(memberAccess, filter), parameter);
                    return lambda;

                case SearchOperator.NullableDateLessThanOrEqual:
                    filter = Expression.Constant(Convert.ToDateTime(searchValue), typeof(DateTime?));
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(memberAccess, filter), parameter);
                    return lambda;

                //Int
                case SearchOperator.IntEqual:
                    filter = Expression.Constant(Convert.ToDateTime(searchValue), typeof(int));
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.Equal(memberAccess, filter), parameter);
                    return lambda;

                case SearchOperator.IntLessThan:
                    filter = Expression.Constant(Convert.ToDateTime(searchValue), typeof(int));
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.LessThan(memberAccess, filter), parameter);
                    return lambda;

                case SearchOperator.IntGreaterThan:
                    filter = Expression.Constant(Convert.ToDateTime(searchValue), typeof(int));
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(memberAccess, filter), parameter);
                    return lambda;

                case SearchOperator.IntGreaterThanOrEqual:
                    filter = Expression.Constant(Convert.ToDateTime(searchValue), typeof(int));
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(memberAccess, filter), parameter);
                    return lambda;

                case SearchOperator.IntLessThanOrEqual:
                    filter = Expression.Constant(Convert.ToDateTime(searchValue), typeof(int));
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(memberAccess, filter), parameter);
                    return lambda;

                //Nullable Int
                case SearchOperator.NullableIntEqual:
                    filter = Expression.Constant(Convert.ToDateTime(searchValue), typeof(int));
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.Equal(memberAccess, filter), parameter);
                    return lambda;

                case SearchOperator.NullableIntLessThan:
                    filter = Expression.Constant(Convert.ToDateTime(searchValue), typeof(int?));
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.LessThan(memberAccess, filter), parameter);
                    return lambda;

                case SearchOperator.NullableIntGreaterThan:
                    filter = Expression.Constant(Convert.ToDateTime(searchValue), typeof(int?));
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(memberAccess, filter), parameter);
                    return lambda;

                case SearchOperator.NullableIntGreaterThanOrEqual:
                    filter = Expression.Constant(Convert.ToDateTime(searchValue), typeof(int?));
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(memberAccess, filter), parameter);
                    return lambda;

                case SearchOperator.NullableIntLessThanOrEqual:
                    filter = Expression.Constant(Convert.ToDateTime(searchValue), typeof(int?));
                    lambda = Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(memberAccess, filter), parameter);
                    return lambda;

            }
            return null;
        }
    }
}