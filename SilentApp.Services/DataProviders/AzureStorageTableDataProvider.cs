using System.Linq.Expressions;
using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SilentApp.Domain.Entities;
using SilentApp.Infrastructure;
using SilentApp.Services.DataProviders.Contracts;

namespace SilentApp.Services.DataProviders
{
    public class AzureStorageTableDataProvider : IAzureStorageTableDataProvider
    {
        private readonly TableClient _tableClient;
        private readonly ILogger _log;

        public AzureStorageTableDataProvider(IConfiguration configuration, ILogger log)
        {
            _log = log;
            var connectionString = configuration[ConfigurationKeyConstants.AzureStorageConnectionString];
            var tableName = configuration[ConfigurationKeyConstants.AzureStorageTableName];

            _tableClient = new TableClient(connectionString, tableName);
        }

        public Task<IEnumerable<T>> GetRecords<T>(string partitionKey) where T : BaseStorageEntity
        {
            var pages = _tableClient
                .QueryAsync<T>(x => x.PartitionKey == partitionKey)
                .AsPages();
            return FormatResults(pages);
        }

        public Task<IEnumerable<T>> GetRecords<T>(string partitionKey, string[] rowKeys) where T : BaseStorageEntity
        {
            var pages = _tableClient
                .QueryAsync<T>(x => x.PartitionKey == partitionKey && rowKeys.Contains(x.RowKey))
                .AsPages();
            return FormatResults(pages);
        }

        public Task<IEnumerable<T>> GetRecords<T>(Expression<Func<T, bool>> filter) where T : BaseStorageEntity
        {
            var pages = _tableClient.QueryAsync<T>(filter).AsPages();
            return FormatResults(pages);
        }

        public async Task<T> GetRecord<T>(string partitionKey, string rowKey) where T : BaseStorageEntity
        {
            var record = await _tableClient.GetEntityAsync<T>(partitionKey, rowKey);

            return record;
        }

        public async Task<T?> GetRecord<T>(Expression<Func<T, bool>> filter) where T : BaseStorageEntity
        {
            var pages = _tableClient.QueryAsync<T>(filter).AsPages();
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

        public async Task UpsertRecord<T>(T entity) where T : BaseStorageEntity
        {
            await _tableClient.UpsertEntityAsync<T>(entity);
        }

        public Task DeleteRecord(string partitionKey, string rowKey)
        {
            return _tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        private static async Task<IEnumerable<T>> FormatResults<T>(IAsyncEnumerable<Page<T>> pages) where T : BaseStorageEntity
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
