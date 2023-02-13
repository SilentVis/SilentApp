using System.Text.Json.Serialization;

namespace SilentApp.Domain.DTO.ZenApi
{
    public class AlertDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("location_title")]
        public string LocationTitle { get; set; }

        [JsonPropertyName("location_type")]
        public string LocationType { get; set; }

        [JsonPropertyName("location_oblast")]
        public string LocationRegion { get; set; }

        [JsonPropertyName("location_uid")]
        public int LocationUid { get; set; }

        [JsonPropertyName("notes")]
        public string Notes { get; set; }

        [JsonPropertyName("started_at")]
        public DateTimeOffset StartedAt { get; set; }

        [JsonIgnore]
        public string FormattedId => Id.ToString();

        public string LocationFormattedId => LocationUid.ToString();
    }
}
