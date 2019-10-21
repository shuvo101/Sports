using System.Threading.Tasks;
using IMTestProject.Common;
using IMTestProject.Common.Enum;

namespace IMTestProject.DAL.Interface
{
    public interface IContinentRepository : IRepository<Continent, int>
    {
        Task<(Continent entity, ExecutionState executionState, string message)> GetContinentByCode(string code);
    }
}   