using PlaylistRecommender.Domain.Commands;
using Xunit;

namespace PlaylistRecommender.Tests.Commands
{
    public class PlaylistRecommendationCommandTests
    {
       [Fact]
       public void  NameAndCoordinatesNotFilled() 
       {
           var command = new PlaylistRecommendationCommand();
           command.Validate();

           Assert.Equal(true, command.Invalid);
       }

       [Fact]
       public void  NameFilledButCoordinatesNotFilled() 
       {
           var command = new PlaylistRecommendationCommand() 
           {
              CityName = "Goiania",
           };

           command.Validate();
           Assert.Equal(true, command.Valid);
       }

       [Fact]
       public void  NameNotFilledButCoordinatesFilled() 
       {
           var command = new PlaylistRecommendationCommand() 
           {
              CityLatitude = "50",
              CityLongitude = "50"
           };

           command.Validate();
           Assert.Equal(true, command.Valid);
       }
       
       [Fact]
       public void  NameFilledButCoordinatesFilledNotCorrect() 
       {
           var command = new PlaylistRecommendationCommand() 
           {
              CityName = "Goiania",
              CityLatitude = "",
              CityLongitude = ""
           };

           command.Validate();
           Assert.Equal(true, command.Valid);
       }

       [Fact]
       public void  CoordinatesFilledNotWithDigit() 
       {
           var command = new PlaylistRecommendationCommand() 
           {
              CityLatitude = "XX",
              CityLongitude = "XX"
           };

           command.Validate();
           Assert.Equal(true, command.Invalid);
       }
    }
}