using System;
using Flunt.Notifications;
using Flunt.Validations;
using PlaylistRecommender.Domain.Core;

namespace PlaylistRecommender.Domain.Commands
{
    public class PlaylistRecommendationCommand : Notifiable, ICommand
    {
        public string CityName { get; set; }

        public string CityLatitude { get; set; }

        public string CityLongitude { get; set; }

        public void Validate()
        {
            if(String.IsNullOrEmpty(CityLongitude) &&
               String.IsNullOrEmpty(CityLatitude))
            {
                AddNotifications(new Contract()
                  .Requires()
                  .IsNotNullOrEmpty(CityName, "CityName", "É obrigatório informar o nome da cidade.")
                );
            }
            else 
            {
                AddNotifications(new Contract()
                  .Requires()
                  .IsNotNullOrEmpty(CityLatitude, "CityLatitude", "É obrigatório informar a latitude.")
                  .IsNotNullOrEmpty(CityLongitude, "CityLongitude", "É obrigatório informar a longitude.")
                  .IsDigit(CityLatitude, "CityLatitude", "A latitude informada não é valida")
                  .IsDigit(CityLongitude, "CityLongitude", "A longitude informada não é valida")
                );
            }
        }
    }
}