using SilentApp.Domain.Entities;
using SilentApp.Services.Contracts;
using System.Threading.Tasks;
using SilentApp.Services.DataProviders.Contracts;

namespace SilentApp.FunctionsApp.Services.Commands.Handlers
{
    public class RegisterSubscriptionCommandHandler : ICommandHandler<RegisterSubscriptionCommand>
    {
        private readonly IAzureTableDataProvider _dataProvider;

        public RegisterSubscriptionCommandHandler(IAzureTableDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async Task<RequestResult> HandleAsync(RegisterSubscriptionCommand command)
        {
            // Transform the command data into the AlertNotificationSubscription object
            var subscription = new AlertNotificationSubscription
            {
                PartitionKey = AlertNotificationSubscription.EntityPartitionKey,
                TelegramChatId = command.ChatId,
                LocationId = command.LocationId,
                RowKey = $"{command.ChatId}_{command.LocationId}"
            };

            // Send the object to the data provider
            await _dataProvider.UpsertRecord(subscription);

            // Return the result
            return new RequestResult();
        }
    }
}
