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
    public partial class AdditionalInformationBusiness : IAdditionalInformationBusiness
    {
        IIMTestProjectUnitOfWork UnitOfWork;

        public AdditionalInformationBusiness(IIMTestProjectUnitOfWork uow)
        {
            UnitOfWork = uow;
        }

        public async Task<(AdditionalInformation entity, ExecutionState executionState, string message)> CreateAsync(AdditionalInformation entity)
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
                    (AdditionalInformation entity, ExecutionState executionState, string message) createdResponse = await UnitOfWork.TableMappingDatas.CreateAsync(entity);

                    if (createdResponse.executionState == ExecutionState.Failure)
                    {
                        return createdResponse;
                    }

                    (ExecutionState executionState, string message) saveRespone = await UnitOfWork.TableMappingDatas.SaveAsync(transaction);
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

                    return (entity: null, executionState: ExecutionState.Failure, message: "Problem on TableMappingData creation");
                }
            }
        }
        public async Task<(AdditionalInformation entity, ExecutionState executionState, string message)> GetAsync(int key)
        {
            return await UnitOfWork.TableMappingDatas.GetAsync(key);
        }
        public (IQueryable<AdditionalInformation> entity, ExecutionState executionState, string message) List(QueryOptions<AdditionalInformation> queryOptions, ListCondition listCondition)
        {
            return UnitOfWork.TableMappingDatas.List(queryOptions, listCondition);
        }
        public async Task<(AdditionalInformation entity, ExecutionState executionState, string message)> GetAsync (FilterOptions<AdditionalInformation> filterOptions)
        {
            return await UnitOfWork.TableMappingDatas.GetAsync (filterOptions);
        }
        public async Task<(ExecutionState executionState, string message)> DoesExistAsync (int key)
        {
            return await UnitOfWork.TableMappingDatas.DoesExistAsync (key);
        }
        public async Task<(ExecutionState executionState, string message)> DoesExistAsync (FilterOptions<AdditionalInformation> filterOptions)
        {
            return await UnitOfWork.TableMappingDatas.DoesExistAsync (filterOptions);
        }
        public async Task<(AdditionalInformation entity, ExecutionState executionState, string message)> UpdateAsync(AdditionalInformation entity)
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
                    (AdditionalInformation entity, ExecutionState executionState, string message) updatedTableMappingData = UnitOfWork.TableMappingDatas.Update(entity);

                    (ExecutionState executionState, string message) saveResponse = await UnitOfWork.TableMappingDatas.SaveAsync(transaction);
                    bool success = saveResponse.executionState == ExecutionState.Success;

                    if (Guid.TryParse(transaction.TransactionId.ToString(), out Guid transactionGuid))
                    {
                        UnitOfWork.Complete(transaction, success ? CompletionState.Success : CompletionState.Error);

                        return success ? updatedTableMappingData : (entity: null, executionState: saveResponse.executionState, message: saveResponse.message);
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

                    return (entity: null, executionState: ExecutionState.Failure, message: "Problem on TableMappingData Update");
                }
            }
        }
        public async Task<(AdditionalInformation entity, ExecutionState executionState, string message)> RemoveAsync(int key)
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
                    (AdditionalInformation entity, ExecutionState executionState, string message) updatedTableMappingData = UnitOfWork.TableMappingDatas.Remove(key);

                    (ExecutionState executionState, string message) saveResponse = await UnitOfWork.TableMappingDatas.SaveAsync(transaction);
                    
                    bool success = saveResponse.executionState == ExecutionState.Success;
                    if (Guid.TryParse(transaction.TransactionId.ToString(), out Guid transactionGuid))
                    {
                        UnitOfWork.Complete(transaction, success ? CompletionState.Success : CompletionState.Error);

                        return success ? updatedTableMappingData : (entity: null, executionState: saveResponse.executionState, message: saveResponse.message);
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

                    return (entity: null, executionState: ExecutionState.Failure, message: "Problem on TableMappingData delete");
                }
            }
        }
        public async Task<(long entityCount, ExecutionState executionState, string message)> CountAsync (CountOptions<AdditionalInformation> countOptions)
        {
            return await UnitOfWork.TableMappingDatas.CountAsync (countOptions);
        }
        public async Task<(AdditionalInformation entity, ExecutionState executionState, string message)> MarkAsActiveAsync(int key)
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
                    (AdditionalInformation entity, ExecutionState executionState, string message) updatedTableMappingData = UnitOfWork.TableMappingDatas.MarkAsActive(key);

                    (ExecutionState executionState, string message) saveResponse = await UnitOfWork.TableMappingDatas.SaveAsync(transaction);
                    bool success = saveResponse.executionState == ExecutionState.Success;

                    if (Guid.TryParse(transaction.TransactionId.ToString(), out Guid transactionGuid))
                    {
                        UnitOfWork.Complete(transaction, success ? CompletionState.Success : CompletionState.Error);

                        return success ? updatedTableMappingData : (entity: null, executionState: saveResponse.executionState, message: saveResponse.message);
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

                    return (entity: null, executionState: ExecutionState.Failure, message: "Problem on TableMappingData Make Active");
                }
            }
        }
        public async Task<(AdditionalInformation entity, ExecutionState executionState, string message)> MarkAsInactiveAsync(int key)
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
                    (AdditionalInformation entity, ExecutionState executionState, string message) updatedTableMappingData = UnitOfWork.TableMappingDatas.MarkAsInactive(key);

                    (ExecutionState executionState, string message) saveResponse = await UnitOfWork.TableMappingDatas.SaveAsync(transaction);
                    bool success = saveResponse.executionState == ExecutionState.Success;

                    if (Guid.TryParse(transaction.TransactionId.ToString(), out Guid transactionGuid))
                    {
                        UnitOfWork.Complete(transaction, success ? CompletionState.Success : CompletionState.Error);

                        return success ? updatedTableMappingData : (entity: null, executionState: saveResponse.executionState, message: saveResponse.message);
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

                    return (entity: null, executionState: ExecutionState.Failure, message: "Problem on TableMappingData Make Inactive");
                }
            }
        }
        public async Task<(AdditionalInformation entity, ExecutionState executionState, string message)> GetTableMappingDataByCodeAsync (string code)
        {
            return await UnitOfWork.TableMappingDatas.GetAdditionalInformationByCode (code);
        }
        public async Task<(ExecutionState executionState, string message)> ValidateCreateAsync(AdditionalInformation toTableMappingData)
        {
            if (string.IsNullOrEmpty(toTableMappingData.Value))
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toTableMappingData.Value), ExceptionMessage.ValueCannotBeNull));
            }

            if (string.IsNullOrEmpty(toTableMappingData.Code))
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toTableMappingData.Code), ExceptionMessage.ValueCannotBeNull));
            }

            (AdditionalInformation TableMappingData, ExecutionState executionState, string message) checkTableMappingData = await UnitOfWork.TableMappingDatas.GetAdditionalInformationByCode(toTableMappingData.Code.ToLower());

            if (checkTableMappingData.executionState == ExecutionState.Success)
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toTableMappingData.Code), ExceptionMessage.KeyShouldBeUnique));
            }

            return (executionState: ExecutionState.Success, message: "Validation successfully completed");

        }
        public async Task<(ExecutionState executionState, string message)> ValidateUpdateAsync(AdditionalInformation toUpdate)
        {
            if (toUpdate.Id <= 0)
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toUpdate.Id), ExceptionMessage.ValueCannotBeNull));
            }

            (AdditionalInformation city, ExecutionState executionState, string message) errorIfExistCode = await UnitOfWork.TableMappingDatas.GetAsync(toUpdate.Id);

            if (errorIfExistCode.executionState == ExecutionState.Failure)
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toUpdate.Id), ExceptionMessage.EntityNotExists));
            }

            if (string.IsNullOrEmpty(toUpdate.Value))
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(toUpdate.Value), ExceptionMessage.ValueCannotBeNull));
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

            (AdditionalInformation TableMappingData, ExecutionState executionState, string message) errorIfExistCode = await UnitOfWork.TableMappingDatas.GetAsync(key);

            if (errorIfExistCode.executionState == ExecutionState.Failure)
            {
                return (executionState: ExecutionState.Failure, message: string.Format("Field {0} {1}", nameof(key), ExceptionMessage.EntityNotExists));
            }
            return (executionState: ExecutionState.Success, message: "Validation successfully completed");
        }

    }
}