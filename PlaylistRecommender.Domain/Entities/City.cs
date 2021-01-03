using System;
using Flunt.Validations;
using PlaylistRecommender.Domain.Core;
using PlaylistRecommender.Domain.ValueObjects;

namespace PlaylistRecommender.Domain.Entities
{
    public class City : Entity
    {
        public City(string name, Coordinates coordinates)
        {
            Coordinates = coordinates;
            Name = name;

            if(coordinates.Invalid)
            {
                AddNotifications(new Contract()
                  .Requires()
                  .IsNotNullOrEmpty(Name, "City.Name", "O Nome da cidade é obrigario quando as coordenadas não são informadas"));
            }    
        }
        public string Name { get; private set; }
        public Coordinates Coordinates { get; private set;}
    }
}