using System.Text.Json.Serialization;

namespace SilentApp.Domain.DTO.ZenApi
{
    public class AlertsResponseDTO
    {
        [JsonPropertyName("alerts")]
        public AlertDTO[]? Alerts { get; set; }

        [JsonPropertyName("error")]
        public ErrorDTO? Error { get; set; }

        public bool IsSuccessful => Error == null;
    }
}
