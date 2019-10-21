using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using IMTestProject.Common;
using IMTestProject.Common.Enum;
using IMTestProject.DAL.Interface;
using IMTestProject.Common.QuerySerialize;

namespace IMTestProject.DAL.Implementation
{
    public class ContinentRepository :BaseRepository<Continent,int>, IContinentRepository
    {
        private IMTestProjectReadOnlyContext ReadOnlyContext;
        private IMTestProjectWriteOnlyContext WriteOnlyContext;
        public ContinentRepository (IMTestProjectReadOnlyContext readOnlyContext, IMTestProjectWriteOnlyContext writeOnlyContext) : base (readOnlyContext, writeOnlyContext)
        {
            ReadOnlyContext = readOnlyContext;
            WriteOnlyContext = writeOnlyContext;
        }

        public async Task<(Continent entity, ExecutionState executionState, string message)> GetContinentByCode (string code)
        {
            try
            {
                Continent continent = await ReadOnlyContext.Continents.FirstOrDefaultAsync(x => x.Code.ToUpper() == code.ToUpper());
                return (entity: continent, executionState: (continent != null)? ExecutionState.Success: ExecutionState.Failure, message: (continent != null) ? "Continent found.": "Continent not found.");
                
            }
            catch (Exception ex)
            {
                return (entity: null, executionState: ExecutionState.Failure, message: ex.Message);
            }
            
        }
        public override void EntityCreated(Continent entity)
        {
            entity.EntityCreated();
        }
        public override void EntityUpdated(Continent entity)
        {
            entity.EntityUpdated();
        }
        public override void EntityRemoved(Continent entity)
        {
            entity.EntityRemoved();
        }

        public override IQueryable<Continent> FilteredWithDelete(IQueryable<Continent> query)
        {
            return query.Where(x => !x.IsDeleted);
        }

        public override (IQueryable<Continent> entity, ExecutionState executionState, string message) List(
            QueryOptions<Continent> queryOptions = null,
            ListCondition listCondition = ListCondition.Normal)
        {
            try
            {
                IQueryable<Continent> continent = ReadOnlyContext.Set<Continent>().Include(x => x.TableMappingDatas).ThenInclude(x => x.TableConfiguration).AsQueryable();
                return (entity: continent, executionState: (continent != null) ? ExecutionState.Success : ExecutionState.Failure, message: (continent != null) ? "Country found." : "Country not found.");
            }
            catch (Exception ex)
            {
                return (entity: null, executionState: ExecutionState.Failure, message: ex.Message);
            }

        }

    }
}