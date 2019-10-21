using IMTestProject.Business.Interface;
using IMTestProject.Common;
using IMTestProject.Common.Const;
using IMTestProject.Common.Enum;
using IMTestProject.Common.QuerySerialize;
using IMTestProject.Service.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IMTestProject.Service.Implementation
{
    /// <summary>
    /// This is base Implementation of Service layer
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TResult">Result model</typeparam>
    /// <typeparam name="TAddDto">Add  model dto</typeparam>
    /// <typeparam name="TEditDto">Edit model dto</typeparam>
    /// <typeparam name="IBusiness">Entity Repository Interface type</typeparam>
    /// <typeparam name="TPk">Primary key type of entity</typeparam>
    public abstract class BaseService<TEntity, TResult, TAddDto, TEditDto, IBusiness, TPk> : IService<TEntity, TResult, TAddDto, TEditDto, TPk>
        where TEntity : class, IEntity<TPk>, new ()
        where TAddDto : class, new ()
        where TEditDto : class, new () where TResult : class, new () where IBusiness : class, IBusiness<TEntity, TPk>
        where TPk : IComparable
    {
        protected IBusiness Business { get; set; }
        
        protected BaseService (IBusiness dao)
        {
            Business = dao;
        }

        public async Task<(TResult entity, ExecutionState executionState, string message)> CreateAsync (TAddDto entity)
        {
            if (entity == null)
            {
                return (entity: null, executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(entity), ExceptionMessage.ValueCannotBeNull));
                //throw new CustomException (nameof (entity), ExceptionMessage.ValueCannotBeNull);
            }
            try
            {

                TEntity toAdd = CastDtoToEntity(entity);
                (TEntity entity, ExecutionState executionState, string message) createdEntity = await Business.CreateAsync(toAdd);

                if (createdEntity.executionState == ExecutionState.Failure)
                {
                    return (entity: null, executionState: createdEntity.executionState, message: createdEntity.message);
                }
                else
                {
                    return (entity: CastEntityToDto(createdEntity.entity), executionState: createdEntity.executionState, message: createdEntity.message);
                    
                }

            }
            catch (Exception ex)
            {
                return (entity: null, executionState: ExecutionState.Failure, message: ex.Message);
            }

        }

        public virtual async Task<(TResult entity, ExecutionState executionState, string message)> GetAsync (TPk key)
        {
            try
            {
                (TEntity entity, ExecutionState executionState, string message) entity = await Business.GetAsync(key);

                if (entity.executionState == ExecutionState.Success)
                {
                    return (entity: CastEntityToDto(entity.entity), executionState: entity.executionState, message: entity.message);
                }
                else
                {
                    return (entity: null, executionState: entity.executionState, message: entity.message);

                }

            }
            catch (Exception ex)
            {
                return (entity: null, executionState: ExecutionState.Failure, message: ex.Message);
            }
        }

        public virtual async Task<(TResult entity, ExecutionState executionState, string message)> GetAsync (FilterOptions<TEntity> filterOptions)
        {
            try
            {
                (TEntity entity, ExecutionState executionState, string message) entity = await Business.GetAsync(filterOptions);

                if (entity.executionState == ExecutionState.Success)
                {
                    return (entity: CastEntityToDto(entity.entity), executionState: entity.executionState, message: entity.message);
                }
                else
                {
                    return (entity: null, executionState: entity.executionState, message: entity.message);

                }

            }
            catch (Exception ex)
            {
                return (entity: null, executionState: ExecutionState.Failure, message: ex.Message);
            }
        }

        public async Task<(IQueryable<TResult> entity, ExecutionState executionState, string message)> List (QueryOptions<TEntity> queryOptions = null, ListCondition listCondition = ListCondition.Normal)
        {
            try
            {
                (IQueryable<TEntity> entity, ExecutionState executionState, string message) itemList = Business.List(queryOptions, listCondition);

                if (itemList.executionState == ExecutionState.Success)
                {
                    return (entity: CastIQueryableEntityToIQueryableDto(itemList.entity), executionState: itemList.executionState, message: itemList.message);
                }
                else
                {
                    return (entity: null, executionState: itemList.executionState, message: itemList.message);

                }

            }
            catch (Exception ex)
            {
                return (entity: null, executionState: ExecutionState.Failure, message: ex.Message);
            }

        }

        public async Task<(ExecutionState executionState, string message)> DoesExistAsync (TPk key)
        {
            try
            {
                return await Business.DoesExistAsync(key);

            }
            catch (Exception ex)
            {
                return (executionState: ExecutionState.Failure, message: ex.Message);
            }
        }

        public async Task<(ExecutionState executionState, string message)> DoesExistAsync (FilterOptions<TEntity> filterOptions)
        {
            try
            {
                return await Business.DoesExistAsync(filterOptions);

            }
            catch (Exception ex)
            {
                return (executionState: ExecutionState.Failure, message: ex.Message);
            }
        }

        public async Task<(TResult entity, ExecutionState executionState, string message)> RemoveAsync (TPk key)
        {
            try
            {
                (TEntity entity, ExecutionState executionState, string message) updateEntity = await Business.RemoveAsync(key);
                if (updateEntity.executionState == ExecutionState.Failure)
                {
                    return (entity: null, executionState: updateEntity.executionState, message: updateEntity.message);
                }
                else
                {
                    return (entity: CastEntityToDto(updateEntity.entity), executionState: updateEntity.executionState, message: updateEntity.message);

                }

            }
            catch (Exception ex)
            {
                return (entity: null, executionState: ExecutionState.Failure, message: ex.Message);
            }
        }

        public async Task<(TResult entity, ExecutionState executionState, string message)> UpdateAsync (TEditDto entity)
        {
            if (entity is null)
            {
                return (entity: null, executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(entity), ExceptionMessage.ValueCannotBeNull));
                //throw new CustomException (nameof (entity), ExceptionMessage.ValueCannotBeNull);
            }
            try
            {
                Task<TEntity> toUpdate = CastDtoToEntityAsync(entity);
                (TEntity entity, ExecutionState executionState, string message) updateEntity = await Business.UpdateAsync(toUpdate.Result);

                if (updateEntity.executionState == ExecutionState.Failure)
                {
                    return (entity: null, executionState: updateEntity.executionState, message: updateEntity.message);
                }
                else
                {
                    return (entity: CastEntityToDto(updateEntity.entity), executionState: updateEntity.executionState, message: updateEntity.message);

                }

            }
            catch (Exception ex)
            {
                return (entity: null, executionState: ExecutionState.Failure, message: ex.Message);
            }
        }

        public virtual async Task<(long entityCount, ExecutionState executionState, string message)> CountAsync (CountOptions<TEntity> countOptions)
        {
            try
            {
                return await Business.CountAsync(countOptions);

            }
            catch (Exception ex)
            {
                return (entityCount: 0, executionState: ExecutionState.Failure, message: ex.Message);
            }
        }

        public virtual async Task<(TResult entity, ExecutionState executionState, string message)> MarkAsActiveAsync (TPk key)
        {
            try
            {
                (TEntity entity, ExecutionState executionState, string message) updateEntity = await Business.MarkAsActiveAsync(key);

                if (updateEntity.executionState == ExecutionState.Success)
                {
                    return (entity: CastEntityToDto(updateEntity.entity), executionState: updateEntity.executionState, message: updateEntity.message);
                }
                else
                {
                    return (entity: null, executionState: updateEntity.executionState, message: updateEntity.message);

                }

            }
            catch (Exception ex)
            {
                return (entity: null, executionState: ExecutionState.Failure, message: ex.Message);
            }
        }

        public virtual async Task<(TResult entity, ExecutionState executionState, string message)> MarkAsInactiveAsync (TPk key)
        {
            try
            {
                (TEntity entity, ExecutionState executionState, string message) updateEntity = await Business.MarkAsInactiveAsync(key);

                if (updateEntity.executionState == ExecutionState.Success)
                {
                    return (entity: CastEntityToDto(updateEntity.entity), executionState: updateEntity.executionState, message: updateEntity.message);
                }
                else
                {
                    return (entity: null, executionState: updateEntity.executionState, message: updateEntity.message);

                }

            }
            catch (Exception ex)
            {
                return (entity: null, executionState: ExecutionState.Failure, message: ex.Message);
            }
        }

        //public async Task<(IQueryable<TResult> entity, ExecutionState executionState, string message)> List(QueryOptionsNodes queryOptionNodes = null)
        //{
        //    try
        //    {
        //        (IQueryable<TEntity> entity, ExecutionState executionState, string message) itemList = Business.List(queryOptionNodes);

        //        if (itemList.executionState == ExecutionState.Success)
        //        {
        //            return (entity: CastIQueryableEntityToIQueryableDto(itemList.entity), executionState: itemList.executionState, message: itemList.message);
        //        }
        //        else
        //        {
        //            return (entity: null, executionState: itemList.executionState, message: itemList.message);

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return (entity: null, executionState: ExecutionState.Failure, message: ex.Message);
        //    }

        //}

        public abstract TEntity CastDtoToEntity (TAddDto source);
        public abstract Task<TEntity> CastDtoToEntityAsync (TEditDto source);
        public abstract TResult CastEntityToDto (TEntity source);
        public abstract IQueryable<TResult> CastIQueryableEntityToIQueryableDto (IQueryable<TEntity> source);

        
    }
}