namespace SilentApp.Infrastructure
{
    public class ConfigurationKeyConstants
    {
        public const string ConfigConnectionString = "SILENTAPP_CONFIG_CONNECTION_STRING";

        public const string TelegramApiKey = "TelegramApiKey";

        public const string AlertsApiUrl = "AlertsApiUrl";
        public const string AlertsApiKey = "AlertsApiKey";

        public const string AzureStorageConnectionString = "AzureStorageConnectionString";
        public const string AzureStorageTableName = "AzureStorageTableName";

        public const string QueueConnectionString = "AzureStorageConnectionString";
        public const string AlertQueueName = "alerts-queue";
    }
}
