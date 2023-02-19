using SilentApp.Domain.Enums;
#pragma warning disable CS8618

namespace SilentApp.Domain.Entities
{
    public class Alert : BaseTableEntity
    {
        public const string EntityPartitionKey = "Alerts";

        public string LocationId { get; set; }

        public AlertType AlertType { get; set; }
    }
}
