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
    public class TableConfigurationRepository : BaseRepository<TableConfiguration,int>, ITableConfigurationRepository
    {
        private IMTestProjectReadOnlyContext ReadOnlyContext;
        private IMTestProjectWriteOnlyContext WriteOnlyContext;
        public TableConfigurationRepository(IMTestProjectReadOnlyContext readOnlyContext, IMTestProjectWriteOnlyContext writeOnlyContext) : base (readOnlyContext, writeOnlyContext)
        {
            ReadOnlyContext = readOnlyContext;
            WriteOnlyContext = writeOnlyContext;
        }

        public async Task<(TableConfiguration entity, ExecutionState executionState, string message)> GetTableConfigurationByCode (string code)
        {
            try
            {
                TableConfiguration TableConfiguration = await ReadOnlyContext.TableConfigurations.FirstOrDefaultAsync(x => x.Name.ToUpper() == code.ToUpper());
                return (entity: TableConfiguration, executionState: (TableConfiguration != null)? ExecutionState.Success: ExecutionState.Failure, message: (TableConfiguration != null) ? "TableConfiguration found.": "TableConfiguration not found.");
                
            }
            catch (Exception ex)
            {
                return (entity: null, executionState: ExecutionState.Failure, message: ex.Message);
            }
            
        }
        public override void EntityCreated(TableConfiguration entity)
        {
            entity.EntityCreated();
        }
        public override void EntityUpdated(TableConfiguration entity)
        {
            entity.EntityUpdated();
        }
        public override void EntityRemoved(TableConfiguration entity)
        {
            entity.EntityRemoved();
        }

        public override IQueryable<TableConfiguration> FilteredWithDelete(IQueryable<TableConfiguration> query)
        {
            return query.Where(x => !x.IsDeleted);
        }
    }
}