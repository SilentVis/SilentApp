using Azure;
using Azure.Data.Tables;
#pragma warning disable CS8618

namespace SilentApp.Domain.Entities
{
    public abstract class BaseTableEntity : ITableEntity
    {
        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public DateTimeOffset? Timestamp { get; set; }

        public ETag ETag { get; set; }
    }
}
