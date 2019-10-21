using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMTestProject.Common.Dto.TableConfiguration;
using IMTestProject.Common.Entity;
using IMTestProject.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMTestProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableConfigurationController : CrudController<ITableConfigurationService, TableConfiguration, TableConfigurationDto, AddTableConfigurationDto, EditTableConfigurationDto, int>
    {
        public TableConfigurationController(ITableConfigurationService service) : base(service) { }
    }
}
