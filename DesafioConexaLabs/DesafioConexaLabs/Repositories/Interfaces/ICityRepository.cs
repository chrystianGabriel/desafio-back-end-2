using DesafioConexaLabs.Entities;
using System.Threading.Tasks;

namespace DesafioConexaLabs.Repositories.Interfaces
{
    public interface ICityRepository
    {
        Task<City> GetCity(string cityName);
        Task<City> GetCity(string latitude, string longitude);
    }
}
