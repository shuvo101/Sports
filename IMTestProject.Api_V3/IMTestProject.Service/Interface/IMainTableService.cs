using IMTestProject.Common.Dto.MainTable;
using IMTestProject.Common.Entity;
using IMTestProject.Common.Enum;
using System.Threading.Tasks;

namespace IMTestProject.Service.Interface
{
    public interface IMainTableService : IService<MainTable, MainTableDto, AddMainTableDto, EditMainTableDto, int>
    {
        //Task<MainTableDto> GetMainTableByCodeAsync (string code);
        Task<(MainTableDto entity, ExecutionState executionState, string message)> GetMainTableByCodeAsync(string code);

        
    }
}