using System.Linq.Expressions;
using SilentApp.Domain.Entities;

namespace SilentApp.Services.DataProviders.Contracts
{
    public interface IAzureTableDataProvider
    {
        Task<IEnumerable<T>> GetRecords<T>(string partitionKey) where T : BaseTableEntity;

        Task<IEnumerable<T>> GetRecords<T>(Expression<Func<T, bool>> filter) where T : BaseTableEntity;

        Task<T> GetRecord<T>(string partitionKey, string rowKey) where T : BaseTableEntity;

        Task<T?> GetRecord<T>(Expression<Func<T, bool>> filter) where T : BaseTableEntity;

        Task UpsertRecord<T>(T entity) where T : BaseTableEntity;

        Task DeleteRecord<T>(string partitionKey, string rowKey) where T: BaseTableEntity;
    }
}
