using Flunt.Validations;
using PlaylistRecommender.Domain.Core;

namespace PlaylistRecommender.Domain.ValueObjects
{
    public class Coordinates : ValueObject
    {
        public Coordinates(string latitude, string longitude)
        {
            Latitude = latitude;
            Longitude = longitude;

            AddNotifications(new Contract()
                .Requires()
                .IsNotNullOrEmpty(Latitude, "Coordinates.Latitude", "Latitude deve ser preenchida")
                .IsNotNullOrEmpty(Longitude, "Coordinates.Longitude", "Longitude deve ser preenchida")
            );
        }
        public string Latitude { get; private set; }
        public string Longitude { get; private set; }
    }
}