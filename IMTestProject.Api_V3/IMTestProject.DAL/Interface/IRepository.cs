using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using IMTestProject.Common.Enum;
using IMTestProject.Common;
using IMTestProject.Common.QuerySerialize;

namespace IMTestProject.DAL.Interface
{
    public interface IRepository<T, TPk> where T : class, IEntity<TPk>, new() where TPk : IComparable
    {
        Task<(ExecutionState executionState, string message)> SaveAsync(IDbContextTransaction transaction);
        Task<(T entity, ExecutionState executionState, string message)> CreateAsync(T entity);
        Task<(T entity, ExecutionState executionState, string message)> GetAsync(TPk key);
        Task<(T entity, ExecutionState executionState, string message)> GetAsync(FilterOptions<T> filterOptions = null);
        (IQueryable<T> entity, ExecutionState executionState, string message) List(QueryOptions<T> queryOptions = null, ListCondition listCondition = ListCondition.Normal);
        Task<(ExecutionState executionState, string message)> DoesExistAsync(TPk key);
        Task<(ExecutionState executionState, string message)> DoesExistAsync(FilterOptions<T> filterOptions = null);
        (T entity, ExecutionState executionState, string message) Update(T entity);
        (T entity, ExecutionState executionState, string message) Remove(TPk key);
        Task<(long entityCount, ExecutionState executionState, string message)> CountAsync(CountOptions<T> countOptions = null);
        (T entity, ExecutionState executionState, string message) MarkAsActive(TPk key);
        (T entity, ExecutionState executionState, string message) MarkAsInactive(TPk key);
    }
}