using Hanssens.Net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SportsWeb.Models;
using SportsWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SportsWeb.Repository
{
    public class WebApiClientRepository<T> : IDisposable, _IWebApiClientRepository<T> where T : class
    {
        Auth auth = new Auth();
        private readonly IHttpContextAccessor _httpContextAccessor;

        public bool Delete(long id, string MethodUrl)
        {
            string BaseUrl = auth.GetBaseURL();

            var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };

            var uri = new Uri(BaseUrl + MethodUrl + "?id=" + id);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string Token = auth.GetToken();

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
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

        public IEnumerable<T> GetAll(string MethodUrl)
        {
            string Token = string.Empty;
            using (var storage = new LocalStorage())
            {
                Token = storage.Get("jwtToken").ToString();
            }
            var jsonString = GlobalApiCallFunction(null, MethodUrl);
            List<T> responseObj = new List<T>();
            try
            {
                responseObj = JsonConvert.DeserializeObject<List<T>>(jsonString.Payload.ToString());
                return responseObj;
            }
            catch (Exception ex)
            {
                return responseObj;
            }
        }

        public T GetByID(dynamic id, string MethodUrl)
        {
            string BaseUrl = auth.GetBaseURL();
            var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
            var uri = new Uri(BaseUrl + MethodUrl + "?id=" + id);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string Token = auth.GetToken();


            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
            HttpResponseMessage result = client.GetAsync(uri.ToString()).Result;

            var jsonString = result.Content.ReadAsStringAsync();
            jsonString.Wait();
            T responseObj;
            try
            {
                responseObj = JsonConvert.DeserializeObject<T>(jsonString.Result);
                return responseObj;
            }
            catch (Exception ex)
            {
                return null;
            }

            //throw new NotImplementedException();
        }

        public T GetByDoubleID(T model, string MethodUrl)
        {
            string BaseUrl = auth.GetBaseURL();
            var content2 = JsonConvert.SerializeObject(model);
            HttpContent content = new StringContent(content2, Encoding.UTF8, "application/json");

            var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
            var uri = new Uri(BaseUrl + MethodUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string Token = auth.GetToken();


            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
            HttpResponseMessage result = client.PostAsync(uri, content).Result;

            var jsonString = result.Content.ReadAsStringAsync();
            jsonString.Wait();
            T responseObj;
            try
            {
                responseObj = JsonConvert.DeserializeObject<T>(jsonString.Result);
                return responseObj;
            }
            catch (Exception ex)
            {
                return null;
            }

            //throw new NotImplementedException();
        }
        
        public PayloadResponse Insert(string MethodUrl)
        {
            string BaseUrl = auth.GetBaseURL();

            string contantData = "";


            HttpContent content = new MultipartFormDataContent();

            var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };

            var uri = new Uri(BaseUrl + MethodUrl);
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string Token = auth.GetToken();

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
            HttpResponseMessage result = new HttpResponseMessage();
            try
            {
                result = client.PostAsync(uri, content).Result;
            }
            catch (Exception)
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
                PayloadResponse response = new PayloadResponse();
                response.Success = false;
                return response;
            }


            PayloadResponse ob;

            try
            {
                ob = JsonConvert.DeserializeObject<PayloadResponse>(jsonString.Result);
                return ob;
            }
            catch (Exception ex)
            {
                return null;
            }


        }

        public bool IsExist(long id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(T model, string MethodUrl)
        {
            string BaseUrl = auth.GetBaseURL();

            var content2 = JsonConvert.SerializeObject(model);
            HttpContent content = new StringContent(content2, Encoding.UTF8, "application/json");

            var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };

            var uri = new Uri(BaseUrl + MethodUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string Token = auth.GetToken();

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
            HttpResponseMessage result = client.PutAsync(uri, content).Result;

            var jsonString = result.Content.ReadAsStringAsync();
            jsonString.Wait();

        }

        public IEnumerable<T> GetAllById(long id, string MethodUrl)
        {
            string BaseUrl = auth.GetBaseURL();
            var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
            var uri = new Uri(BaseUrl + MethodUrl + "?id=" + id);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials;

            string Token = string.Empty;
            using (var storage = new LocalStorage())
            {
                Token = storage.Get("jwtToken").ToString();
            }

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);

            HttpResponseMessage result = client.GetAsync(uri.ToString()).Result;

            var jsonString = result.Content.ReadAsStringAsync();
            jsonString.Wait();
            List<T> responseObj = new List<T>();
            try
            {
                responseObj = JsonConvert.DeserializeObject<List<T>>(jsonString.Result);
                return responseObj;
            }
            catch (Exception ex)
            {
                return responseObj;
            }
        }

        public T GetByModel(T model, string MethodUrl)
        {
            string BaseUrl = auth.GetBaseURL();

            var content2 = JsonConvert.SerializeObject(model);
            HttpContent content = new StringContent(content2, Encoding.UTF8, "application/json");

            var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
            var uri = new Uri(BaseUrl + MethodUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            string Token = string.Empty;
            using (var storage = new LocalStorage())
            {
                Token = storage.Get("jwtToken").ToString();
            }

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
            HttpResponseMessage result = client.PostAsync(uri, content).Result;
            var jsonString = result.Content.ReadAsStringAsync();
            jsonString.Wait();

            T responseObj;
            try
            {
                responseObj = JsonConvert.DeserializeObject<T>(jsonString.Result);
                return responseObj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Confirmation InsertList(List<T> list, string MethodUrl)
        {
            string BaseUrl = auth.GetBaseURL();

            var content2 = JsonConvert.SerializeObject(list);
            HttpContent content = new StringContent(content2, Encoding.UTF8, "application/json");

            var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };

            var uri = new Uri(BaseUrl + MethodUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string Token = string.Empty;
            using (var storage = new LocalStorage())
            {
                Token = storage.Get("jwtToken").ToString();
            }

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
            HttpResponseMessage result = client.PostAsync(uri, content).Result;
            var jsonString = result.Content.ReadAsStringAsync();
            jsonString.Wait();

            Confirmation ob;

            try
            {
                ob = JsonConvert.DeserializeObject<Confirmation>(jsonString.Result);
                return ob;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public PayloadResponse GlobalGetApiCallFunction(dynamic model, string methodUrl)
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

            //string Token = auth.GetToken();
            string Token = string.Empty;
            using (var storage = new LocalStorage())
            {
                Token = storage.Get("jwtToken").ToString();
            }
            
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
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
                PayloadResponse response = new PayloadResponse();
                response.Success = false;
                return response;
            }


            PayloadResponse ob;

            try
            {
                ob = JsonConvert.DeserializeObject<PayloadResponse>(jsonString.Result);
                return ob;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public PayloadResponse GlobalApiCallFunction(dynamic model, string methodUrl)
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
            
            string Token = string.Empty;
            using (var storage = new LocalStorage())
            {
                Token = storage.Get("jwtToken").ToString();
            }

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
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
                PayloadResponse response = new PayloadResponse();
                response.Success = false;
                return response;
            }


            PayloadResponse ob;

            try
            {
                ob = JsonConvert.DeserializeObject<PayloadResponse>(jsonString.Result);
                return ob;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public T GlobalApiCallPost(dynamic model, string methodUrl)
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

            string Token = string.Empty;
            using (var storage = new LocalStorage())
            {
                if (storage.Exists("jwtToken"))
                {
                    Token = storage.Get("jwtToken").ToString();
                }
            }

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
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


            T ob;

            try
            {
                ob = JsonConvert.DeserializeObject<T>(jsonString.Result);
                return ob;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public T GlobalApiCallGet(dynamic model, string methodUrl)
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

            string Token = string.Empty;
            using (var storage = new LocalStorage())
            {
                Token = storage.Get("jwtToken").ToString();
            }

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
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


            T ob;

            try
            {
                ob = JsonConvert.DeserializeObject<T>(jsonString.Result);
                return ob;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //public PayloadResponse GlobalApi(dynamic model, string methodUrl)
        //{
        //    string BaseUrl = auth.GetBaseURL();

        //    string contantData = "";
        //    if (model != null)
        //    {
        //        contantData = JsonConvert.SerializeObject(model);
        //    }

        //    HttpContent content = new StringContent(contantData, Encoding.UTF8, "application/json");

        //    var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };

        //    var uri = new Uri(BaseUrl + methodUrl);
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //    string Token = string.Empty;
        //    using (var storage = new LocalStorage())
        //    {
        //        Token = storage.Get("jwtToken").ToString();
        //    }

        //    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
        //    HttpResponseMessage result = new HttpResponseMessage();
        //    try
        //    {
        //        result = client.PostAsync(uri, content).Result;
        //    }
        //    catch (Exception ex)
        //    {
        //        result = null;
        //    }
        //    dynamic jsonString = "";
        //    try
        //    {
        //        jsonString = result.Content.ReadAsStringAsync();
        //        jsonString.Wait();
        //    }
        //    catch (Exception ex)
        //    {
        //        PayloadResponse response = new PayloadResponse();
        //        response.Success = false;
        //        return response;
        //    }


        //    PayloadResponse ob;

        //    try
        //    {
        //        ob = JsonConvert.DeserializeObject<PayloadResponse>(jsonString.Result);
        //        return ob;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
    }


    interface _IWebApiClientRepository<T> where T : class
    {
        IEnumerable<T> GetAll(string MethodUrl);

        T GetByID(dynamic id, string MethodUrl);
        T GetByDoubleID(T model, string MethodUrl);

        IEnumerable<T> GetAllById(long id, string MethodUrl);

        PayloadResponse GlobalGetApiCallFunction(dynamic model, string methodUrl);

        PayloadResponse GlobalApiCallFunction(dynamic model, string methodUrl);
        
        T GetByModel(T model, string MethodUrl);

        PayloadResponse Insert(string MethodUrl);

        Confirmation InsertList(List<T> list, string MethodUrl);

        void Update(T model, string MethodUrl);

        bool Delete(long id, string MethodUrl);

        bool IsExist(long id);

        void Save();

        void Dispose();



    }
}
