namespace SilentApp.Domain.Entities
{
    public class AlertNotificationSubscription : BaseStorageEntity
    {
        public const string EntityPartitionKey = "Subscriptions";

        public long? TelegramChatId { get; set; }

        public string? TelegramUsername { get; set; }

        public string LocationId { get; set; }

        public bool IsUsernameBased => !string.IsNullOrWhiteSpace(TelegramUsername);
    }
}
