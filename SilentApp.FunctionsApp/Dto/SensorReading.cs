using System.Text.Json.Serialization;

namespace SilentApp.FunctionsApp.Dto
{
    public class SensorReading
    {
        [JsonPropertyName("deviceName")]
        public string DeviceName { get; set; }

        [JsonPropertyName("temperature")]
        public decimal Temperature { get; set; }

        [JsonPropertyName("humidity")]
        public decimal Humidity { get; set; }
    }
}
