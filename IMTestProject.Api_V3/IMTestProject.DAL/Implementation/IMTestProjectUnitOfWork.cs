using System;
using Microsoft.EntityFrameworkCore.Storage;
using IMTestProject.Common;
using IMTestProject.DAL.Interface;
using IMTestProject.Common.Enum;

namespace IMTestProject.DAL.Implementation
{
    public partial class IMTestProjectUnitOfWork : IIMTestProjectUnitOfWork
    {
        private IMTestProjectReadOnlyContext ReadOnlyContext;
        private IMTestProjectWriteOnlyContext WriteOnlyContext;

        public IContinentRepository Continents { get; }
        public IMainTableRepository MainTables { get; }
        public ITableConfigurationRepository TableConfigurations { get; }
        public IAdditionalInformationRepository TableMappingDatas { get; }

        public IMTestProjectUnitOfWork(
            IMTestProjectReadOnlyContext readOnlyContext,
            IMTestProjectWriteOnlyContext writeOnlyContext,
            IContinentRepository continentRepository,
            IMainTableRepository mainTableRepository,
            ITableConfigurationRepository tableConfigurationRepository,
            IAdditionalInformationRepository tableMappingDataRepository
            )
        {
            ReadOnlyContext = readOnlyContext;
            WriteOnlyContext = writeOnlyContext;
            Continents = continentRepository;
            MainTables = mainTableRepository; ;
            TableConfigurations = tableConfigurationRepository; ;
            TableMappingDatas = tableMappingDataRepository; ;
        }

        public IDbContextTransaction Begin()
        {
            try
            {
                IDbContextTransaction transaction = WriteOnlyContext.Database.BeginTransaction();
                return transaction;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Complete(
            IDbContextTransaction transaction,
            CompletionState completionState)
        {
            try
            {
                if (transaction != null &&
                    transaction.TransactionId != null &&
                    transaction.GetDbTransaction() != null)
                {
                    if (completionState == CompletionState.Success)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
            }
            catch
            {
                transaction.Rollback();
            }
        }

        public void Dispose()
        {
            ReadOnlyContext?.Dispose();
            WriteOnlyContext?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}