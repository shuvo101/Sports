using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace IMTestProject.Common.QuerySerialize
{
    public class FilterOptions<TEntity>
    {
        public Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> IncludeExpression { get; set; }
        public Expression<Func<TEntity, bool>> FilterExpression { get; set; }

        public static FilterOptions<TEntity> FromFilterOptionsNodes (FilterOptionsNodes nodes)
        {
            if (nodes == null)
            {
                return null;
            }

            FilterOptions<TEntity> filterOptions = new FilterOptions<TEntity> ();

            if (nodes.FilterExpressionNode != null)
            {
                filterOptions.FilterExpression = nodes.FilterExpressionNode.ToBooleanExpression<TEntity>();
            }
            if (nodes.IncludeExpressionNode != null)
            {
                filterOptions.IncludeExpression = nodes
                .IncludeExpressionNode.ToExpression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>();
            }


            //filterOptions.IncludeExpression = nodes?
            //    .IncludeExpressionNode?.ToExpression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> ()?.Compile ();

            //filterOptions.FilterExpression = nodes?.FilterExpressionNode?.ToBooleanExpression<TEntity> ()?.Compile ();

            return filterOptions;
        }
    }
}