namespace SilentApp.Infrastructure.Configuration;

public struct TelegramSettings
{
    public TelegramSettings(string apiKey)
    {
        ApiKey = apiKey;
    }

    public string ApiKey { get; }
}