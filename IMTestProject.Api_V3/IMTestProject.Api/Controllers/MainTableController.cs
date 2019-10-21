using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMTestProject.Common.Dto.MainTable;
using IMTestProject.Common.Entity;
using IMTestProject.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMTestProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainTableController : CrudController<IMainTableService, MainTable, MainTableDto, AddMainTableDto, EditMainTableDto, int>
    {
        public MainTableController(IMainTableService service) : base(service) { }
    }
}
