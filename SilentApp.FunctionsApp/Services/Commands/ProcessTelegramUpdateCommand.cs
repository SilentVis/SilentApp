using SilentApp.Services.Contracts;
using Telegram.Bot.Types;

namespace SilentApp.FunctionsApp.Services.Commands
{
    public class ProcessTelegramUpdateCommand : ICommand
    {
        public ProcessTelegramUpdateCommand(Update update)
        {
            Update = update;
        }

        public Update Update { get; }
    }
}
