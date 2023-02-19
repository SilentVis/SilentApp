using Azure.Data.Tables;
using Azure.Storage.Queues;
using SilentApp.Domain.DTO.Internal;
using SilentApp.Domain.Entities;
using SilentApp.Infrastructure.Configuration;
using SilentApp.Services.DataProviders.Contracts;

namespace SilentApp.Services.DataProviders
{
    internal class AzureStorageClientFactory : IAzureStorageClientFactory
    {
        private readonly IStorageNameIndex _storageNameIndex;
        private readonly string _storageConnectionString;

        public AzureStorageClientFactory(IStorageNameIndex storageNameIndex, SilentAppConfiguration configuration)
        {
            _storageNameIndex = storageNameIndex;
            _storageConnectionString = configuration.AzureStorage.ConnectionString;
        }

        public async Task<TableClient> CreateTableClient<T>() where T : BaseTableEntity
        {
            var tableName = _storageNameIndex.GetTableName(nameof(T));

            var client = new TableClient(_storageConnectionString, tableName);

            await client.CreateIfNotExistsAsync();

            return client;
        }

        public async Task<QueueClient> CreateQueueClient<T>() where T : BaseQueueMessage
        {
            var queueName = _storageNameIndex.GetQueueName(nameof(T));
            var client = new QueueClient(_storageConnectionString, queueName, new QueueClientOptions()
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });

            await client.CreateIfNotExistsAsync();

            return client;
        }
    }
}
