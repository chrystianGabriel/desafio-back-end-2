using Flunt.Validations;
using PlaylistRecommender.Domain.Core;

namespace PlaylistRecommender.Domain.Entities
{
    public class Track : Entity
    {
        public string Name {  get; private set; }

        public Track(string name)
        {   
            Name = name;     
             
            AddNotifications(new Contract()
              .Requires()
              .IsNotNullOrEmpty(name, "Track.Name", "O nome da musica deve ser informado")
            );
        }
    }
}