using IMTestProject.Common;
using IMTestProject.Common.Enum;
using IMTestProject.Portal.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IMTestProject.Portal.Repository
{
    public class WebApiClientRepository<T> : IDisposable, _IWebApiClientRepository<T> where T : class
    {
        Auth auth = new Auth();
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public ResponseDatum GetList(dynamic model, string methodUrl)
        {
            string BaseUrl = auth.GetBaseURL();

            string contantData = "";
            if (model != null)
            {
                contantData = JsonConvert.SerializeObject(model);
            }

            HttpContent content = new StringContent(contantData, Encoding.UTF8, "application/json");

            var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };

            var uri = new Uri(BaseUrl + methodUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            HttpResponseMessage result = new HttpResponseMessage();
            try
            {
                result = client.GetAsync(uri.ToString()).Result;
            }
            catch (Exception ex)
            {
                result = null;
            }
            dynamic jsonString = "";
            try
            {
                jsonString = result.Content.ReadAsStringAsync();
                jsonString.Wait();
            }
            catch (Exception ex)
            {

                return null;
            }

            ResponseDatum ob;
            //(Continent entity, ExecutionState executionState, string message);
            try
            {
                ob = JsonConvert.DeserializeObject<ResponseDatum>(jsonString.Result);
                return ob;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ResponseDatum GetById(dynamic Id, string methodUrl)
        {
            string BaseUrl = auth.GetBaseURL();
            var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
            var uri = new Uri(BaseUrl + methodUrl + Id);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //string Token = auth.GetToken();
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
            HttpResponseMessage result = client.GetAsync(uri.ToString()).Result;

            var jsonString = result.Content.ReadAsStringAsync();
            jsonString.Wait();
            ResponseDatum responseObj;
            try
            {
                responseObj = JsonConvert.DeserializeObject<ResponseDatum>(jsonString.Result);
                return responseObj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ResponseDatum Post(dynamic model, string methodUrl)
        {
            string BaseUrl = auth.GetBaseURL();

            string contantData = "";
            if (model != null)
            {
                contantData = JsonConvert.SerializeObject(model);
            }

            HttpContent content = new StringContent(contantData, Encoding.UTF8, "application/json");

            var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };

            var uri = new Uri(BaseUrl + methodUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage result = new HttpResponseMessage();
            try
            {
                result = client.PostAsync(uri, content).Result;
            }
            catch (Exception ex)
            {
                result = null;
            }
            dynamic jsonString = "";
            try
            {
                jsonString = result.Content.ReadAsStringAsync();
                jsonString.Wait();
            }
            catch (Exception ex)
            {

                return null;
            }


            ResponseDatum ob;

            try
            {
                ob = JsonConvert.DeserializeObject<ResponseDatum>(jsonString.Result);
                return ob;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ResponseDatum Put(dynamic model, string methodUrl)
        {
            string BaseUrl = auth.GetBaseURL();

            string contantData = "";
            if (model != null)
            {
                contantData = JsonConvert.SerializeObject(model);
            }

            HttpContent content = new StringContent(contantData, Encoding.UTF8, "application/json");

            var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };

            var uri = new Uri(BaseUrl + methodUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage result = new HttpResponseMessage();
            try
            {
                result = client.PutAsync(uri, content).Result;
            }
            catch (Exception ex)
            {
                result = null;
            }
            dynamic jsonString = "";
            try
            {
                jsonString = result.Content.ReadAsStringAsync();
                jsonString.Wait();
            }
            catch (Exception ex)
            {

                return null;
            }


            ResponseDatum ob;

            try
            {
                ob = JsonConvert.DeserializeObject<ResponseDatum>(jsonString.Result);
                return ob;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool Delete(dynamic id, string methodUrl)
        {
            string BaseUrl = auth.GetBaseURL();

            var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };

            var uri = new Uri(BaseUrl + methodUrl + "?id=" + id);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //string Token = auth.GetToken();
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
            HttpResponseMessage result = client.DeleteAsync(uri).Result;
            var jsonString = result.Content.ReadAsStringAsync();
            jsonString.Wait();


            return true;

        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            //Dispose(true);
            //GC.SuppressFinalize(this);
        }
    }
    interface _IWebApiClientRepository<T> where T : class
    {   
        ResponseDatum GetList(dynamic model, string methodUrl);
        ResponseDatum GetById(dynamic Id, string methodUrl);
        ResponseDatum Post(dynamic model, string methodUrl);
        ResponseDatum Put(dynamic model, string methodUrl);
        bool Delete(dynamic id, string methodUrl);
        void Dispose();
    }
}
