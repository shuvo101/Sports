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
using IMTestProject.Common.Entity;

namespace IMTestProject.Business.Implementation
{
    public partial class MainTableBusiness : IMainTableBusiness
    {
        IIMTestProjectUnitOfWork UnitOfWork;

        public MainTableBusiness(IIMTestProjectUnitOfWork uow)
        {
            UnitOfWork = uow;
        }

        public async Task<(MainTable entity, ExecutionState executionState, string message)> CreateAsync(MainTable entity)
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
                    (MainTable entity, ExecutionState executionState, string message) createdResponse = await UnitOfWork.MainTables.CreateAsync(entity);

                    if (createdResponse.executionState == ExecutionState.Failure)
                    {
                        return createdResponse;
                    }

                    (ExecutionState executionState, string message) saveRespone = await UnitOfWork.MainTables.SaveAsync(transaction);
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

                    return (entity: null, executionState: ExecutionState.Failure, message: "Problem on MainTable creation");
                }
            }
        }
        public async Task<(MainTable entity, ExecutionState executionState, string message)> GetAsync(int key)
        {
            return await UnitOfWork.MainTables.GetAsync(key);
        }
        public (IQueryable<MainTable> entity, ExecutionState executionState, string message) List(QueryOptions<MainTable> queryOptions, ListCondition listCondition)
        {
            return UnitOfWork.MainTables.List(queryOptions, listCondition);
        }
        public async Task<(MainTable entity, ExecutionState executionState, string message)> GetAsync (FilterOptions<MainTable> filterOptions)
        {
            return await UnitOfWork.MainTables.GetAsync (filterOptions);
        }
        public async Task<(ExecutionState executionState, string message)> DoesExistAsync (int key)
        {
            return await UnitOfWork.MainTables.DoesExistAsync (key);
        }
        public async Task<(ExecutionState executionState, string message)> DoesExistAsync (FilterOptions<MainTable> filterOptions)
        {
            return await UnitOfWork.MainTables.DoesExistAsync (filterOptions);
        }
        public async Task<(MainTable entity, ExecutionState executionState, string message)> UpdateAsync(MainTable entity)
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
                    (MainTable entity, ExecutionState executionState, string message) updatedMainTable = UnitOfWork.MainTables.Update(entity);

                    (ExecutionState executionState, string message) saveResponse = await UnitOfWork.MainTables.SaveAsync(transaction);
                    bool success = saveResponse.executionState == ExecutionState.Success;

                    if (Guid.TryParse(transaction.TransactionId.ToString(), out Guid transactionGuid))
                    {
                        UnitOfWork.Complete(transaction, success ? CompletionState.Success : CompletionState.Error);

                        return success ? updatedMainTable : (entity: null, executionState: saveResponse.executionState, message: saveResponse.message);
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

                    return (entity: null, executionState: ExecutionState.Failure, message: "Problem on MainTable Update");
                }
            }
        }
        public async Task<(MainTable entity, ExecutionState executionState, string message)> RemoveAsync(int key)
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
                    (MainTable entity, ExecutionState executionState, string message) updatedMainTable = UnitOfWork.MainTables.Remove(key);

                    (ExecutionState executionState, string message) saveResponse = await UnitOfWork.MainTables.SaveAsync(transaction);
                    
                    bool success = saveResponse.executionState == ExecutionState.Success;
                    if (Guid.TryParse(transaction.TransactionId.ToString(), out Guid transactionGuid))
                    {
                        UnitOfWork.Complete(transaction, success ? CompletionState.Success : CompletionState.Error);

                        return success ? updatedMainTable : (entity: null, executionState: saveResponse.executionState, message: saveResponse.message);
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

                    return (entity: null, executionState: ExecutionState.Failure, message: "Problem on MainTable delete");
                }
            }
        }
        public async Task<(long entityCount, ExecutionState executionState, string message)> CountAsync (CountOptions<MainTable> countOptions)
        {
            return await UnitOfWork.MainTables.CountAsync (countOptions);
        }
        public async Task<(MainTable entity, ExecutionState executionState, string message)> MarkAsActiveAsync(int key)
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
                    (MainTable entity, ExecutionState executionState, string message) updatedMainTable = UnitOfWork.MainTables.MarkAsActive(key);

                    (ExecutionState executionState, string message) saveResponse = await UnitOfWork.MainTables.SaveAsync(transaction);
                    bool success = saveResponse.executionState == ExecutionState.Success;

                    if (Guid.TryParse(transaction.TransactionId.ToString(), out Guid transactionGuid))
                    {
                        UnitOfWork.Complete(transaction, success ? CompletionState.Success : CompletionState.Error);

                        return success ? updatedMainTable : (entity: null, executionState: saveResponse.executionState, message: saveResponse.message);
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

                    return (entity: null, executionState: ExecutionState.Failure, message: "Problem on MainTable Make Active");
                }
            }
        }
        public async Task<(MainTable entity, ExecutionState executionState, string message)> MarkAsInactiveAsync(int key)
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
                    (MainTable entity, ExecutionState executionState, string message) updatedMainTable = UnitOfWork.MainTables.MarkAsInactive(key);

                    (ExecutionState executionState, string message) saveResponse = await UnitOfWork.MainTables.SaveAsync(transaction);
                    bool success = saveResponse.executionState == ExecutionState.Success;

                    if (Guid.TryParse(transaction.TransactionId.ToString(), out Guid transactionGuid))
                    {
                        UnitOfWork.Complete(transaction, success ? CompletionState.Success : CompletionState.Error);

                        return success ? updatedMainTable : (entity: null, executionState: saveResponse.executionState, message: saveResponse.message);
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

                    return (entity: null, executionState: ExecutionState.Failure, message: "Problem on MainTable Make Inactive");
                }
            }
        }
        public async Task<(MainTable entity, ExecutionState executionState, string message)> GetMainTableByCodeAsync (string code)
        {
            return await UnitOfWork.MainTables.GetMainTableByCode (code);
        }
        public async Task<(ExecutionState executionState, string message)> ValidateCreateAsync(MainTable toMainTable)
        {
            if (string.IsNullOrEmpty(toMainTable.TableName))
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toMainTable.TableName), ExceptionMessage.ValueCannotBeNull));
            }

            if (string.IsNullOrEmpty(toMainTable.TableCode))
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toMainTable.TableCode), ExceptionMessage.ValueCannotBeNull));
            }

            (MainTable MainTable, ExecutionState executionState, string message) checkMainTable = await UnitOfWork.MainTables.GetMainTableByCode(toMainTable.TableCode.ToLower());

            if (checkMainTable.executionState == ExecutionState.Success)
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toMainTable.TableCode), ExceptionMessage.KeyShouldBeUnique));
            }

            return (executionState: ExecutionState.Success, message: "Validation successfully completed");

        }
        public async Task<(ExecutionState executionState, string message)> ValidateUpdateAsync(MainTable toUpdate)
        {
            if (toUpdate.Id <= 0)
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toUpdate.Id), ExceptionMessage.ValueCannotBeNull));
            }

            (MainTable city, ExecutionState executionState, string message) errorIfExistCode = await UnitOfWork.MainTables.GetAsync(toUpdate.Id);

            if (errorIfExistCode.executionState == ExecutionState.Failure)
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toUpdate.Id), ExceptionMessage.EntityNotExists));
            }

            if (string.IsNullOrEmpty(toUpdate.TableName))
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toUpdate.TableName), ExceptionMessage.ValueCannotBeNull));
            }

            if (string.IsNullOrEmpty(toUpdate.TableCode))
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toUpdate.TableCode), ExceptionMessage.ValueCannotBeNull));

            }
            return (executionState: ExecutionState.Success, message: "Validation successfully completed");
        }
        public async Task<(ExecutionState executionState, string message)> ValidateKeyAsync(int key)
        {
            if (key <= 0)
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(key), ExceptionMessage.ValueCannotBeNull));
            }

            (MainTable MainTable, ExecutionState executionState, string message) errorIfExistCode = await UnitOfWork.MainTables.GetAsync(key);

            if (errorIfExistCode.executionState == ExecutionState.Failure)
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(key), ExceptionMessage.EntityNotExists));
            }
            return (executionState: ExecutionState.Success, message: "Validation successfully completed");
        }

    }
}