using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using SilentApp.Domain.DTO.ZenApi;
using SilentApp.Infrastructure;
using SilentApp.Services.DataProviders.Contracts;

namespace SilentApp.Services.DataProviders
{
    internal class AlertsApiDataProvider : IAlertsApiDataProvider
    {
        private readonly string _alertsApiKey;
        private readonly string _alertsApiUrl;

        public AlertsApiDataProvider(IConfiguration _configuration)
        {
            _alertsApiKey = _configuration[ConfigurationKeyConstants.AlertsApiKey];
            _alertsApiUrl = _configuration[ConfigurationKeyConstants.AlertsApiUrl];
        }

        public async Task<AlertsResponseDTO> GetActualAlertsState()
        {
            using var client = new HttpClient();

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, _alertsApiUrl);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _alertsApiKey);

            var response = await client.SendAsync(requestMessage);

            var responseRawData = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<AlertsResponseDTO>(responseRawData);

            return responseData;
        }
    }
}
