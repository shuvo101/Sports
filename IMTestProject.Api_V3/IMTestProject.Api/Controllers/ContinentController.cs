using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMTestProject.Common;
using IMTestProject.Common.Dto;
using IMTestProject.Common.Dto.Continent;
using IMTestProject.Common.Enum;
using IMTestProject.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMTestProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContinentController : CrudController<IContinentService, Continent, ContinentDto, AddContinentDto, EditContinentDto, int>
    {
        public ContinentController(IContinentService service) : base(service) { }

        [HttpGet("GetByCode")]
        public virtual async Task<ActionResult<WebApiResponse<ContinentDto>>> GetByCode([FromQuery] string code)
        {
            (ContinentDto entity, ExecutionState executionState, string message) result = await Service.GetContinentByCodeAsync(code);
            WebApiResponse<ContinentDto> apiResponse = new WebApiResponse<ContinentDto>(result);
            if (result.executionState == ExecutionState.Failure)
            {
                return NotFound(apiResponse);
            }
            else
            {
                return Ok(apiResponse);
            }
        }
    }
}