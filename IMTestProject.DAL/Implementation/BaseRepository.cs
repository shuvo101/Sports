using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Query;
using IMTestProject.DAL.Interface;
using System.Linq.Expressions;
using IMTestProject.Common;
using IMTestProject.Common.Enum;
using IMTestProject.Common.QuerySerialize;

namespace IMTestProject.DAL.Implementation
{
    /// <summary>
    /// Base Repository that implement base functions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TPk">Entity Unique Key type</typeparam>
    public abstract class BaseRepository<T, TPk> : IRepository<T, TPk> where T : class, IEntity<TPk>, new() where TPk : IComparable
    {
        private readonly DbContext WriteOnlyCtx;
        private readonly DbContext ReadOnlyCtx;

        private readonly DbSet<T> WriteOnlySet;
        private readonly DbSet<T> ReadOnlySet;
        static bool _hasAuditProperties = typeof(T).IsSubclassOf(typeof(AuditEntity<TPk>));
        
        protected BaseRepository(DbContext readOnlyCtx, DbContext writeOnlyCtx)
        {
            this.WriteOnlyCtx = writeOnlyCtx;
            this.ReadOnlyCtx = readOnlyCtx;

            this.WriteOnlySet = this.WriteOnlyCtx.Set<T>();
            this.ReadOnlySet = this.ReadOnlyCtx.Set<T>();
        }

        public virtual async Task<(ExecutionState executionState, string message)> SaveAsync(IDbContextTransaction transaction)
        {
            if (transaction != null)
            {
                if (Guid.TryParse(transaction.TransactionId.ToString(), out Guid transactionGuid))
                {
                    try
                    {
                        await WriteOnlyCtx.SaveChangesAsync();

                        return (executionState: ExecutionState.Success, message: "Transaction completed");
                    }
                    catch (Exception ex)
                    {
                        return (executionState: ExecutionState.Failure, message: ex.Message);
                    }
                }
                else
                {
                    return (executionState: ExecutionState.Failure, message: "Transaction not found");
                }
            }
            else
            {
                return (executionState: ExecutionState.Failure, message: "Transaction not found");
            }
        }

        public virtual async Task<(T entity, ExecutionState executionState, string message)> CreateAsync(T entity)
        {
            try
            {
                if (_hasAuditProperties)
                {
                    EntityCreated(entity);
                }
                entity.EntityActivated();
                await WriteOnlySet.AddAsync(entity);

                return (entity, ExecutionState.Created, "Item added");
            }
            catch (Exception ex)
            {
                return (entity: null, executionState: ExecutionState.Failure, message: ex.Message);
            }
        }

        public virtual async Task<(T entity, ExecutionState executionState, string message)> GetAsync(TPk id)
        {
            try
            {
                T entity = await ReadOnlySet.FirstOrDefaultAsync(x => x.Id.Equals(id));
                if (entity != null)
                {
                    return (entity, ExecutionState.Success, "Item found.");
                }
                else
                {
                    return (entity: null, ExecutionState.Failure, "Item not found.");
                }
                
            }
            catch (Exception ex)
            {
                return (null, executionState: ExecutionState.Failure, message: ex.Message);
            }
        }

        public virtual async Task<(T entity, ExecutionState executionState, string message)> GetAsync(FilterOptions<T> filterOptions = null)
        {
            if (filterOptions != null)
            {
                IQueryable<T> query = ReadOnlySet;

                Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>> includeExpression = filterOptions.IncludeExpression;
                Expression<Func<T, bool>> filterExpression = filterOptions.FilterExpression;

                // apply include
                if (includeExpression != null)
                {
                    query = (includeExpression.Compile())(query);
                }

                // apply filter
                if (filterExpression != null)
                {
                    query = query.Where(filterExpression.Compile()).AsQueryable();
                }


                try
                {
                    T entity = await query.FirstOrDefaultAsync();
                    
                    if (entity != null)
                    {
                        return (entity, executionState: ExecutionState.Success, message: "Item found");
                    }
                    else
                    {
                        return (entity: null, executionState: ExecutionState.Failure, message: "Item not found");
                    }

                    
                }
                catch (Exception ex)
                {
                    return (entity: null, executionState: ExecutionState.Failure, message: ex.Message);
                    
                }
            }
            else
            {
                return (entity: null, executionState: ExecutionState.Failure, message: "filter Options not inserted.");
                
            }
        }

        public virtual (IQueryable<T> entity, ExecutionState executionState, string message) List(
            QueryOptions<T> queryOptions = null,
            ListCondition listCondition = ListCondition.Normal)
        {
            IQueryable<T> query = null;

            if (listCondition == ListCondition.Normal)
            {
                query = ReadOnlySet;
            }
            else
            {
                query = ReadOnlySet.IgnoreQueryFilters();
                if (_hasAuditProperties)
                {
                    query = FilteredWithDelete(query);
                }
            }

            if (queryOptions != null)
            {
                Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>> includeExpression = queryOptions.IncludeExpression;
                Expression<Func<T, bool>> filterExpression = queryOptions.FilterExpression;
                Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> sortExpression = queryOptions.SortingExpression;
                Pagination pagination = queryOptions.Pagination;

                // apply include
                if (includeExpression != null)
                {
                    query = (includeExpression.Compile())(query);
                }

                // apply filter
                if (filterExpression != null)
                {
                    query = query
                        .Where(filterExpression.Compile())
                        .AsQueryable();
                }

                // apply sorting
                if (sortExpression != null)
                {
                    query = (sortExpression.Compile())(query);
                }

                // apply pagination
                if (pagination != null)
                {
                    query = query.Skip(pagination.Start).Take(pagination.Limit);
                }
            }

            try
            {
                IQueryable<T> entities = query.AsQueryable();
                if(entities.Count() > 0)
                {
                    return (entity: entities, executionState: ExecutionState.Success, message: "Items found.");
                }
                else
                {
                    return (entity: entities, executionState: ExecutionState.Failure, message: "Item not found.");
                }
            }
            catch (Exception ex)
            {
                return (entity: null, executionState: ExecutionState.Failure, message: ex.Message.ToString());
            }
        }

        public virtual async Task<(ExecutionState executionState, string message)> DoesExistAsync(TPk key)
        {
            bool DoesExist = false;

            try
            {
                T entity = await ReadOnlySet.FirstOrDefaultAsync(x => x.Id.Equals(key));                
                DoesExist = entity != null;
            }
            catch (Exception ex)
            {
                return (executionState: ExecutionState.Failure, message: ex.Message.ToString());
            }

            return ( executionState: (DoesExist)? ExecutionState.Success: ExecutionState.Failure, message: (DoesExist) ? "Item found." : "Item Not Found");
        }

        public virtual async Task<(ExecutionState executionState, string message)> DoesExistAsync(FilterOptions<T> filterOptions = null)
        {
            bool DoesExist = false;

            if (filterOptions != null)            
            {
                Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>> includeExpression = filterOptions.IncludeExpression;
                Expression<Func<T, bool>> filterExpression = filterOptions.FilterExpression;

                IQueryable<T> query = ReadOnlySet;

                // apply include
                if (includeExpression != null)
                {
                    query = (includeExpression.Compile())(query);
                }

                // apply filter
                if (filterExpression != null)
                {
                    query = query.Where(filterExpression.Compile()).AsQueryable();
                }

                try
                {
                    int result = await query.CountAsync();

                    if (result > 0)
                    {
                        DoesExist = true;
                    }
                }
                catch (Exception ex)
                {
                    return (executionState: ExecutionState.Failure, message: ex.Message.ToString());
                }
            }
            else
            {
                return (executionState: ExecutionState.Failure, message: "filter Options not found.");
            }
            return (executionState: (DoesExist) ? ExecutionState.Success : ExecutionState.Failure, message: (DoesExist) ? "Item found." : "Item Not Found");
        }

        public virtual (T entity, ExecutionState executionState, string message) Update(T entity)
        {
            try
            {
                if (_hasAuditProperties)
                {
                    EntityUpdated(entity);
                }
                WriteOnlySet.Add(entity).State = EntityState.Modified;
                return (entity, executionState: ExecutionState.Updated, message: "Update Sucessfully.");
            }
            catch (Exception ex)
            {
                return (entity: null, executionState: ExecutionState.Failure, message: ex.Message.ToString());
            }
        }

        public virtual (T entity, ExecutionState executionState, string message) Remove(TPk id)
        {
            try
            {
                T entity = WriteOnlySet.Find(id);

                if (entity != null)
                {
                    if (_hasAuditProperties)
                    {
                        EntityRemoved(entity);
                        WriteOnlySet.Update(entity);
                    }
                    else
                    {
                        WriteOnlySet.Remove(entity);
                    }

                    return (entity, executionState: ExecutionState.Deleted, message: "Item remove sucessfully.");
                }
                else
                {
                    return (entity : null, executionState: ExecutionState.Failure, message: "Item not found.");
                }
            }
            catch (Exception ex)
            {
                return (entity: null, executionState: ExecutionState.Failure, message: ex.Message.ToString());
            }
        }

        public async virtual Task<(long entityCount, ExecutionState executionState, string message)> CountAsync(CountOptions<T> countOptions = null)
        {
            long count = 0;

            if (countOptions != null)
            {
                Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>> includeExpression = countOptions.IncludeExpression;
                Expression<Func<T, bool>> filterExpression = countOptions.FilterExpression;

                IQueryable<T> query = ReadOnlySet;

                // apply include
                if (includeExpression != null)
                {
                    query = (includeExpression.Compile())(query);
                }

                // apply filter
                if (filterExpression != null)
                {
                    query = query.Where(filterExpression.Compile()).AsQueryable();
                }

                try
                {
                    count = await query.AsQueryable().CountAsync();
                    
                }
                catch (Exception ex)
                {
                    return (entityCount: 0, executionState: ExecutionState.Failure, message: ex.Message.ToString());
                }
            }
            else
            {
                return (entityCount: count, executionState: ExecutionState.Failure, message: "count Options not found.");
            }

            return (entityCount: count, executionState: ExecutionState.Success, message: "Item found");
        }

        public virtual (T entity, ExecutionState executionState, string message) MarkAsActive(TPk id)
        {
            try
            {
                T entity = WriteOnlySet.IgnoreQueryFilters().FirstOrDefault(x => x.Id.Equals(id));

                if (entity != null)
                {
                    //entity.IsActive = true;
                    entity.EntityActivated();

                    WriteOnlyCtx.Attach(entity);

                    WriteOnlyCtx.Entry(entity).Property(x => x.IsActive).IsModified = true;
                    return (entity , executionState: ExecutionState.Success, message: "Item is Mark as active.");
                    
                }
                else
                {
                    return (entity: null, executionState: ExecutionState.Failure, message: "Item not found.");
                }
            }
            catch (Exception ex)
            {
                return (entity : null, executionState: ExecutionState.Failure, message: ex.Message.ToString());
            }
        }

        public virtual (T entity, ExecutionState executionState, string message) MarkAsInactive(TPk id)
        {
            try
            {
                T entity = WriteOnlySet.IgnoreQueryFilters().FirstOrDefault(x => x.Id.Equals(id));

                if (entity != null)
                {
                    //entity.IsActive = false;
                    entity.EntityInactivated();

                    WriteOnlyCtx.Attach(entity);

                    WriteOnlyCtx.Entry(entity).Property(x => x.IsActive).IsModified = true;

                    return (entity, executionState: ExecutionState.Success, message: "Item is Mark as active.");
                }
                else
                {
                    return (entity: null, executionState: ExecutionState.Failure, message: "Item not found.");
                }
            }
            catch (Exception ex)
            {
                return (entity: null, executionState: ExecutionState.Failure, message: ex.Message.ToString());
            }
        }

        public abstract void EntityCreated(T entity);
        public abstract void EntityUpdated(T entity);
        public abstract void EntityRemoved(T entity);

        public abstract IQueryable<T> FilteredWithDelete(IQueryable<T> query);

        public void Dispose()
        {
            WriteOnlyCtx?.Dispose();
            ReadOnlyCtx?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}