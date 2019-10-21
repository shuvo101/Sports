using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMTestProject.Common;
using IMTestProject.Common.Dto;
using IMTestProject.Common.Enum;
using IMTestProject.Common.QuerySerialize;
using IMTestProject.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMTestProject.Api.Controllers
{
    public abstract class CrudController<TService, TEntity, TResult, TAdd, TEdit, TPk> : ControllerBase
        where TEntity : class, IEntity<TPk>, new()
        where TAdd : class, new()
        where TEdit : class, new() where TResult : class, new() where TService : IService<TEntity, TResult, TAdd, TEdit, TPk>
        where TPk : IComparable
    {
        protected readonly TService Service;

        protected CrudController(TService service)
        {
            Service = service;
        }

        [HttpGet("GetList")]
        public virtual async Task<ActionResult<WebApiResponse<List<TResult>>>> GetList()
        {
            QueryOptions<TEntity> queryOptions = QueryOptions<TEntity>.FromQueryOptionsNodes(null);

            (IQueryable<TResult> entity, ExecutionState executionState, string message) result = await Service.List(queryOptions);
            (List<TResult> entity, ExecutionState executionState, string message) result1 = (result.entity.ToList(), result.executionState, result.message);
            WebApiResponse<List<TResult>> apiResponse = new WebApiResponse<List<TResult>>(result1);
            
            if (result.executionState == ExecutionState.Failure)
            {
                return NotFound(apiResponse);
            }
            else
            {
                return Ok(apiResponse);
            }
        }

        [HttpPost]
        public virtual async Task<ActionResult<WebApiResponse<TResult>>> Create([FromBody] TAdd add)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (add is null)
            {
                return BadRequest("Model cannot be null");
            }

            (TResult entity, ExecutionState executionState, string message) result = await Service.CreateAsync(add);
            WebApiResponse<TResult> apiResponse = new WebApiResponse<TResult>(result);
            //apiResponse.PrepareDataToWebAPIResponse(result);
            if (result.executionState == ExecutionState.Failure)
            {
                return NotFound(apiResponse);
            }
            else
            {
                return Ok(apiResponse);
            }
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<WebApiResponse<TResult>>> Get(TPk id)
        {
            (TResult entity, ExecutionState executionState, string message) result = await Service.GetAsync(id);
            WebApiResponse<TResult> apiResponse = new WebApiResponse<TResult>(result);
            if (result.executionState == ExecutionState.Failure)
            {
                return NotFound(apiResponse);
            }
            else
            {
                return Ok(apiResponse);
            }

        }

        [HttpPost("GetWithfilterOptions")]
        public virtual async Task<ActionResult<WebApiResponse<TResult>>> GetWithfilterOptions([FromBody] FilterOptions<TEntity> filterOptions)
        {
            (TResult entity, ExecutionState executionState, string message) result = await Service.GetAsync(filterOptions);
            WebApiResponse<TResult> apiResponse = new WebApiResponse<TResult>(result);
            //apiResponse.PrepareDataToWebAPIResponse(result);
            if (result.executionState == ExecutionState.Failure)
            {
                return NotFound(apiResponse);
            }
            else
            {
                return Ok(apiResponse);
            }

        }


        //[HttpPost("ListWithExpression")]
        //public virtual async Task<ActionResult<List<TResult>>> ListWithExpression([FromBody] ExpressionNode query)
        //{
        //    List<TResult> results = new List<TResult>();
        //    var expression = query.ToBooleanExpression<TEntity>();
        //    var expressionCompile = expression.Compile();



        //    return Ok(results);


        //}

        [HttpPost("ListWithQueryOption")]
        public virtual async Task<ActionResult<WebApiResponse<List<TResult>>>> List([FromBody] QueryOptionsNodes queryOptionsNodes)
        {
            QueryOptions<TEntity> queryOptions = QueryOptions<TEntity>.FromQueryOptionsNodes(queryOptionsNodes);

            (IQueryable<TResult> entity, ExecutionState executionState, string message) result = await Service.List(queryOptions);
            (List<TResult> entity, ExecutionState executionState, string message) result1 = (result.entity.ToList(), result.executionState, result.message);
            WebApiResponse<List<TResult>> apiResponse = new WebApiResponse<List<TResult>>(result1);
            //apiResponse.PrepareDataToWebAPIResponse(result);
            if (result.executionState == ExecutionState.Failure)
            {
                return NotFound(apiResponse);
            }
            else
            {
                return Ok(apiResponse);
            }
        }


        [HttpGet("DoesExist/{id}")]
        public virtual async Task<ActionResult<WebApiResponse<bool>>> DoesExist([FromQuery] TPk id)
        {
            (ExecutionState executionState, string message) result = await Service.DoesExistAsync(id);
            WebApiResponse<bool> apiResponse = new WebApiResponse<bool>(result);
            //apiResponse.PrepareDataToWebAPIResponse(result);
            if (result.executionState == ExecutionState.Failure)
            {
                apiResponse.Data = false;
                return NotFound(apiResponse);
            }
            else
            {
                apiResponse.Data = true;
                return Ok(apiResponse);
            }
        }

        [HttpPost("DoesExist")]
        public virtual async Task<ActionResult<WebApiResponse<bool>>> DoesExist([FromBody] FilterOptions<TEntity> filterOptions)
        {
            (ExecutionState executionState, string message) result = await Service.DoesExistAsync(filterOptions);
            WebApiResponse<bool> apiResponse = new WebApiResponse<bool>(result);
            //apiResponse.PrepareDataToWebAPIResponse(result);
            if (result.executionState == ExecutionState.Failure)
            {
                apiResponse.Data = false;
                return NotFound(apiResponse);
            }
            else
            {
                apiResponse.Data = true;
                return Ok(apiResponse);
            }
        }

        [HttpPut]
        public virtual async Task<ActionResult<WebApiResponse<TResult>>> Update([FromBody] TEdit edit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (edit is null)
            {
                return BadRequest("Model cannot be null");
            }

            (TResult entity, ExecutionState executionState, string message) result = await Service.UpdateAsync(edit);
            WebApiResponse<TResult> apiResponse = new WebApiResponse<TResult>(result);
            //apiResponse.PrepareDataToWebAPIResponse(result);
            if (result.executionState == ExecutionState.Failure)
            {
                return NotFound(apiResponse);
            }
            else
            {
                return Ok(apiResponse);
            }
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult<WebApiResponse<bool>>> Remove(TPk id)
        {

            (TResult entity, ExecutionState executionState, string message) result = await Service.RemoveAsync(id);
            (ExecutionState executionState, string message) statusResult = new ValueTuple<ExecutionState, string>(result.executionState, result.message);
            WebApiResponse<bool> apiResponse = new WebApiResponse<bool>(statusResult);
            //apiResponse.PrepareDataToWebAPIResponse(statusResult);
            if (result.executionState == ExecutionState.Failure)
            {
                apiResponse.Data = false;
                return NotFound(apiResponse);
            }
            else
            {
                apiResponse.Data = true;
                return Ok(apiResponse);
            }
        }

        //[HttpGet("Count/{countOptions}")]
        [HttpPost("Count")]
        public virtual async Task<ActionResult<WebApiResponse<long>>> Count([FromBody] CountOptions<TEntity> countOptions)
        {

            (long entityCount, ExecutionState executionState, string message) result = await Service.CountAsync(countOptions);
            WebApiResponse<long> apiResponse = new WebApiResponse<long>(result);
            if (result.executionState == ExecutionState.Failure)
            {
                return NotFound(apiResponse);
            }
            else
            {
                return Ok(apiResponse);
            }

        }

        [HttpPut("MarkAsActive/{id}")]
        public virtual ActionResult<WebApiResponse<TResult>> MarkAsActive([FromQuery] TPk id)
        {
            (TResult entity, ExecutionState executionState, string message) result = Service.MarkAsActiveAsync(id).Result;
            WebApiResponse<TResult> apiResponse = new WebApiResponse<TResult>(result);
            if (result.executionState == ExecutionState.Failure)
            {
                return NotFound(apiResponse);
            }
            else
            {
                return Ok(apiResponse);
            }
        }

        [HttpPut("MarkAsInactive/{id}")]
        public virtual ActionResult<WebApiResponse<TResult>> MarkAsInactive([FromQuery] TPk id)
        {
            (TResult entity, ExecutionState executionState, string message) result = Service.MarkAsInactiveAsync(id).Result;
            WebApiResponse<TResult> apiResponse = new WebApiResponse<TResult>(result);
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
