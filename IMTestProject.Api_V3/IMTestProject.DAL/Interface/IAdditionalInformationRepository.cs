using System.Threading.Tasks;
using IMTestProject.Common;
using IMTestProject.Common.Entity;
using IMTestProject.Common.Enum;

namespace IMTestProject.DAL.Interface
{
    public interface IAdditionalInformationRepository : IRepository<AdditionalInformation, int>
    {
        Task<(AdditionalInformation entity, ExecutionState executionState, string message)> GetAdditionalInformationByCode(string code);
    }
}   