using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SilentApp.Services.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SilentApp.FunctionsApp.Services.Commands.Handlers
{
    public class ProcessTelegramUpdateCommandHandler : ICommandHandler<ProcessTelegramUpdateCommand>
    {
        private readonly ITelegramBotClient _telegramBotClient;

        public ProcessTelegramUpdateCommandHandler(ITelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        public async Task<RequestResult> HandleAsync(ProcessTelegramUpdateCommand command)
        {
            var chatId = new ChatId(command.Update.Message.Chat.Id);

            await _telegramBotClient.SendTextMessageAsync(chatId, "templateMessage");

            return new RequestResult();
        }
    }
}
