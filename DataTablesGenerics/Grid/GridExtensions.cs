using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataTablesGenerics.Grid
{
    public static class GridExtensions
    {
        public static GridResult<TViewModel> ToGridData<TModel, TViewModel>(
            this IQueryable<TModel> query,
            FilterExpression<TModel> filterExpression,
            Expression<Func<TModel, TViewModel>> projection,
            SortExpression<TModel> sortExpression,
            int skip,
            int take
            )
        {
            IEnumerable<TViewModel> result = new List<TViewModel>();

            //count
            int totalPages = 0;
            var recordsTotal = query.Count();
            var recordsFiltered = recordsTotal;

            //filtering
            if (filterExpression != null && filterExpression.Expression != null)
            {
                query = query.Where(filterExpression.Expression);
                recordsFiltered = query.Count();
            }

            if (recordsTotal > 0)
            {
                //sorting
                var isSorted = false;
                foreach (var se in sortExpression.SortExpressions)
                {
                    if (!isSorted)
                    {
                        query = ApplySort(query, se.Key, se.Value);
                        isSorted = true;
                    }
                    else
                    {
                        query = ApplySortAgain((IOrderedQueryable<TModel>)query, se.Key, se.Value);
                    }
                }

                //paging
                var data = query.Skip(skip).Take(take);

                totalPages = recordsFiltered > 0 && take > 0
                    ? Convert.ToInt32(Math.Ceiling((recordsFiltered / (decimal)take)))
                    : 0;

                //projecting
                result = data.Select(projection)
                             .ToList();
            }

            return new GridResult<TViewModel>(result, recordsTotal, recordsFiltered, totalPages);
        }


        public static GridResult<TViewModel> ToGridData2<TModel, TViewModel>(
            this IQueryable<TModel> query,
            FilterExpression<TModel> filterExpression,
            Expression<Func<TModel, TViewModel>> projection,
            SortExpression<TModel> sortExpression,
            int pageIndex,
            int pageSize
            )
        {
            int skip = (pageIndex - 1) * pageSize;
            return ToGridData(query, filterExpression, projection, sortExpression, skip, pageSize);
        }

        private static IOrderedQueryable<TModel> ApplySort<TModel>(IQueryable<TModel> query, Expression<Func<TModel, object>> expression,
            OrderDirection direction)
        {
            return direction == OrderDirection.Ascendant
                ? query.OrderBy(expression)
                : query.OrderByDescending(expression);
        }

        private static IOrderedQueryable<TModel> ApplySortAgain<TModel>(IOrderedQueryable<TModel> query, Expression<Func<TModel, object>> expression,
            OrderDirection direction)
        {
            return direction == OrderDirection.Ascendant
                ? query.ThenBy(expression)
                : query.ThenByDescending(expression);
        }
    }
}
