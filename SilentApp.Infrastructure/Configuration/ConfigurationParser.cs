using Microsoft.Extensions.Configuration;
using SilentApp.Infrastructure.Constants;

namespace SilentApp.Infrastructure.Configuration
{
    public class ConfigurationParser
    {
        public static SilentAppConfiguration Parse(IConfiguration configuration)
        {
            ICollection<string> errors = new List<string>();

            var telegramApiKey = ExtractValue(configuration, ConfigurationKeyConstants.TelegramApiKey,  errors);
            var alertsApiUrl = ExtractValue(configuration, ConfigurationKeyConstants.AlertsApiUrl,  errors);
            var alertsApiKey = ExtractValue(configuration, ConfigurationKeyConstants.AlertsApiKey,  errors);
            var storageConnectionString = ExtractValue(configuration, ConfigurationKeyConstants.AzureStorageConnectionString,  errors);


            var isDevelopmentEnv = false;
            if(bool.TryParse(configuration[ConfigurationKeyConstants.DevMode], out var parsed))
            {
                isDevelopmentEnv = parsed;
            }

            var hostUrl = "http://localhost:7030";
            if (!isDevelopmentEnv)
            {
                hostUrl = ExtractValue(configuration, ConfigurationKeyConstants.HostUrlKey, errors);
            }

            if (errors.Any())
            {
                var message = string.Join("\r\n", errors);
                throw new ArgumentException(message);
            }

            var telegramSettings = new TelegramSettings(telegramApiKey);
            var alertsApiSettings = new AlertsApiSettings(alertsApiUrl, alertsApiKey);
            var azureStorageSettings = new AzureStorageSettings(storageConnectionString);

            var settings = new SilentAppConfiguration(telegramSettings, alertsApiSettings, azureStorageSettings, hostUrl, isDevelopmentEnv);

            return settings;
        }

        private static string ExtractValue(IConfiguration configuration, string key, ICollection<string> errors)
        {
            var telegramApiKey = configuration[key];
            if (string.IsNullOrWhiteSpace(telegramApiKey))
            {
                errors.Add($"Absent configuration value: {key}");
            }

            return telegramApiKey;
        }
    }
}
