using IMTestProject.Common;
using IMTestProject.Common.Dto.Continent;
using IMTestProject.Common.Enum;
using System.Threading.Tasks;

namespace IMTestProject.Service.Interface
{
    public interface IContinentService : IService<Continent, ContinentDto, AddContinentDto, EditContinentDto, int>
    {
        //Task<ContinentDto> GetContinentByCodeAsync (string code);
        Task<(ContinentDto entity, ExecutionState executionState, string message)> GetContinentByCodeAsync(string code);

        
    }
}