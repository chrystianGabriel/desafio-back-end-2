using PlaylistRecommender.Domain.Core;

namespace PlaylistRecommender.Domain.Entities
{
    public class Weather : Entity
    {
        public Weather(double temperature)
        {
            this.Temperature = temperature;
        }
        public double Temperature { get; private set; }
    }
}