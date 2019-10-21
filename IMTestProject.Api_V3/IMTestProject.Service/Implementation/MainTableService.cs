using System;
using System.Linq;
using System.Threading.Tasks;
using IMTestProject.Business.Interface;
using IMTestProject.Common.Enum;
using IMTestProject.Service.Interface;
using IMTestProject.Common.Entity;
using IMTestProject.Common.Dto.MainTable;

namespace IMTestProject.Service.Implementation
{
    public class MainTableService : BaseService<MainTable, MainTableDto, AddMainTableDto, EditMainTableDto,
        IMainTableBusiness, int>, IMainTableService
    {
        public MainTableService(IMainTableBusiness business) : base(business)
        { }

        public async Task<(MainTableDto entity, ExecutionState executionState, string message)> GetMainTableByCodeAsync(string code)
        {
            try
            {
                (MainTable entity, ExecutionState executionState, string message) data = await Business.GetMainTableByCodeAsync(code);
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

        public override MainTable CastDtoToEntity(AddMainTableDto source)
        {
            if (source is null)
            {
                return null;
            }

            return new MainTable
            {
                TableCode = source.TableCode,
                TableName = source.TableName
            };
        }

        public override async Task<MainTable> CastDtoToEntityAsync(EditMainTableDto source)
        {
            if (source == null)
            {
                return null;
            }

            (MainTable entity, ExecutionState executionState, string message) newTracker = await Business.GetAsync(source.Id);
            if (newTracker.executionState == ExecutionState.Failure)
            {
                return null;
            }
            newTracker.entity.TableCode = source.TableCode;
            newTracker.entity.TableName = source.TableName;

            return newTracker.entity;
        }

        public override MainTableDto CastEntityToDto(MainTable source)
        {
            if (source == null)
            {
                return null;
            }

            return new MainTableDto
            {
                Id = source.Id,
                TableName = source.TableName,
                TableCode = source.TableCode,
            };
        }

        public override IQueryable<MainTableDto> CastIQueryableEntityToIQueryableDto(IQueryable<MainTable> source)
        {
            if (source == null)
            {
                return null;
            }

            return source.Select(s => new MainTableDto
            {
                Id = s.Id,
                TableName = s.TableName,
                TableCode = s.TableCode,
            });
        }
    }
}