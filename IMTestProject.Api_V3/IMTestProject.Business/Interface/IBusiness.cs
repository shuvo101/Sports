using System.Linq;
using System.Threading.Tasks;
using IMTestProject.Common.Enum;
using IMTestProject.Common.QuerySerialize;

namespace IMTestProject.Business.Interface
{
    public interface IBusiness<T, TPk>
    {
        Task<(T entity, ExecutionState executionState, string message)> CreateAsync(T entity);
        Task<(T entity, ExecutionState executionState, string message)> GetAsync(TPk key);
        Task<(T entity, ExecutionState executionState, string message)> GetAsync(FilterOptions<T> filterOptions = null);
        (IQueryable<T> entity, ExecutionState executionState, string message) List(QueryOptions<T> queryOptions = null, ListCondition listCondition = ListCondition.Normal);
        Task<(ExecutionState executionState, string message)> DoesExistAsync(TPk key);
        Task<(ExecutionState executionState, string message)> DoesExistAsync(FilterOptions<T> filterOptions = null);
        Task<(T entity, ExecutionState executionState, string message)> UpdateAsync(T entity);
        Task<(T entity, ExecutionState executionState, string message)> RemoveAsync(TPk key);
        Task<(long entityCount, ExecutionState executionState, string message)> CountAsync(CountOptions<T> countOptions = null);
        Task<(T entity, ExecutionState executionState, string message)> MarkAsActiveAsync(TPk key);
        Task<(T entity, ExecutionState executionState, string message)> MarkAsInactiveAsync(TPk key);

        Task<(ExecutionState executionState, string message)> ValidateCreateAsync (T toCreate);
        Task<(ExecutionState executionState, string message)> ValidateUpdateAsync (T toUpdate);
        Task<(ExecutionState executionState, string message)> ValidateKeyAsync(TPk key);
    }
}