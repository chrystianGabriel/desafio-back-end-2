using System;
using Flunt.Notifications;
using PlaylistRecommender.Domain.Commands;
using PlaylistRecommender.Domain.Core;
using PlaylistRecommender.Domain.Entities;
using PlaylistRecommender.Domain.Providers;
using PlaylistRecommender.Domain.ValueObjects;

namespace PlaylistRecommender.Domain.Handlers
{
    public class PlaylistRecommendationHandler :
        Notifiable,
        IHandler<PlaylistRecommendationCommand>
    {
        public ICommandResult Handle(PlaylistRecommendationCommand command)
        {
            try
            {
                command.Validate();

                if(command.Invalid)
                {
                    AddNotifications(command);
                    return new CommandResult(false, "Não foi possivel obter recomendação de playlist, verifique as inconsistencias", this.Notifications);
                }

                var city = new City(command.CityName,
                           new Coordinates(command.CityLatitude, command.CityLongitude));
                
                var weather = WeatherMapsProvider.Instance.GetCityWeather(city).Result;
                var recommendation = PlaylistProvider.Instance.GetPlaylistRecommedation(weather).Result;

                AddNotifications(city, weather, recommendation);

                return new CommandResult(true, recommendation);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}