using Microsoft.Extensions.Configuration;
using SilentApp.Infrastructure;
using Telegram.Bot;

namespace SilentApp.Services.Integrations
{
    public class TelegramBotClientFactory
    {
        public static ITelegramBotClient Create(IConfiguration configuration)
        {
            var apiKey = configuration[ConfigurationKeyConstants.TelegramApiKey];

            var options = new TelegramBotClientOptions(apiKey);
            var client = new TelegramBotClient(options);

            return client;
        }
    }
}
