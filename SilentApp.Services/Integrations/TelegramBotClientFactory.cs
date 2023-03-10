using SilentApp.Infrastructure.Configuration;
using SilentApp.Infrastructure.Constants;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace SilentApp.Services.Integrations
{
    public class TelegramBotClientFactory
    {
        public static ITelegramBotClient Create(SilentAppConfiguration configuration)
        {
            var apiKey = configuration.Telegram.ApiKey;
            var webhookUrl = $"{configuration.HostUrl}/api/{AppConstants.TelegramWebhookFunctionName}";

            var options = new TelegramBotClientOptions(apiKey);
            var client = new TelegramBotClient(options);

            if (!configuration.IsDevelopmentEnv)
            {
                client.SetWebhookAsync(
                    url: webhookUrl,
                    allowedUpdates: new List<UpdateType>() { UpdateType.Message })
                    .GetAwaiter().GetResult();
            }

            return client;
        }
    }
}