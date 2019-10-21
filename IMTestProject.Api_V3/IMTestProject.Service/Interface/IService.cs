using IMTestProject.Common;
using IMTestProject.Common.Enum;
using IMTestProject.Common.QuerySerialize;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IMTestProject.Service.Interface
{
    public interface IService <TEntity,TResult, in TAddDto, in TEditDto, TPk> where TEntity : class, IEntity<TPk> where TAddDto : class, new ()
    where  TEditDto : class, new ()  where TResult : class, new () where TPk : IComparable
    {
        Task<(TResult entity, ExecutionState executionState, string message)> CreateAsync(TAddDto entity);
        Task<(TResult entity, ExecutionState executionState, string message)> GetAsync(TPk key);
        Task<(TResult entity, ExecutionState executionState, string message)> GetAsync(FilterOptions<TEntity> filterOptions = null);
        Task<(IQueryable<TResult> entity, ExecutionState executionState, string message)> List(QueryOptions<TEntity> queryOptions = null, ListCondition listCondition = ListCondition.Normal);
        Task<(ExecutionState executionState, string message)> DoesExistAsync(TPk key);
        Task<(ExecutionState executionState, string message)> DoesExistAsync(FilterOptions<TEntity> filterOptions = null);
        Task<(TResult entity, ExecutionState executionState, string message)> UpdateAsync(TEditDto entity);
        Task<(TResult entity, ExecutionState executionState, string message)> RemoveAsync(TPk key);
        Task<(long entityCount, ExecutionState executionState, string message)> CountAsync(CountOptions<TEntity> countOptions = null);
        Task<(TResult entity, ExecutionState executionState, string message)> MarkAsActiveAsync(TPk key);
        Task<(TResult entity, ExecutionState executionState, string message)> MarkAsInactiveAsync(TPk key);

        /*Task<(IQueryable<TResult> entity, ExecutionState executionState, string message)> List(QueryOptionsNodes queryOptionNodes = null);*/

        TEntity CastDtoToEntity (TAddDto source);
        Task<TEntity> CastDtoToEntityAsync (TEditDto source);
        TResult CastEntityToDto (TEntity source);
    }
}