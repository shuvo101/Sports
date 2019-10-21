using IMTestProject.Common.Dto.TableMappingData;
using IMTestProject.Common.Entity;
using IMTestProject.Common.Enum;
using System.Threading.Tasks;

namespace IMTestProject.Service.Interface
{
    public interface IAdditionalInformationService : IService<AdditionalInformation, AdditionalInformationDto, AddAdditionalInformationDto, EditAdditionalInformationDto, int>
    {
        Task<(AdditionalInformationDto entity, ExecutionState executionState, string message)> GetTableMappingDataByCodeAsync(string code);
    }
}