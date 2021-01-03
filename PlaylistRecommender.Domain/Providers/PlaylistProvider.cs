using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlaylistRecommender.Domain.Core;
using PlaylistRecommender.Domain.Entities;

namespace PlaylistRecommender.Domain.Providers
{
    public class PlaylistProvider : Provider<Playlist>
    {
        private static PlaylistProvider _instance { get; set; }

        private const string AUTHORIZATION_URL_KEY = "AUTHORIZATION";
        private const string ROUTE_AUTHORIZATION = "ROUTE_AUTHORIZATION";
        private const string ROUTE_RECOMMENDATION = "ROUTE_RECOMMENDATION";

        private DateTime AccessTokenValidity { get; set; }

        private string AccessToken { get; set; }
        
        public static PlaylistProvider Instance {
            get {

                if(_instance == null)  _instance = new PlaylistProvider();

                return _instance;
            }
            private set {}
        }
        private PlaylistProvider() : base("Playlist") 
        {
        }

        public async Task Authenticate()
        {
           var url = this.GetUrl(AUTHORIZATION_URL_KEY, ROUTE_AUTHORIZATION);

           using(var request = new HttpRequestMessage(new HttpMethod("POST"), url))
           {
              var plainTextToken = Encoding.UTF8.GetBytes(ProviderConfig.ApiKey);
                
              this.AddHeaders("Authorization", $"Basic {Convert.ToBase64String(plainTextToken)}");

              request.Content = new StringContent("grant_type=client_credentials");
              request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

              var jsonConfig = await this.SendRequest(request);
              this.ConfigureAccessToken(jsonConfig);
           }
        }

        public bool AccessTokenIsValid(){
            if(this.AccessTokenValidity == null) return false;

            if(String.IsNullOrEmpty(this.AccessToken)) return false;

            return this.AccessTokenValidity > DateTime.Now;
        }
        public async Task<Playlist> GetPlaylistRecommedation(Weather weather)
        {
            var genre = this.GetGenreOfPlaylistByTemperature(weather.Temperature);
            this.AddUrlParams("seed_genres", genre);

            var url = this.GetUrl(ROUTE_RECOMMENDATION);
            using(var request = new HttpRequestMessage(new HttpMethod("GET"), url)) 
            {

                if(!this.AccessTokenIsValid())
                {
                    await this.Authenticate();
                }

                this.AddHeaders("ContentType", "application/json");
                this.AddHeaders("Accept", "application/json");
                this.AddHeaders("Authorization", $"Bearer {this.AccessToken}");

                return await this.SendRequestAndConvertResponse(request);
            }
        } 
        protected override Playlist ConvertObject(string json)
        {
           var jsonObject = JsonConvert.DeserializeObject<JObject>(json);
           var result = new Playlist(jsonObject["seeds"][0]["id"].ToString());

           foreach(var track in jsonObject["tracks"])
           {
               var trackName = track["album"]["name"].ToString();
               result.AddTrack(new Track(trackName));
           }

           return result;
        }

        private void ConfigureAccessToken(string jsonToken)
        {
            var jsonObject = JsonConvert.DeserializeObject<JObject>(jsonToken);
            this.AccessToken = jsonObject["access_token"].ToString();
            this.AccessTokenValidity = DateTime.Now.AddSeconds(Convert.ToInt64(jsonObject["expires_in"].ToString()));
        }

        private string GetGenreOfPlaylistByTemperature(double temperature){

            if(temperature > 30) return "party";
            if(temperature > 14 && temperature < 31) return "pop";
            if(temperature > 9 && temperature < 15) return "rock";

            return "classical";
        }
    }
}