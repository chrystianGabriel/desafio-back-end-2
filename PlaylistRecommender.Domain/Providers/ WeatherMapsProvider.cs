using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlaylistRecommender.Domain.Core;
using PlaylistRecommender.Domain.Entities;

namespace PlaylistRecommender.Domain.Providers
{
    public class  WeatherMapsProvider : Provider<Weather>
    {
        private static WeatherMapsProvider _instance { get; set; }
        private const string ROUTE_GET_WEATHER = "ROUTE_GET_WEATHER";
        public static WeatherMapsProvider Instance {
            get {

                if(_instance == null)  _instance = new WeatherMapsProvider();

                return _instance;
            }
            private set {}
        }
        private WeatherMapsProvider() : base("WeatherMaps") 
        {
        }

        public async Task<Weather> GetCityWeather(City city)
        {

            try
            {
                this.AddUrlParams("appid", ProviderConfig.ApiKey);
                this.AddUrlParams("units", "metric");

                if(city.Coordinates.Valid)
                {
                    this.AddUrlParams("lat", city.Coordinates.Latitude);
                    this.AddUrlParams("lon", city.Coordinates.Longitude);
                }

                if(!String.IsNullOrEmpty(city.Name))
                {
                    this.AddUrlParams("q", city.Name);
                }

                var url = this.GetUrl(ROUTE_GET_WEATHER);

                using(var request = new HttpRequestMessage(new HttpMethod("GET"), url))
                {
                    return await this.SendRequestAndConvertResponse(request);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        protected override Weather ConvertObject(string json)
        {
            var jsonObject = JsonConvert.DeserializeObject<JObject>(json);
            var temperature = jsonObject["main"]["temp"];

            return new Weather(Convert.ToDouble(temperature));
        }
    }
}