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
        private readonly IAzureStorageTableDataProvider _azureStorageTableDataProvider;
        private readonly ILogger _logger;

        private const string StartAlertMessage = "";
        private const string FinishAlertMessage = "";

        public SendTelegramNotificationsCommandHandler(ITelegramBotClient telegramBotClient, IAzureStorageTableDataProvider azureStorageTableDataProvider, ILogger logger)
        {
            _telegramBotClient = telegramBotClient;
            _azureStorageTableDataProvider = azureStorageTableDataProvider;
            _logger = logger;
        }

        public async Task<RequestResult> HandleAsync(SendTelegramNotificationsCommand command)
        {
            var locationId = command.LocationId;
            var location = await _azureStorageTableDataProvider.GetRecord<Location>(Location.EntityPartitionKey, command.LocationId);

            if (location.Type != LocationType.Region)
            {
                locationId = location.RegionId;
            }

            var subscriptions = (await
                    _azureStorageTableDataProvider.GetRecords<AlertNotificationSubscription>(s =>
                        s.PartitionKey == AlertNotificationSubscription.EntityPartitionKey &&
                        s.LocationId == locationId))
                .ToList();

            if (!subscriptions.Any())
            {
                return new RequestResult();
            }

            var message = command.Type == AlertMessageType.StartAlert ? StartAlertMessage : FinishAlertMessage;

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
