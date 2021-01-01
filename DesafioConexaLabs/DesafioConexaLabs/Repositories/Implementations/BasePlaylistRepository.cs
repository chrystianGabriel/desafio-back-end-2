using DesafioConexaLabs.Entities.Enum;
using DesafioConexaLabs.Repositories.Interfaces;

namespace DesafioConexaLabs.Repositories.Implementations
{
    public class BasePlaylistRepository : IBasePlaylistRepository
    {
        public EPlaylistCategory GetPlaylistCategory(float temperature)
        {
            if(temperature > 30f)
            {
                return EPlaylistCategory.PARTY;
            }
            if(temperature >= 15f && temperature <= 30f)
            {
                return EPlaylistCategory.ROCK;
            }
            if(temperature >= 10f && temperature < 15f)
            {
                return EPlaylistCategory.POP;
            }
            // if temperature < 10
            return EPlaylistCategory.CLASSICAL;
        }
    }
}
