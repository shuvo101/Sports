using IMTestProject.Common.Dto.TableConfiguration;
using IMTestProject.Common.Entity;
using IMTestProject.Common.Enum;
using System.Threading.Tasks;

namespace IMTestProject.Service.Interface
{
    public interface ITableConfigurationService : IService<TableConfiguration, TableConfigurationDto, AddTableConfigurationDto, EditTableConfigurationDto, int>
    {
        //Task<TableConfigurationDto> GetTableConfigurationByCodeAsync (string code);
        Task<(TableConfigurationDto entity, ExecutionState executionState, string message)> GetTableConfigurationByCodeAsync(string code);

        
    }
}