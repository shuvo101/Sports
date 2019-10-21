using System;
using System.Threading.Tasks;
using IMTestProject.Common;
using IMTestProject.Common.Entity;
using IMTestProject.Common.Enum;

namespace IMTestProject.Business.Interface
{
    public interface IAdditionalInformationBusiness : IBusiness<AdditionalInformation, int>
    {
        Task<(AdditionalInformation entity, ExecutionState executionState, string message)> GetTableMappingDataByCodeAsync(string code);
    }
}