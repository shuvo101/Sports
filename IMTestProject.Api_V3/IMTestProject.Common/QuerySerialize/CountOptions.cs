using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace IMTestProject.Common.QuerySerialize
{
    public class CountOptions<TEntity>
    {
        public Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> IncludeExpression { get; set; }
        public Expression<Func<TEntity, bool>> FilterExpression { get; set; }

        public static CountOptions<TEntity> FromCountOptionsNodes (CountOptionsNodes nodes)
        {
            if (nodes == null)
            {
                return null;
            }

            CountOptions<TEntity> countOptions = new CountOptions<TEntity> ();

            if (nodes.FilterExpressionNode != null)
            {
                countOptions.FilterExpression = nodes.FilterExpressionNode.ToBooleanExpression<TEntity>();
            }
            if (nodes.IncludeExpressionNode != null)
            {
                countOptions.IncludeExpression = nodes
                .IncludeExpressionNode.ToExpression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>();
            }

            //countOptions.IncludeExpression = nodes?
            //    .IncludeExpressionNode?.ToExpression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> ()?.Compile ();

            //countOptions.FilterExpression = nodes?.FilterExpressionNode?.ToBooleanExpression<TEntity> ()?.Compile ();

            return countOptions;
        }
    }
}