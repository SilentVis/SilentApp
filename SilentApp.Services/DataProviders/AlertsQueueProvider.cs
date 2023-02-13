using System.Text.Json;
using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using SilentApp.Domain.DTO.Internal;
using SilentApp.Infrastructure;
using SilentApp.Services.DataProviders.Contracts;

namespace SilentApp.Services.DataProviders
{
    internal class AlertsQueueProvider : IAlertsQueueDataProvider
    {
        private readonly QueueClient _queueClient;

        public AlertsQueueProvider(IConfiguration configuration)
        {
            var connectionString = configuration[ConfigurationKeyConstants.QueueConnectionString];
            var queueName = ConfigurationKeyConstants.AlertQueueName;

            _queueClient = new QueueClient(connectionString, queueName, new QueueClientOptions()
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });
        }


        public Task Send(AlertMessage message)
        {
            var formattedMessage = JsonSerializer.Serialize(message);

            return _queueClient.SendMessageAsync(formattedMessage);
        }
    }
}
