using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using IMTestProject.Common;
using IMTestProject.Common.Enum;
using IMTestProject.DAL.Interface;
using IMTestProject.Common.Entity;

namespace IMTestProject.DAL.Implementation
{
    public class MainTableRepository : BaseRepository<MainTable,int>, IMainTableRepository
    {
        private IMTestProjectReadOnlyContext ReadOnlyContext;
        private IMTestProjectWriteOnlyContext WriteOnlyContext;
        public MainTableRepository(IMTestProjectReadOnlyContext readOnlyContext, IMTestProjectWriteOnlyContext writeOnlyContext) : base (readOnlyContext, writeOnlyContext)
        {
            ReadOnlyContext = readOnlyContext;
            WriteOnlyContext = writeOnlyContext;
        }

        public async Task<(MainTable entity, ExecutionState executionState, string message)> GetMainTableByCode (string code)
        {
            try
            {
                MainTable MainTable = await ReadOnlyContext.MainTables.FirstOrDefaultAsync(x => x.TableCode.ToUpper() == code.ToUpper());
                return (entity: MainTable, executionState: (MainTable != null)? ExecutionState.Success: ExecutionState.Failure, message: (MainTable != null) ? "MainTable found.": "MainTable not found.");
                
            }
            catch (Exception ex)
            {
                return (entity: null, executionState: ExecutionState.Failure, message: ex.Message);
            }
            
        }
        public override void EntityCreated(MainTable entity)
        {
            entity.EntityCreated();
        }
        public override void EntityUpdated(MainTable entity)
        {
            entity.EntityUpdated();
        }
        public override void EntityRemoved(MainTable entity)
        {
            entity.EntityRemoved();
        }

        public override IQueryable<MainTable> FilteredWithDelete(IQueryable<MainTable> query)
        {
            return query.Where(x => !x.IsDeleted);
        }
    }
}