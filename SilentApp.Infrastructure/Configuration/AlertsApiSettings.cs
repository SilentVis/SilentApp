namespace SilentApp.Infrastructure.Configuration;

public struct AlertsApiSettings
{
    public AlertsApiSettings(string url, string apiKey)
    {
        Url = url;
        ApiKey = apiKey;
    }

    public string Url { get; }

    public string ApiKey { get; }
}