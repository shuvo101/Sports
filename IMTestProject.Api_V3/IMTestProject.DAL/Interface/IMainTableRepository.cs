using System.Threading.Tasks;
using IMTestProject.Common;
using IMTestProject.Common.Entity;
using IMTestProject.Common.Enum;

namespace IMTestProject.DAL.Interface
{
    public interface IMainTableRepository : IRepository<MainTable, int>
    {
        Task<(MainTable entity, ExecutionState executionState, string message)> GetMainTableByCode(string code);
    }
}   