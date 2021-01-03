using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PlaylistRecommender.Domain.Core
{
    public abstract class Provider<T> where T : Entity
    {
        private Dictionary<String, String> _urlParams = new Dictionary<string, string>();

        private Dictionary<String, String> _headers = new Dictionary<string, string>();
        private const string DEFAULT_URL_KEY = "BASE_URL";
        
        public ProviderConfig ProviderConfig { get; private set; }
        protected Provider(string providerKey)
        {
            ProviderConfig = ProviderSettings.Instance.GetProviderConfig(providerKey);
        }
        
        protected async Task<T> SendRequestAndConvertResponse(HttpRequestMessage request)
        {
            return this.ConvertObject(await SendRequest(request));
        } 

        protected async Task<string> SendRequest(HttpRequestMessage request)
        {
            using(var httpClient = new HttpClient())
            {
                this.SetHeadersInRequest(ref request);

                var response = await httpClient.SendAsync(request);

                string result = await response.Content.ReadAsStringAsync();

                this.ClearHeaders();
                this.ClearUrlParams();

                return result;
            }
        } 

    
        protected void AddUrlParams(string key, string value)
        {
            if(! _urlParams.ContainsKey(key)) this._urlParams.Add(key, value);
        }
        
        protected void AddHeaders(string key, string value)
        {
            if(!_headers.ContainsKey(key)) _headers.Add(key, value);
        }

        protected string GetUrl(string keyUrl, string keyRoute)
        {
            return QueryHelpers.AddQueryString($"{ProviderConfig.GetUrl(keyUrl)}/{ProviderConfig.GetRoute(keyRoute)}", this._urlParams);
        }

        protected string GetUrl(string keyRoute)
        {
            return this.GetUrl(DEFAULT_URL_KEY, keyRoute);
        }

        private void SetHeadersInRequest(ref HttpRequestMessage request) 
        {
            foreach(var header in this._headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        private void ClearHeaders()
        {
            this._headers.Clear();
        }

        private void ClearUrlParams()
        {
            this._urlParams.Clear();
        }
        protected abstract T ConvertObject(string json);
    }
}