using PlaylistRecommender.Domain.Entities;
using PlaylistRecommender.Domain.ValueObjects;
using Xunit;

namespace PlaylistRecommender.Tests.Entities
{
    public class CityTests
    {
       [Fact]
       public void  NameAndCoordinatesNotFilled() 
       {
           var city = new City("", new Coordinates("", ""));

           Assert.Equal(true, city.Invalid);
       }

       [Fact]
       public void  NameFilledButCoordinatesNotFilled() 
       {
           var city = new City("Goiania", new Coordinates("", ""));

           Assert.Equal(true, city.Valid);
       }

       [Fact]
       public void  NameNotFilledButCoordinatesFilled() 
       {
           var city = new City("", new Coordinates("50", "50"));

           Assert.Equal(true, city.Valid);
       }
       
       [Fact]
       public void  NameFilledButCoordinatesFilledNotCorrect() 
       {
           var city = new City("Goiania", new Coordinates("DD", "DD"));

           Assert.Equal(true, city.Valid);
       }
    }
}