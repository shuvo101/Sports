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
    public class AdditionalInformationRepository : BaseRepository<AdditionalInformation,int>, IAdditionalInformationRepository
    {
        private IMTestProjectReadOnlyContext ReadOnlyContext;
        private IMTestProjectWriteOnlyContext WriteOnlyContext;
        public AdditionalInformationRepository(IMTestProjectReadOnlyContext readOnlyContext, IMTestProjectWriteOnlyContext writeOnlyContext) : base (readOnlyContext, writeOnlyContext)
        {
            ReadOnlyContext = readOnlyContext;
            WriteOnlyContext = writeOnlyContext;
        }

        public async Task<(AdditionalInformation entity, ExecutionState executionState, string message)> GetAdditionalInformationByCode (string code)
        {
            try
            {
                AdditionalInformation additionalInformation = await ReadOnlyContext.AdditionalInformationDatas.FirstOrDefaultAsync(x => x.Code.ToUpper() == code.ToUpper());
                return (entity: additionalInformation, executionState: (additionalInformation != null)? ExecutionState.Success: ExecutionState.Failure, message: (additionalInformation != null) ? "AdditionalInformation found.": "AdditionalInformation not found.");
                
            }
            catch (Exception ex)
            {
                return (entity: null, executionState: ExecutionState.Failure, message: ex.Message);
            }
            
        }
        public override void EntityCreated(AdditionalInformation entity)
        {
            entity.EntityCreated();
        }
        public override void EntityUpdated(AdditionalInformation entity)
        {
            entity.EntityUpdated();
        }
        public override void EntityRemoved(AdditionalInformation entity)
        {
            entity.EntityRemoved();
        }

        public override IQueryable<AdditionalInformation> FilteredWithDelete(IQueryable<AdditionalInformation> query)
        {
            return query.Where(x => !x.IsDeleted);
        }
    }
}