using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace PlaylistRecommender.Domain.Core
{
    public class ProviderSettings
    {
        private static ProviderSettings _instance;

        [JsonProperty("providers")]
        private List<ProviderConfig> Providers = new List<ProviderConfig>();
        public static ProviderSettings Instance {
            get 
            {
                if(_instance == null) _instance = GetSettingsFromFile();

                return _instance;
            }
            private set {}
        }

        private static ProviderSettings GetSettingsFromFile()
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "providerssettings.json");

                return JsonConvert.DeserializeObject<ProviderSettings>(File.ReadAllText(path.ToString()));
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        
        public ProviderConfig GetProviderConfig(string providerKey)
        {
            var provider = Providers.Find(p => p.ProviderKey == providerKey);

             if(provider == null)
                 throw new Exception("O Provider informado não foi encontrado, verifique o arquivo de configuração 'providerssettings'");

            return provider;
        }
    }
}