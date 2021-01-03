namespace PlaylistRecommender.Domain.Core
{
    public interface ICommandResult
    {
        bool Success { get; }
        string Message { get; }
        object Data { get; }

    }
}