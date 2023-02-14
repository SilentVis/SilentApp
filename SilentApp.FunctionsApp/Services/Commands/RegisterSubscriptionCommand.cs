using SilentApp.Services.Contracts;

namespace SilentApp.FunctionsApp.Services.Commands
{
    public class RegisterSubscriptionCommand : ICommand
    {
        public RegisterSubscriptionCommand(int chatId, string locationId)
        {
            ChatId = chatId;
            LocationId = locationId;
        }

        public int ChatId { get; }
        public string LocationId { get; }
    }
}
