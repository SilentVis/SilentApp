namespace SilentApp.Infrastructure.Configuration
{
    public class SilentAppConfiguration
    {
        public SilentAppConfiguration(
            TelegramSettings telegram,
            AlertsApiSettings alertsApi,
            AzureStorageSettings azureStorage, string hostUrl, bool isDevelopmentEnv)
        {
            Telegram = telegram;
            AlertsApi = alertsApi;
            AzureStorage = azureStorage;
            HostUrl = hostUrl;
            IsDevelopmentEnv = isDevelopmentEnv;
        }

        public TelegramSettings Telegram { get; }

        public AlertsApiSettings AlertsApi { get; }

        public AzureStorageSettings AzureStorage { get; }

        public string HostUrl { get; }

        public bool IsDevelopmentEnv { get; }
    }
}
