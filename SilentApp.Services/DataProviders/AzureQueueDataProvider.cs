using System.Text.Json;
using SilentApp.Domain.DTO.Internal;
using SilentApp.Services.DataProviders.Contracts;

namespace SilentApp.Services.DataProviders
{
    internal class AzureQueueDataProvider : IAzureQueueDataProvider
    {
        private readonly IAzureStorageClientFactory _azureStorageClientFactory;

        public AzureQueueDataProvider(IAzureStorageClientFactory azureStorageClientFactory)
        {
            _azureStorageClientFactory = azureStorageClientFactory;
        }

        public async Task Send<T>(T message) where T : BaseQueueMessage
        {
            var client = await _azureStorageClientFactory.CreateQueueClient<T>();

            var serializedMessage = JsonSerializer.Serialize(message);

            await client.SendMessageAsync(serializedMessage);
        }
    }
}
