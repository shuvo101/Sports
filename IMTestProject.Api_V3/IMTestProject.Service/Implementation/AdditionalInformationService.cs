using System;
using System.Linq;
using System.Threading.Tasks;
using IMTestProject.Common;
using IMTestProject.Business.Interface;
using IMTestProject.Common.Dto.TableMappingData;
using IMTestProject.Common.Enum;
using IMTestProject.Service.Interface;
using IMTestProject.Common.Entity;

namespace IMTestProject.Service.Implementation
{
    public class AdditionalInformationService : BaseService<AdditionalInformation, AdditionalInformationDto, AddAdditionalInformationDto, EditAdditionalInformationDto,
        IAdditionalInformationBusiness, int>, IAdditionalInformationService
    {
        public AdditionalInformationService(IAdditionalInformationBusiness business) : base(business)
        { }

        public async Task<(AdditionalInformationDto entity, ExecutionState executionState, string message)> GetTableMappingDataByCodeAsync(string code)
        {
            try
            {
                (AdditionalInformation entity, ExecutionState executionState, string message) data = await Business.GetTableMappingDataByCodeAsync(code);
                if (data.executionState == ExecutionState.Failure)
                {
                    return (entity: null, executionState: data.executionState, message: data.message);
                }
                return (entity: CastEntityToDto(data.entity), executionState: data.executionState, message: data.message);

            }
            catch (Exception ex)
            {
                return (entity: null, executionState: ExecutionState.Failure, message: ex.Message);
            }
        }

        public override AdditionalInformation CastDtoToEntity(AddAdditionalInformationDto source)
        {
            if (source is null)
            {
                return null;
            }

            return new AdditionalInformation
            {
                Value = source.Value,
                ContinentId = source.ContinentId,
                TableConfigurationId = source.TableConfigurationId,
            };
        }

        public override async Task<AdditionalInformation> CastDtoToEntityAsync(EditAdditionalInformationDto source)
        {
            if (source == null)
            {
                return null;
            }

            (AdditionalInformation entity, ExecutionState executionState, string message) newTracker = await Business.GetAsync(source.Id);
            if (newTracker.executionState == ExecutionState.Failure)
            {
                return null;
            }

            newTracker.entity.Value = source.Value;
            newTracker.entity.ContinentId = source.ContinentId;
            newTracker.entity.TableConfigurationId = source.TableConfigurationId;

            return newTracker.entity;
        }

        public override AdditionalInformationDto CastEntityToDto(AdditionalInformation source)
        {
            if (source == null)
            {
                return null;
            }

            return new AdditionalInformationDto
            {
                Id = source.Id,
                Value = source.Value,
                ContinentId = source.ContinentId,
                TableConfigurationId = source.TableConfigurationId,
            };
        }

        public override IQueryable<AdditionalInformationDto> CastIQueryableEntityToIQueryableDto(IQueryable<AdditionalInformation> source)
        {
            if (source == null)
            {
                return null;
            }

            return source.Select(s => new AdditionalInformationDto
            {
                Id = s.Id,
                Value = s.Value,
                ContinentId = s.ContinentId,
                TableConfigurationId = s.TableConfigurationId,
            });
        }
    }
}