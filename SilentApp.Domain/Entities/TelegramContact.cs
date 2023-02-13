namespace SilentApp.Domain.Entities
{
    public class TelegramContact : BaseStorageEntity
    {
        public const string EntityPartitionKey = "Contacts";

        public long? ChatId { get; set; }

        public string? Username { get; set; }

        public string? Notes { get; set; }
    }
}
