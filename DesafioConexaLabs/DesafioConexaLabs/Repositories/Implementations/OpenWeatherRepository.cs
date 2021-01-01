using DesafioConexaLabs.Entities;
using DesafioConexaLabs.Repositories.Interfaces;
using DesafioConexaLabs.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DesafioConexaLabs.Repositories.Implementations
{
    public class OpenWeatherRepository : ICityRepository
    {
        private readonly string _mainEndpoint = "http://api.openweathermap.org/data/2.5/";
        private readonly string _cityNameSufix = "weather?q={city_name}&appid={API_key}&units=metric";
        private readonly string _coordinateSefix = "weather?lat={lat}&lon={lon}&appid={API_key}&units=metric";
        private string _apiKey { get; }

        public OpenWeatherRepository()
        {
            _apiKey = AppConfigUtil.GetValue("OpenWeatherCredentials:APIKey");
        }

        public async Task<City> GetCity(string latitude, string longitude)
        {
            Coordinate coordinates = new Coordinate(latitude, longitude);

            if (!coordinates.IsValid())
            {
                throw new Exception("Invalid coordinates");
            }

            string endpoint = _mainEndpoint + _coordinateSefix
                .Replace("{lat}",coordinates.Latitude)
                .Replace("{lon}", coordinates.Longitude)
                .Replace("{API_key}", _apiKey);

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var json = JObject.Parse(await response.Content.ReadAsStringAsync());
                    string cityName = json["name"].ToString();
                    float temperature = float.Parse(json["main"]["temp"].ToString());
                    City city = new City(cityName, temperature);

                    return city;
                }

                throw new Exception("Failed to get city by coordinates. " + response.Content.ToString());
            }
            
        }

        public async Task<City> GetCity(string cityName)
        {
            string endpoint = _mainEndpoint + _cityNameSufix
                .Replace("{city_name}", cityName)
                .Replace("{API_key}", _apiKey);

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var json = JObject.Parse(await response.Content.ReadAsStringAsync());
                    var jsonName = json["name"];
                    var jsonTemp = json["main"]["temp"];

                    // Throws an error if the city has no name or temperature
                    if (jsonName != null && jsonTemp != null)
                    {
                        string name = json["name"].ToString();
                        float temperature = float.Parse(json["main"]["temp"].ToString());
                        City city = new City(name, temperature);

                        return city;
                    }
                }
                if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception("City not found." + response.Content.ToString());
                }

                throw new Exception("Failed to get city by name. " + response.Content.ToString());
            }
        }
    }
}
