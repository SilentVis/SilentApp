using System.Net.Http.Headers;
using System.Text.Json;
using SilentApp.Domain.DTO.ZenApi;
using SilentApp.Infrastructure.Configuration;
using SilentApp.Services.DataProviders.Contracts;

namespace SilentApp.Services.DataProviders
{
    internal class AlertsApiDataProvider : IAlertsApiDataProvider
    {
        private readonly string _alertsApiKey;
        private readonly string _alertsApiUrl;

        public AlertsApiDataProvider(SilentAppConfiguration _configuration)
        {
            _alertsApiKey = _configuration.AlertsApi.ApiKey;
            _alertsApiUrl = _configuration.AlertsApi.Url;
        }

        public async Task<AlertsResponseDTO> GetActualAlertsState()
        {
            using var client = new HttpClient();

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, _alertsApiUrl);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _alertsApiKey);

            var response = await client.SendAsync(requestMessage);

            var responseRawData = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<AlertsResponseDTO>(responseRawData);

            if (responseData == null)
            {
                throw new ArgumentException("Unable to retrieve data from alerts API - null response returned");
            }

            return responseData;
        }
    }
}
