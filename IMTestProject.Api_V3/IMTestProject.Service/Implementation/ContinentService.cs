using System;
using System.Linq;
using System.Threading.Tasks;
using IMTestProject.Common;
using IMTestProject.Business.Interface;
using IMTestProject.Common.Dto.Continent;
using IMTestProject.Common.Enum;
using IMTestProject.Service.Interface;
using IMTestProject.Common.Entity;
using System.Collections.Generic;

namespace IMTestProject.Service.Implementation
{
    public class ContinentService : BaseService<Continent, ContinentDto, AddContinentDto, EditContinentDto,
        IContinentBusiness, int>, IContinentService
    {
        public ContinentService(IContinentBusiness business) : base(business)
        { }

        public async Task<(ContinentDto entity, ExecutionState executionState, string message)> GetContinentByCodeAsync(string code)
        {
            try
            {
                (Continent entity, ExecutionState executionState, string message) data = await Business.GetContinentByCodeAsync(code);
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

        public override Continent CastDtoToEntity(AddContinentDto source)
        {
            if (source is null)
            {
                return null;
            }

            Continent continent = new Continent();
            continent.Code = source.Code;
            continent.Name = source.Name;
            if (source.TableConfigurationDtos.Count > 0)
            {
                List<AdditionalInformation> infoList = new List<AdditionalInformation>();
                foreach (var item in source.TableConfigurationDtos)
                {
                    AdditionalInformation info = new AdditionalInformation();
                    info.TableConfigurationId = item.additionalInformationDto.TableConfigurationId;
                    info.Value = item.additionalInformationDto.Value;
                    infoList.Add(info);
                }
                continent.TableMappingDatas = infoList;
            }

            return continent;
        }

        public override async Task<Continent> CastDtoToEntityAsync(EditContinentDto source)
        {
            if (source == null)
            {
                return null;
            }

            (Continent entity, ExecutionState executionState, string message) newTracker = await Business.GetAsync(source.Id);
            if (newTracker.executionState == ExecutionState.Failure)
            {
                return null;
            }
            newTracker.entity.Code = source.Code;
            newTracker.entity.Name = source.Name;

            return newTracker.entity;
        }

        public override ContinentDto CastEntityToDto(Continent source)
        {
            if (source == null)
            {
                return null;
            }


            return new ContinentDto
            {
                Id = source.Id,
                Name = source.Name,
                Code = source.Code,
            };
        }

        public override IQueryable<ContinentDto> CastIQueryableEntityToIQueryableDto(IQueryable<Continent> source)
        {
            if (source == null)
            {
                return null;
            }

            return source.Select(s => new ContinentDto
            {
                Id = s.Id,
                Name = s.Name,
                Code = s.Code,
            });
        }
    }
}