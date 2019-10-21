using System;
using Microsoft.EntityFrameworkCore.Storage;
using IMTestProject.Common.Enum;

namespace IMTestProject.DAL.Interface
{
    public interface IIMTestProjectUnitOfWork : IDisposable
    {
        IDbContextTransaction Begin ();
        void Complete (IDbContextTransaction transaction, CompletionState completionState);
        
        IContinentRepository Continents { get; }
        IMainTableRepository MainTables { get; }
        ITableConfigurationRepository TableConfigurations { get; }
        IAdditionalInformationRepository TableMappingDatas { get; }
    }
}