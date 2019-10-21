using System;
using System.Threading.Tasks;
using IMTestProject.Common;
using IMTestProject.Common.Entity;
using IMTestProject.Common.Enum;

namespace IMTestProject.Business.Interface
{
    public interface ITableConfigurationBusiness : IBusiness<TableConfiguration, int>
    {
        Task<(TableConfiguration entity, ExecutionState executionState, string message)> GetTableConfigurationByCodeAsync(string code);
    }
}