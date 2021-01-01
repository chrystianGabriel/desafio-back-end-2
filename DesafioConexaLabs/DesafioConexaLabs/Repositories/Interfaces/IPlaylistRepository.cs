using DesafioConexaLabs.Entities;
using System.Threading.Tasks;

namespace DesafioConexaLabs.Repositories.Interfaces
{
    public interface IPlaylistRepository : IBasePlaylistRepository
    {
        Task<Playlist> GetPlaylistByCityTemperature(City city);
    }
}
