#pragma warning disable CS8618
namespace SilentApp.Domain.Entities
{
    public class AlertNotificationSubscription : BaseTableEntity
    {
        public const string EntityPartitionKey = "Subscriptions";

        public long? TelegramChatId { get; set; }

        public string? TelegramUsername { get; set; }

        public string LocationId { get; set; }

        public string Notes { get; set; }

        public bool IsUsernameBased => !string.IsNullOrWhiteSpace(TelegramUsername);
    }
}
