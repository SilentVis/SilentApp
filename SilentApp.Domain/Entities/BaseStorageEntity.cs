using Azure;
using Azure.Data.Tables;

namespace SilentApp.Domain.Entities
{
    public abstract class BaseStorageEntity : ITableEntity
    {
        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public DateTimeOffset? Timestamp { get; set; }

        public ETag ETag { get; set; }
    }
}
