using DesafioConexaLabs.Entities.Enum;

namespace DesafioConexaLabs.Repositories.Interfaces
{
    public interface IBasePlaylistRepository
    {
        EPlaylistCategory GetPlaylistCategory(float temperature);
    }
}
