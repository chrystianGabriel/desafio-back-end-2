using DesafioConexaLabs.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using DesafioConexaLabs.Repositories.Interfaces;
using Newtonsoft.Json;

namespace DesafioConexaLabs.Controllers
{
    [ApiController]
    [Route("api/[action]")]
    public class PlaylistRecommendationController : ControllerBase
    {
        private ICityRepository _cityRepository { get; }
        private IPlaylistRepository _playlistRepository { get; }

        public PlaylistRecommendationController(ICityRepository cityRepository, IPlaylistRepository playlistRepository)
        {
            this._cityRepository = cityRepository;
            this._playlistRepository = playlistRepository;
        }

        /// <summary>
        /// Finds a city using it's name then uses the city's temperature to choose a playlist genre and returns it's songs names in a list
        /// </summary>
        /// <param name="cityName">Name of the city</param>
        /// <returns>A list of songs names in the playlist</returns>
        [HttpGet("city={cityName}")]
        public async Task<ActionResult> GetPlaylistByCityTemperature(string cityName)
        {
            try
            {
                City city = await this._cityRepository.GetCity(cityName);
                Playlist playlist = await this._playlistRepository.GetPlaylistByCityTemperature(city);
                string json = JsonConvert.SerializeObject(playlist);
                return Ok(json);
            }
            catch (Exception ex)
            {
                return Problem(title: ex.Message, statusCode: 400);
            }
        }

        /// <summary>
        /// Finds a city using it's name then uses the city's temperature to choose a playlist genre and returns it's songs names in a list
        /// </summary>
        /// <param name="latitude">Latitude of the city</param>
        /// <param name="longitude">Longitude of the city</param>
        /// <returns>A list of songs names in the playlist</returns>
        [HttpGet("lat={latitude}&lon={longitude}")]
        public async Task<ActionResult> GetPlaylistByCityTemperature(string latitude, string longitude)
        {
            try
            {
                Coordinate coordinates = new Coordinate(latitude, longitude);
                City city = await _cityRepository.GetCity(latitude, longitude);
                Playlist playlist = await this._playlistRepository.GetPlaylistByCityTemperature(city);
                string json = JsonConvert.SerializeObject(playlist);
                return Ok(json);
            }
            catch (Exception ex)
            {
                return Problem(title: ex.Message, statusCode: 400);
            }
        }
    }
}
