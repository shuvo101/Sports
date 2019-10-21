using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace IMTestProject.Common.QuerySerialize
{
    public class QueryOptions<TEntity>
    {
        public Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> IncludeExpression { get; set; }
        public Expression<Func<TEntity, bool>> FilterExpression { get; set; }
        public Expression<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>> SortingExpression { get; set; }
        public Pagination Pagination { get; set; }

        public static QueryOptions<TEntity> FromQueryOptionsNodes (QueryOptionsNodes nodes)
        {
            if (nodes == null)
            {
                return null;
            }
            

            var expression = nodes.FilterExpressionNode.ToBooleanExpression<TEntity>();
            var test = expression.Compile();

            QueryOptions<TEntity> queryOptions = new QueryOptions<TEntity> ();

            if (nodes.FilterExpressionNode != null)
            {
                queryOptions.FilterExpression = nodes.FilterExpressionNode.ToBooleanExpression<TEntity>();
            }
            if (nodes.IncludeExpressionNode != null)
            {
                queryOptions.IncludeExpression = nodes.IncludeExpressionNode.ToExpression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>();
            }

            if (nodes.SortingExpressionNode != null)
            {
                queryOptions.SortingExpression = nodes.SortingExpressionNode.ToExpression<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>>();
            }

            //queryOptions.IncludeExpression = nodes?
            //    .IncludeExpressionNode?.ToExpression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> ()?.Compile ();
            
            //queryOptions.FilterExpression = nodes?
            //    .FilterExpressionNode?.ToBooleanExpression<TEntity> ()?.Compile ();
            
            //queryOptions.SortingExpression = nodes?
            //    .SortingExpressionNode?.ToExpression<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>> ()?.Compile ();
            
            queryOptions.Pagination = nodes?.Pagination;

            return queryOptions;
        }
    }
}