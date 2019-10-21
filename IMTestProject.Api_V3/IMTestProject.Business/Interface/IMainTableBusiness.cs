using System;
using System.Threading.Tasks;
using IMTestProject.Common;
using IMTestProject.Common.Entity;
using IMTestProject.Common.Enum;

namespace IMTestProject.Business.Interface
{
    public interface IMainTableBusiness : IBusiness<MainTable, int>
    {
        Task<(MainTable entity, ExecutionState executionState, string message)> GetMainTableByCodeAsync(string code);
    }
}