namespace PlaylistRecommender.Domain.Core
{
    public interface IHandler<T> where T : ICommand
    {
        ICommandResult Handle(T command);
    }
}