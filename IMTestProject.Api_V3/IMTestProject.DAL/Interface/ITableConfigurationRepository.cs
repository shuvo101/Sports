using System.Threading.Tasks;
using IMTestProject.Common;
using IMTestProject.Common.Entity;
using IMTestProject.Common.Enum;

namespace IMTestProject.DAL.Interface
{
    public interface ITableConfigurationRepository : IRepository<TableConfiguration, int>
    {
        Task<(TableConfiguration entity, ExecutionState executionState, string message)> GetTableConfigurationByCode(string code);
    }
}   