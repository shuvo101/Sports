using System;
using System.Threading.Tasks;
using IMTestProject.Common;
using IMTestProject.Common.Enum;

namespace IMTestProject.Business.Interface
{
    public interface IContinentBusiness : IBusiness<Continent, int>
    {
        Task<(Continent entity, ExecutionState executionState, string message)> GetContinentByCodeAsync (string code);
    }
}