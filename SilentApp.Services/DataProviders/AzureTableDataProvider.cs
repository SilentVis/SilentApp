using System.Linq.Expressions;
using Azure;
using Microsoft.Extensions.Logging;
using SilentApp.Domain.Entities;
using SilentApp.Services.DataProviders.Contracts;

namespace SilentApp.Services.DataProviders
{
    public class AzureTableDataProvider : IAzureTableDataProvider
    {
        private readonly ILogger _log;
        private readonly IAzureStorageClientFactory _azureStorageClientFactory;

        public AzureTableDataProvider(ILogger log, IAzureStorageClientFactory azureStorageClientFactory)
        {
            _log = log;
            _azureStorageClientFactory = azureStorageClientFactory;
        }

        public async Task<IEnumerable<T>> GetRecords<T>(string partitionKey) where T : BaseTableEntity
        {
            var client = await _azureStorageClientFactory.CreateTableClient<T>();

            var pages = client
                .QueryAsync<T>(x => x.PartitionKey == partitionKey)
                .AsPages();
            return await FormatResults(pages);
        }
        
        public async Task<IEnumerable<T>> GetRecords<T>(Expression<Func<T, bool>> filter) where T : BaseTableEntity
        {
            var client = await _azureStorageClientFactory.CreateTableClient<T>();

            var pages = client.QueryAsync<T>(filter).AsPages();
            return await FormatResults(pages);
        }

        public async Task<T> GetRecord<T>(string partitionKey, string rowKey) where T : BaseTableEntity
        {
            var client = await _azureStorageClientFactory.CreateTableClient<T>();

            var record = await client.GetEntityAsync<T>(partitionKey, rowKey);

            return record;
        }

        public async Task<T?> GetRecord<T>(Expression<Func<T, bool>> filter) where T : BaseTableEntity
        {
            var client = await _azureStorageClientFactory.CreateTableClient<T>();

            var pages = client.QueryAsync<T>(filter).AsPages();
            var records = (await FormatResults(pages)).ToList();

            switch (records.Count)
            {
                case 0:
                    {
                        _log.LogError("0 results found while execution GetRecord(filter) query for entity {entityName}", nameof(T));
                        return null;
                    }
                    
                case 1:
                    return records.Single();
                default:
                {
                    _log.LogError("Too many results found while execution GetRecord(filter) query for entity {entityName}", nameof(T));
                    return null;
                }
            };
        }

        public async Task UpsertRecord<T>(T entity) where T : BaseTableEntity
        {
            var client = await _azureStorageClientFactory.CreateTableClient<T>();

            await client.UpsertEntityAsync<T>(entity);
        }

        public async Task DeleteRecord<T>(string partitionKey, string rowKey) where T : BaseTableEntity
        {
            var client = await _azureStorageClientFactory.CreateTableClient<T>();

            await client.DeleteEntityAsync(partitionKey, rowKey);
        }

        private static async Task<IEnumerable<T>> FormatResults<T>(IAsyncEnumerable<Page<T>> pages) where T : BaseTableEntity
        {
            var records = new List<T>();

            await foreach (var page in pages)
            {
                records.AddRange(page.Values);
            }

            return records;
        }
    }
}
