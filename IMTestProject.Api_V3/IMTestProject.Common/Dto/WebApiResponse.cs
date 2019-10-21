using IMTestProject.Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMTestProject.Common.Dto
{
    public class WebApiResponse<T>
    {
        public WebApiResponse()
        {
            
        }
        public WebApiResponse((T entity, ExecutionState executionState, string message) retult)
        {
            this.Data = retult.entity;
            this.ExecutionState = retult.executionState;
            this.Message = retult.message;

        }
        public WebApiResponse((ExecutionState executionState, string message) retult)
        {
            this.ExecutionState = retult.executionState;
            this.Message = retult.message;
        }
        public ExecutionState ExecutionState { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
