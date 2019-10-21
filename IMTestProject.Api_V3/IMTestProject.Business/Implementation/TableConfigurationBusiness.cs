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
    public partial class TableConfigurationBusiness : ITableConfigurationBusiness
    {
        IIMTestProjectUnitOfWork UnitOfWork;

        public TableConfigurationBusiness(IIMTestProjectUnitOfWork uow)
        {
            UnitOfWork = uow;
        }

        public async Task<(TableConfiguration entity, ExecutionState executionState, string message)> CreateAsync(TableConfiguration entity)
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
                    (TableConfiguration entity, ExecutionState executionState, string message) createdResponse = await UnitOfWork.TableConfigurations.CreateAsync(entity);

                    if (createdResponse.executionState == ExecutionState.Failure)
                    {
                        return createdResponse;
                    }

                    (ExecutionState executionState, string message) saveRespone = await UnitOfWork.TableConfigurations.SaveAsync(transaction);
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

                    return (entity: null, executionState: ExecutionState.Failure, message: "Problem on TableConfiguration creation");
                }
            }
        }
        public async Task<(TableConfiguration entity, ExecutionState executionState, string message)> GetAsync(int key)
        {
            return await UnitOfWork.TableConfigurations.GetAsync(key);
        }
        public (IQueryable<TableConfiguration> entity, ExecutionState executionState, string message) List(QueryOptions<TableConfiguration> queryOptions, ListCondition listCondition)
        {
            return UnitOfWork.TableConfigurations.List(queryOptions, listCondition);
        }
        public async Task<(TableConfiguration entity, ExecutionState executionState, string message)> GetAsync (FilterOptions<TableConfiguration> filterOptions)
        {
            return await UnitOfWork.TableConfigurations.GetAsync (filterOptions);
        }
        public async Task<(ExecutionState executionState, string message)> DoesExistAsync (int key)
        {
            return await UnitOfWork.TableConfigurations.DoesExistAsync (key);
        }
        public async Task<(ExecutionState executionState, string message)> DoesExistAsync (FilterOptions<TableConfiguration> filterOptions)
        {
            return await UnitOfWork.TableConfigurations.DoesExistAsync (filterOptions);
        }
        public async Task<(TableConfiguration entity, ExecutionState executionState, string message)> UpdateAsync(TableConfiguration entity)
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
                    (TableConfiguration entity, ExecutionState executionState, string message) updatedTableConfiguration = UnitOfWork.TableConfigurations.Update(entity);

                    (ExecutionState executionState, string message) saveResponse = await UnitOfWork.TableConfigurations.SaveAsync(transaction);
                    bool success = saveResponse.executionState == ExecutionState.Success;

                    if (Guid.TryParse(transaction.TransactionId.ToString(), out Guid transactionGuid))
                    {
                        UnitOfWork.Complete(transaction, success ? CompletionState.Success : CompletionState.Error);

                        return success ? updatedTableConfiguration : (entity: null, executionState: saveResponse.executionState, message: saveResponse.message);
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

                    return (entity: null, executionState: ExecutionState.Failure, message: "Problem on TableConfiguration Update");
                }
            }
        }
        public async Task<(TableConfiguration entity, ExecutionState executionState, string message)> RemoveAsync(int key)
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
                    (TableConfiguration entity, ExecutionState executionState, string message) updatedTableConfiguration = UnitOfWork.TableConfigurations.Remove(key);

                    (ExecutionState executionState, string message) saveResponse = await UnitOfWork.TableConfigurations.SaveAsync(transaction);
                    
                    bool success = saveResponse.executionState == ExecutionState.Success;
                    if (Guid.TryParse(transaction.TransactionId.ToString(), out Guid transactionGuid))
                    {
                        UnitOfWork.Complete(transaction, success ? CompletionState.Success : CompletionState.Error);

                        return success ? updatedTableConfiguration : (entity: null, executionState: saveResponse.executionState, message: saveResponse.message);
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

                    return (entity: null, executionState: ExecutionState.Failure, message: "Problem on TableConfiguration delete");
                }
            }
        }
        public async Task<(long entityCount, ExecutionState executionState, string message)> CountAsync (CountOptions<TableConfiguration> countOptions)
        {
            return await UnitOfWork.TableConfigurations.CountAsync (countOptions);
        }
        public async Task<(TableConfiguration entity, ExecutionState executionState, string message)> MarkAsActiveAsync(int key)
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
                    (TableConfiguration entity, ExecutionState executionState, string message) updatedTableConfiguration = UnitOfWork.TableConfigurations.MarkAsActive(key);

                    (ExecutionState executionState, string message) saveResponse = await UnitOfWork.TableConfigurations.SaveAsync(transaction);
                    bool success = saveResponse.executionState == ExecutionState.Success;

                    if (Guid.TryParse(transaction.TransactionId.ToString(), out Guid transactionGuid))
                    {
                        UnitOfWork.Complete(transaction, success ? CompletionState.Success : CompletionState.Error);

                        return success ? updatedTableConfiguration : (entity: null, executionState: saveResponse.executionState, message: saveResponse.message);
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

                    return (entity: null, executionState: ExecutionState.Failure, message: "Problem on TableConfiguration Make Active");
                }
            }
        }
        public async Task<(TableConfiguration entity, ExecutionState executionState, string message)> MarkAsInactiveAsync(int key)
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
                    (TableConfiguration entity, ExecutionState executionState, string message) updatedTableConfiguration = UnitOfWork.TableConfigurations.MarkAsInactive(key);

                    (ExecutionState executionState, string message) saveResponse = await UnitOfWork.TableConfigurations.SaveAsync(transaction);
                    bool success = saveResponse.executionState == ExecutionState.Success;

                    if (Guid.TryParse(transaction.TransactionId.ToString(), out Guid transactionGuid))
                    {
                        UnitOfWork.Complete(transaction, success ? CompletionState.Success : CompletionState.Error);

                        return success ? updatedTableConfiguration : (entity: null, executionState: saveResponse.executionState, message: saveResponse.message);
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

                    return (entity: null, executionState: ExecutionState.Failure, message: "Problem on TableConfiguration Make Inactive");
                }
            }
        }
        public async Task<(TableConfiguration entity, ExecutionState executionState, string message)> GetTableConfigurationByCodeAsync (string code)
        {
            return await UnitOfWork.TableConfigurations.GetTableConfigurationByCode (code);
        }
        public async Task<(ExecutionState executionState, string message)> ValidateCreateAsync(TableConfiguration toTableConfiguration)
        {
            if (string.IsNullOrEmpty(toTableConfiguration.Name))
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toTableConfiguration.Name), ExceptionMessage.ValueCannotBeNull));
            }

            if (string.IsNullOrEmpty(toTableConfiguration.DisplayName))
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toTableConfiguration.DisplayName), ExceptionMessage.ValueCannotBeNull));
            }

            (TableConfiguration TableConfiguration, ExecutionState executionState, string message) checkTableConfiguration = await UnitOfWork.TableConfigurations.GetTableConfigurationByCode(toTableConfiguration.Name.ToLower());

            if (checkTableConfiguration.executionState == ExecutionState.Success)
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toTableConfiguration.Name), ExceptionMessage.KeyShouldBeUnique));
            }

            return (executionState: ExecutionState.Success, message: "Validation successfully completed");

        }
        public async Task<(ExecutionState executionState, string message)> ValidateUpdateAsync(TableConfiguration toUpdate)
        {
            if (toUpdate.Id <= 0)
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toUpdate.Id), ExceptionMessage.ValueCannotBeNull));
            }

            (TableConfiguration city, ExecutionState executionState, string message) errorIfExistCode = await UnitOfWork.TableConfigurations.GetAsync(toUpdate.Id);

            if (errorIfExistCode.executionState == ExecutionState.Failure)
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toUpdate.Id), ExceptionMessage.EntityNotExists));
            }

            if (string.IsNullOrEmpty(toUpdate.Name))
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toUpdate.Name), ExceptionMessage.ValueCannotBeNull));
            }

            if (string.IsNullOrEmpty(toUpdate.DisplayName))
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toUpdate.DisplayName), ExceptionMessage.ValueCannotBeNull));

            }
            return (executionState: ExecutionState.Success, message: "Validation successfully completed");
        }
        public async Task<(ExecutionState executionState, string message)> ValidateKeyAsync(int key)
        {
            if (key <= 0)
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(key), ExceptionMessage.ValueCannotBeNull));
            }

            (TableConfiguration TableConfiguration, ExecutionState executionState, string message) errorIfExistCode = await UnitOfWork.TableConfigurations.GetAsync(key);

            if (errorIfExistCode.executionState == ExecutionState.Failure)
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(key), ExceptionMessage.EntityNotExists));
            }
            return (executionState: ExecutionState.Success, message: "Validation successfully completed");
        }

    }
}