using PlaylistRecommender.Domain.Commands;
using PlaylistRecommender.Domain.Handlers;
using Xunit;

namespace PlaylistRecommender.Tests.Handler
{
    public class PlaylistRecommendationHandlerTests
    {
        [Fact]
        public void HandleCommandInvalid()
        {
           var command = new PlaylistRecommendationCommand();

           var handler = new PlaylistRecommendationHandler();

           var result = handler.Handle(command);
           Assert.Equal(false, result.Success);
        }

        [Fact]
        public void HandleCommandValid()
        {
           var command = new PlaylistRecommendationCommand() 
           {
               CityName = "Goiania"
           };

           var handler = new PlaylistRecommendationHandler();

           var result = handler.Handle(command);
           Assert.Equal(true, result.Success);
        }
    }
}