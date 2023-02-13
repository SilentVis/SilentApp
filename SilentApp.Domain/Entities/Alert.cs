using SilentApp.Domain.Enums;

namespace SilentApp.Domain.Entities
{
    public class Alert : BaseStorageEntity
    {
        public const string EntityPartitionKey = "Alerts";

        public string LocationId { get; set; }

        public string Notes { get; set; }

        public AlertType AlertType { get; set; }
    }
}
