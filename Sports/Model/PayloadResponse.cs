using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.API.Model
{
    public class PayloadResponse
    {
        public PayloadResponse()
        {
            this.RequestTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }
        public string RequestTime { get; set; }
        public string ResponseTime { get; set; }
        public string RequestURL { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public dynamic Payload { get; set; }
        public string PayloadType { get; set; }
    }
}
