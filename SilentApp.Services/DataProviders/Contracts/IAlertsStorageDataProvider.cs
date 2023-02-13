using System.Linq.Expressions;
using SilentApp.Domain.Entities;

namespace SilentApp.Services.DataProviders.Contracts
{
    public interface IAzureStorageTableDataProvider
    {
        Task<IEnumerable<T>> GetRecords<T>(string partitionKey) where T : BaseStorageEntity;

        Task<IEnumerable<T>> GetRecords<T>(Expression<Func<T, bool>> filter) where T : BaseStorageEntity;

        Task<T> GetRecord<T>(string partitionKey, string rowKey) where T : BaseStorageEntity;

        Task<T> GetRecord<T>(Expression<Func<T, bool>> filter) where T : BaseStorageEntity;

        Task UpsertRecord<T>(T entity) where T : BaseStorageEntity;

        Task DeleteRecord(string partitionKey, string rowKey);
    }
}
