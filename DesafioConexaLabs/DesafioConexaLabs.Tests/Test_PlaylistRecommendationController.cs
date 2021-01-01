using NUnit.Framework;
using Moq;
using DesafioConexaLabs.Repositories.Interfaces;
using DesafioConexaLabs.Entities;
using DesafioConexaLabs.Controllers;
using System.Threading.Tasks;
using System.Collections.Generic;
using DesafioConexaLabs.Entities.Enum;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace DesafioConexaLabs.Tests
{
    public class Test_PlaylistRecommendationController
    {
        private PlaylistRecommendationController _controller { get; set; }

        [SetUp]
        public void Setup()
        {
            List<Track> songsList = new List<Track>()
            {
                new Track("Song name 1"),
                new Track("Song name 2"),
                new Track("Song name 3"),
                new Track("Song name 4"),
                new Track("Song name 5")
            };

            City testCity = new City("teste", 30f);
            Playlist testPlaylist = new Playlist(EPlaylistCategory.CLASSICAL, songsList);

            var mockCity = new Mock<ICityRepository>();
            mockCity.Setup(m => m.GetCity(testCity.Name)).ReturnsAsync(testCity);
            mockCity.Setup(m2 => m2.GetCity("10","10")).ReturnsAsync(testCity);

            var mockPlaylist = new Mock<IPlaylistRepository>();
            mockPlaylist.Setup(m => m.GetPlaylistByCityTemperature(testCity)).ReturnsAsync(testPlaylist);
            mockPlaylist.Setup(m => m.GetPlaylistCategory(testCity.Temperature)).Returns(testPlaylist.Category);

            _controller = new PlaylistRecommendationController(mockCity.Object, mockPlaylist.Object);
        }

        [Test]
        public void Test_GetPlaylistByCityTemperature_using_city_name()
        {
            var json = _controller.GetPlaylistByCityTemperature("teste");
            var result = json.Result as OkObjectResult;
            Assert.IsNotNull(result.Value);
            
            Playlist playlist = JsonConvert.DeserializeObject<Playlist>(result.Value.ToString());
            Assert.AreEqual(EPlaylistCategory.CLASSICAL, playlist.Category);
            Assert.AreEqual(5, playlist.SongsList.Count);
        }

        [Test]
        public void Test_GetPlaylistByCityTemperature_using_city_coordinates()
        {
            var json = _controller.GetPlaylistByCityTemperature("10", "10");
            var result = json.Result as OkObjectResult;
            Assert.IsNotNull(result.Value);

            Playlist playlist = JsonConvert.DeserializeObject<Playlist>(result.Value.ToString());
            Assert.AreEqual(EPlaylistCategory.CLASSICAL, playlist.Category);
            Assert.AreEqual(5, playlist.SongsList.Count);
        }
    }
}