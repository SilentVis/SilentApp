using Azure.Data.Tables;
using Azure.Storage.Queues;
using SilentApp.Domain.DTO.Internal;
using SilentApp.Domain.Entities;

namespace SilentApp.Services.DataProviders.Contracts
{
    public interface IAzureStorageClientFactory
    {
        Task<TableClient> CreateTableClient<T>() where T : BaseTableEntity;

        Task<QueueClient> CreateQueueClient<T>() where T : BaseQueueMessage;
    }
}
