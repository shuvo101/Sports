using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using IMTestProject.Common.Enum;
using IMTestProject.Common;
using IMTestProject.Common.QuerySerialize;
using IMTestProject.Business.Interface;
using IMTestProject.DAL.Interface;
using IMTestProject.Common.Const;

namespace IMTestProject.Business.Implementation
{
    public partial class ContinentBusiness : IContinentBusiness
    {
        IIMTestProjectUnitOfWork UnitOfWork;

        public ContinentBusiness (IIMTestProjectUnitOfWork uow)
        {
            UnitOfWork = uow;
        }

        public async Task<(Continent entity, ExecutionState executionState, string message)> CreateAsync(Continent entity)
        {
            (ExecutionState executionState, string message) validateResponse = await ValidateCreateAsync (entity);

            if (validateResponse.executionState == ExecutionState.Failure)
            {
                return (entity: null, validateResponse.executionState, validateResponse.message);
            }

            using (IDbContextTransaction transaction = UnitOfWork.Begin())
            {
                try
                {
                    (Continent entity, ExecutionState executionState, string message) createdResponse = await UnitOfWork.Continents.CreateAsync(entity);

                    if (createdResponse.executionState == ExecutionState.Failure)
                    {
                        return createdResponse;
                    }

                    (ExecutionState executionState, string message) saveRespone = await UnitOfWork.Continents.SaveAsync(transaction);
                    bool success = (saveRespone.executionState == ExecutionState.Success);

                    if (Guid.TryParse(transaction.TransactionId.ToString(), out Guid transactionGuid))
                    {
                        UnitOfWork.Complete(transaction, success ? CompletionState.Success : CompletionState.Error);

                        return success ? createdResponse : (entity: null, executionState: saveRespone.executionState, message: saveRespone.message);
                    }
                    else
                    {
                        return (entity: null, executionState: ExecutionState.Failure, message: "Transction not found");
                    }

                }
                catch
                {
                    if (Guid.TryParse(transaction.TransactionId.ToString(), out Guid transactionGuid))
                    {
                        UnitOfWork.Complete(transaction, CompletionState.Error);
                    }

                    return (entity: null, executionState: ExecutionState.Failure, message: "Problem on continent creation");
                }
            }
        }
        public async Task<(Continent entity, ExecutionState executionState, string message)> GetAsync(int key)
        {
            return await UnitOfWork.Continents.GetAsync(key);
        }
        public (IQueryable<Continent> entity, ExecutionState executionState, string message) List(QueryOptions<Continent> queryOptions, ListCondition listCondition)
        {
            return UnitOfWork.Continents.List(queryOptions, listCondition);
        }
        public async Task<(Continent entity, ExecutionState executionState, string message)> GetAsync (FilterOptions<Continent> filterOptions)
        {
            return await UnitOfWork.Continents.GetAsync (filterOptions);
        }
        public async Task<(ExecutionState executionState, string message)> DoesExistAsync (int key)
        {
            return await UnitOfWork.Continents.DoesExistAsync (key);
        }
        public async Task<(ExecutionState executionState, string message)> DoesExistAsync (FilterOptions<Continent> filterOptions)
        {
            return await UnitOfWork.Continents.DoesExistAsync (filterOptions);
        }
        public async Task<(Continent entity, ExecutionState executionState, string message)> UpdateAsync(Continent entity)
        {
            (ExecutionState executionState, string message) validateResponse = await ValidateUpdateAsync(entity);
            if (validateResponse.executionState == ExecutionState.Failure)
            {
                return (entity: null, validateResponse.executionState, validateResponse.message);
            }

            using (IDbContextTransaction transaction = UnitOfWork.Begin())
            {
                try
                {
                    (Continent entity, ExecutionState executionState, string message) updatedContinent = UnitOfWork.Continents.Update(entity);

                    (ExecutionState executionState, string message) saveResponse = await UnitOfWork.Continents.SaveAsync(transaction);
                    bool success = saveResponse.executionState == ExecutionState.Success;

                    if (Guid.TryParse(transaction.TransactionId.ToString(), out Guid transactionGuid))
                    {
                        UnitOfWork.Complete(transaction, success ? CompletionState.Success : CompletionState.Error);

                        return success ? updatedContinent : (entity: null, executionState: saveResponse.executionState, message: saveResponse.message);
                    }
                    else
                    {
                        return (entity: null, executionState: ExecutionState.Failure, message: "Transction not found");
                    }
                }
                catch
                {
                    if (Guid.TryParse(transaction.TransactionId.ToString(), out Guid transactionGuid))
                    {
                        UnitOfWork.Complete(transaction, CompletionState.Error);
                    }

                    return (entity: null, executionState: ExecutionState.Failure, message: "Problem on continent Update");
                }
            }
        }
        public async Task<(Continent entity, ExecutionState executionState, string message)> RemoveAsync(int key)
        {
            (ExecutionState executionState, string message) validateResponse = await ValidateKeyAsync(key);
            if (validateResponse.executionState == ExecutionState.Failure)
            {
                return (entity: null, validateResponse.executionState, validateResponse.message);
            }

            using (IDbContextTransaction transaction = UnitOfWork.Begin())
            {
                try
                {
                    (Continent entity, ExecutionState executionState, string message) updatedContinent = UnitOfWork.Continents.Remove(key);

                    (ExecutionState executionState, string message) saveResponse = await UnitOfWork.Continents.SaveAsync(transaction);
                    
                    bool success = saveResponse.executionState == ExecutionState.Success;
                    if (Guid.TryParse(transaction.TransactionId.ToString(), out Guid transactionGuid))
                    {
                        UnitOfWork.Complete(transaction, success ? CompletionState.Success : CompletionState.Error);

                        return success ? updatedContinent : (entity: null, executionState: saveResponse.executionState, message: saveResponse.message);
                    }
                    else
                    {
                        return (entity: null, executionState: ExecutionState.Failure, message: "Transction not found");
                    }
                }
                catch
                {
                    if (Guid.TryParse(transaction.TransactionId.ToString(), out Guid transactionGuid))
                    {
                        UnitOfWork.Complete(transaction, CompletionState.Error);
                    }

                    return (entity: null, executionState: ExecutionState.Failure, message: "Problem on continent delete");
                }
            }
        }
        public async Task<(long entityCount, ExecutionState executionState, string message)> CountAsync (CountOptions<Continent> countOptions)
        {
            return await UnitOfWork.Continents.CountAsync (countOptions);
        }
        public async Task<(Continent entity, ExecutionState executionState, string message)> MarkAsActiveAsync(int key)
        {
            (ExecutionState executionState, string message) validateResponse = await ValidateKeyAsync(key);
            if (validateResponse.executionState == ExecutionState.Failure)
            {
                return (entity: null, validateResponse.executionState, validateResponse.message);
            }
            using (IDbContextTransaction transaction = UnitOfWork.Begin())
            {
                try
                {
                    (Continent entity, ExecutionState executionState, string message) updatedContinent = UnitOfWork.Continents.MarkAsActive(key);

                    (ExecutionState executionState, string message) saveResponse = await UnitOfWork.Continents.SaveAsync(transaction);
                    bool success = saveResponse.executionState == ExecutionState.Success;

                    if (Guid.TryParse(transaction.TransactionId.ToString(), out Guid transactionGuid))
                    {
                        UnitOfWork.Complete(transaction, success ? CompletionState.Success : CompletionState.Error);

                        return success ? updatedContinent : (entity: null, executionState: saveResponse.executionState, message: saveResponse.message);
                    }
                    else
                    {
                        return (entity: null, executionState: ExecutionState.Failure, message: "Transction not found");
                    }
                }
                catch
                {
                    if (Guid.TryParse(transaction.TransactionId.ToString(), out Guid transactionGuid))
                    {
                        UnitOfWork.Complete(transaction, CompletionState.Error);
                    }

                    return (entity: null, executionState: ExecutionState.Failure, message: "Problem on continent Make Active");
                }
            }
        }
        public async Task<(Continent entity, ExecutionState executionState, string message)> MarkAsInactiveAsync(int key)
        {
            (ExecutionState executionState, string message) validateResponse = await ValidateKeyAsync(key);
            if (validateResponse.executionState == ExecutionState.Failure)
            {
                return (entity: null, validateResponse.executionState, validateResponse.message);
            }
            using (IDbContextTransaction transaction = UnitOfWork.Begin())
            {
                try
                {
                    (Continent entity, ExecutionState executionState, string message) updatedContinent = UnitOfWork.Continents.MarkAsInactive(key);

                    (ExecutionState executionState, string message) saveResponse = await UnitOfWork.Continents.SaveAsync(transaction);
                    bool success = saveResponse.executionState == ExecutionState.Success;

                    if (Guid.TryParse(transaction.TransactionId.ToString(), out Guid transactionGuid))
                    {
                        UnitOfWork.Complete(transaction, success ? CompletionState.Success : CompletionState.Error);

                        return success ? updatedContinent : (entity: null, executionState: saveResponse.executionState, message: saveResponse.message);
                    }
                    else
                    {
                        return (entity: null, executionState: ExecutionState.Failure, message: "Transction not found");
                    }
                }
                catch
                {
                    if (Guid.TryParse(transaction.TransactionId.ToString(), out Guid transactionGuid))
                    {
                        UnitOfWork.Complete(transaction, CompletionState.Error);
                    }

                    return (entity: null, executionState: ExecutionState.Failure, message: "Problem on continent Make Inactive");
                }
            }
        }
        public async Task<(Continent entity, ExecutionState executionState, string message)> GetContinentByCodeAsync (string code)
        {
            return await UnitOfWork.Continents.GetContinentByCode (code);
        }
        public async Task<(ExecutionState executionState, string message)> ValidateCreateAsync(Continent toContinent)
        {
            if (string.IsNullOrEmpty(toContinent.Name))
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toContinent.Name), ExceptionMessage.ValueCannotBeNull));
            }

            if (string.IsNullOrEmpty(toContinent.Code))
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toContinent.Code), ExceptionMessage.ValueCannotBeNull));
            }

            (Continent continent, ExecutionState executionState, string message) checkcontinent = await UnitOfWork.Continents.GetContinentByCode(toContinent.Code.ToLower());

            if (checkcontinent.executionState == ExecutionState.Success)
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toContinent.Code), ExceptionMessage.KeyShouldBeUnique));
            }

            return (executionState: ExecutionState.Success, message: "Validation successfully completed");

        }
        public async Task<(ExecutionState executionState, string message)> ValidateUpdateAsync(Continent toUpdate)
        {
            if (toUpdate.Id <= 0)
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toUpdate.Id), ExceptionMessage.ValueCannotBeNull));
            }

            (Continent city, ExecutionState executionState, string message) errorIfExistCode = await UnitOfWork.Continents.GetAsync(toUpdate.Id);

            if (errorIfExistCode.executionState == ExecutionState.Failure)
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toUpdate.Id), ExceptionMessage.EntityNotExists));
            }

            if (string.IsNullOrEmpty(toUpdate.Name))
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toUpdate.Name), ExceptionMessage.ValueCannotBeNull));
            }

            if (string.IsNullOrEmpty(toUpdate.Code))
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toUpdate.Code), ExceptionMessage.ValueCannotBeNull));

            }
            return (executionState: ExecutionState.Success, message: "Validation successfully completed");
        }
        public async Task<(ExecutionState executionState, string message)> ValidateKeyAsync(int key)
        {
            if (key <= 0)
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(key), ExceptionMessage.ValueCannotBeNull));
            }

            (Continent continent, ExecutionState executionState, string message) errorIfExistCode = await UnitOfWork.Continents.GetAsync(key);

            if (errorIfExistCode.executionState == ExecutionState.Failure)
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(key), ExceptionMessage.EntityNotExists));
            }
            return (executionState: ExecutionState.Success, message: "Validation successfully completed");
        }

    }
}