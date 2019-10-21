using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMTestProject.Portal.Models
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

    public class APIResponse
    {
        public dynamic response { get; set; }

    }

    public class Confirmation
    {
        public string output { get; set; }
        public string msg { get; set; }
        public object returnvalue { get; set; }
    }

    public class Auth
    {
        public static string Token = "";
        public string BaseUrl = "http://localhost:12507/api/";//Properties.Settings.Default.BaseURL;

        public string GetBaseURL() //method return Base Url
        {
            return BaseUrl;
        }

        public void SetToken(string t) //method return Access Token of API
        {
            Token = t;
        }

        public string GetToken()
        {
            string T = Token;
            return T;
        }
        public void DestroyToken()
        {
            Token = "";
        }
    }

    public class ResponseDatum
    {
        public int executionState { get; set; }
        public string message { get; set; }
        public dynamic data { get; set; }
    }
}
