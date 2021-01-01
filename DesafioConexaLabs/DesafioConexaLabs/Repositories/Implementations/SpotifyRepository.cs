using DesafioConexaLabs.Entities;
using DesafioConexaLabs.Entities.Enum;
using DesafioConexaLabs.Repositories.Interfaces;
using DesafioConexaLabs.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DesafioConexaLabs.Repositories.Implementations
{
    public class SpotifyRepository : BasePlaylistRepository, IPlaylistRepository
    {
        private readonly string _baseEndpoint = "https://api.spotify.com/v1";
        private readonly string _browseCategorySufix = "/browse/categories/{category}/playlists?limit=1";
        private readonly string _getPlaylistSongsSufix = "/playlists/{playlist_id}/tracks?offset=0&limit=20";

        private readonly string _authEndpoint = "https://accounts.spotify.com/api/token";
        private string _clientID { get; }
        private string _secretID { get; }

        public string _acessToken = null;
        public string AccessToken
        {
            get
            {
                if(_acessToken == null || _acessToken == string.Empty)
                {
                    _acessToken = RefreshAcessToken().Result;
                }
                return _acessToken;
            }
            set
            {
                _acessToken = value;
            }
        }

        public SpotifyRepository()
        {
            _clientID = AppConfigUtil.GetValue("SpotifyCredentials:ClientID");
            _secretID = AppConfigUtil.GetValue("SpotifyCredentials:SecretID");
        }

        public async Task<Playlist> GetPlaylistByCityTemperature(City city)
        {
            EPlaylistCategory playlistCategory = base.GetPlaylistCategory(city.Temperature);
            string playlistID = await GetPlaylistId(playlistCategory);
            List<Track> songsList = await GetPlaylistSongs(playlistID);
            Playlist playlist = new Playlist(playlistCategory, songsList);
            return playlist;
        }

        private async Task<string> RefreshAcessToken()
        {
            using (HttpClient client = new HttpClient())
            {
                string token = $"{_clientID}:{_secretID}";
                var encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", encodedToken);

                var parameters = new Dictionary<string, string>();
                parameters.Add("grant_type", "client_credentials");
                parameters.Add("Content-Type", "application/x-www-form-urlencoded");

                var response = await client.PostAsync(_authEndpoint, new FormUrlEncodedContent(parameters));

                if (response.IsSuccessStatusCode)
                {
                    var json = JObject.Parse(await response.Content.ReadAsStringAsync());

                    return json["access_token"].ToString();
                }
                throw new Exception("Error on the spotify authentication!");
            }
        }

        private async Task<string> GetPlaylistId(EPlaylistCategory category)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);

                string endpoint = _baseEndpoint + _browseCategorySufix
                    .Replace("{category}", category.ToString().ToLower());

                var response = await client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var json = JObject.Parse(await response.Content.ReadAsStringAsync());

                    // Gets only the first playlist
                    return json["playlists"]["items"][0]["id"].ToString();
                }

                throw new Exception("Failed to get playlist ID on spotify. " + response.Content.ToString());
            }
        }

        private async Task<List<Track>> GetPlaylistSongs(string playlistID)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);

                string endpoint = _baseEndpoint + _getPlaylistSongsSufix
                    .Replace("{playlist_id}", playlistID);

                var response = await client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var json = JObject.Parse(await response.Content.ReadAsStringAsync());

                    var itemsList = json["items"];
                    List<Track> songsList = new List<Track>();
                    foreach(var item in itemsList)
                    {
                        var jsonSongName = item["track"]["name"];

                        // Ignore songs without names
                        if(jsonSongName != null)
                        {
                            Track track = new Track(jsonSongName.ToString());
                            songsList.Add(track);
                        }
                    }

                    return songsList;
                }

                throw new Exception("Failed to get songs from the playlist " + response.Content.ToString());
            }
        }
    }
}
