using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PlaylistRecommender.Domain.Core
{
    public class ProviderConfig
    {
        [JsonProperty("provider_key")]
        public string ProviderKey { get; set; }

        [JsonProperty("api_key")]
        public string ApiKey { get; private set; }
        
        [JsonProperty("urls")]
        public Dictionary<string, string> _urls { get; set; }

        [JsonProperty("routes")]
        private Dictionary<string, string> _routes { get; set; }

        public string GetUrl(string keyUrl)
        {
            if(!this._urls.ContainsKey(keyUrl.ToLowerInvariant()))
                throw new Exception("A Url informada não foi encontrada");

            return this._urls[keyUrl.ToLowerInvariant()];
        }

        public string GetRoute(string keyRoute)
        {
            if(!this._routes.ContainsKey(keyRoute.ToLowerInvariant()))
                throw new Exception("A Rota informada não foi encontrada");

            return this._routes[keyRoute.ToLowerInvariant()];
        }

    }
}