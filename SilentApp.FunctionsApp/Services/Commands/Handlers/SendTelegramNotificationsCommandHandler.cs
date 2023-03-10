using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SilentApp.Domain.Entities;
using SilentApp.Domain.Enums;
using SilentApp.Services.Contracts;
using SilentApp.Services.DataProviders.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types;
using Location = SilentApp.Domain.Entities.Location;

namespace SilentApp.FunctionsApp.Services.Commands.Handlers
{
    internal class SendTelegramNotificationsCommandHandler : ICommandHandler<SendTelegramNotificationsCommand>
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IAzureTableDataProvider _azureTableDataProvider;
        private readonly ILogger _logger;

        private const string StartAlertMessageTemplate = "\U0001f170 {0}: AIR ALERT! \U0001f170";
        private const string FinishAlertMessageTemplate = "\U00002747 {0}: Finish alert! \U00002747";

        public SendTelegramNotificationsCommandHandler(ITelegramBotClient telegramBotClient, IAzureTableDataProvider azureTableDataProvider, ILogger logger)
        {
            _telegramBotClient = telegramBotClient;
            _azureTableDataProvider = azureTableDataProvider;
            _logger = logger;
        }

        public async Task<RequestResult> HandleAsync(SendTelegramNotificationsCommand command)
        {
            var locationId = command.LocationId;
            var location = await _azureTableDataProvider.GetRecord<Location>(Location.EntityPartitionKey, command.LocationId);

            if (location.Type != LocationType.Region)
            {
                locationId = location.RegionId;
            }

            var subscriptions = (await
                    _azureTableDataProvider.GetRecords<AlertNotificationSubscription>(s =>
                        s.PartitionKey == AlertNotificationSubscription.EntityPartitionKey &&
                        s.LocationId == locationId))
                .ToList();

            if (!subscriptions.Any())
            {
                return new RequestResult();
            }

            var messageTemplate = command.Type == AlertMessageType.StartAlert
                ? StartAlertMessageTemplate
                : FinishAlertMessageTemplate;

            var message = string.Format(messageTemplate, location.Name);

            foreach (var subscription in subscriptions)
            {
                if (string.IsNullOrWhiteSpace(subscription.TelegramUsername) && !subscription.TelegramChatId.HasValue)
                {
                    _logger.LogWarning("Subscription row key: {SubscriptionRowKey} has no data to send a message", subscription.RowKey);
                    continue;
                }

                if (subscription.IsUsernameBased)
                {
                    await _telegramBotClient.SendTextMessageAsync(new ChatId(subscription.TelegramUsername), message);
                }
                else
                {
                    await _telegramBotClient.SendTextMessageAsync(new ChatId(subscription.TelegramChatId.Value), message);
                }

            }

            return new RequestResult();
        }
    }
}
