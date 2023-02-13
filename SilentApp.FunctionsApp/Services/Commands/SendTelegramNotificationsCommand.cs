using SilentApp.Domain.Enums;
using SilentApp.Services.Contracts;

namespace SilentApp.FunctionsApp.Services.Commands
{
    internal class SendTelegramNotificationsCommand : ICommand
    {
        public SendTelegramNotificationsCommand(string locationId, AlertMessageType type)
        {
            LocationId = locationId;
            Type = type;
        }

        public string LocationId { get; }

        public AlertMessageType Type { get; }
    }
}
