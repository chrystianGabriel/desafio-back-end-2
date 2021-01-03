using Flunt.Notifications;
using PlaylistRecommender.Domain.Core;

namespace PlaylistRecommender.Domain.Commands
{
    public class CommandResult : ICommandResult
    {
        public CommandResult(bool success, string message, object data)
        {
            this.Success = success;
            this.Message = message;
            this.Data = data;
        }
        
        public CommandResult(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
        }
        
        public CommandResult(bool success, object data)
        {
            this.Success = success;
            this.Data = data;
        }

        public bool Success { get; private set; }
        public string Message { get; private set; }
        public object Data { get; private set; }
    }
}