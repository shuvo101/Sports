using System;
using System.Linq;
using System.Threading.Tasks;
using IMTestProject.Business.Interface;
using IMTestProject.Common.Enum;
using IMTestProject.Service.Interface;
using IMTestProject.Common.Entity;
using IMTestProject.Common.Dto.TableConfiguration;

namespace IMTestProject.Service.Implementation
{
    public class TableConfigurationService : BaseService<TableConfiguration, TableConfigurationDto, AddTableConfigurationDto, EditTableConfigurationDto,
        ITableConfigurationBusiness, int>, ITableConfigurationService
    {
        public TableConfigurationService(ITableConfigurationBusiness business) : base(business)
        { }

        public async Task<(TableConfigurationDto entity, ExecutionState executionState, string message)> GetTableConfigurationByCodeAsync(string code)
        {
            try
            {
                (TableConfiguration entity, ExecutionState executionState, string message) data = await Business.GetTableConfigurationByCodeAsync(code);
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

        public override TableConfiguration CastDtoToEntity(AddTableConfigurationDto source)
        {
            if (source is null)
            {
                return null;
            }

            return new TableConfiguration
            {
                Name = source.Name,
                DisplayName = source.DisplayName,
                ControlType = source.ControlType,
                DataType = source.DataType,
                ReferanceTable = source.ReferanceTable,
                TextField = source.TextField,
                ValueField = source.ValueField,
            };
        }

        public override async Task<TableConfiguration> CastDtoToEntityAsync(EditTableConfigurationDto source)
        {
            if (source == null)
            {
                return null;
            }

            (TableConfiguration entity, ExecutionState executionState, string message) newTracker = await Business.GetAsync(source.Id);
            if (newTracker.executionState == ExecutionState.Failure)
            {
                return null;
            }
            newTracker.entity.Name = source.Name;
            newTracker.entity.DisplayName = source.DisplayName;
            newTracker.entity.ControlType = source.ControlType;
            newTracker.entity.DataType = source.DataType;
            newTracker.entity.ReferanceTable = source.ReferanceTable;
            newTracker.entity.  TextField = source.TextField;
            newTracker.entity.ValueField = source.ValueField;

            return newTracker.entity;
        }

        public override TableConfigurationDto CastEntityToDto(TableConfiguration source)
        {
            if (source == null)
            {
                return null;
            }

            return new TableConfigurationDto
            {
                Id = source.Id,
                Name = source.Name,
                DisplayName = source.DisplayName,
                ControlType = source.ControlType,
                DataType = source.DataType,
                TextField = source.TextField,
                ValueField = source.ValueField,
                ReferanceTable = source.ReferanceTable,
            };
        }

        public override IQueryable<TableConfigurationDto> CastIQueryableEntityToIQueryableDto(IQueryable<TableConfiguration> source)
        {
            if (source == null)
            {
                return null;
            }

            return source.Select(s => new TableConfigurationDto
            {
                Id = s.Id,
                Name = s.Name,
                DisplayName = s.DisplayName,
                ControlType = s.ControlType,
                DataType = s.DataType,
                TextField = s.TextField,
                ValueField = s.ValueField,
                ReferanceTable = s.ReferanceTable,
            });
        }
    }
}