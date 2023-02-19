using System.Text.Json.Serialization;

namespace SilentApp.Domain.DTO.ZenApi
{
    public class ErrorDTO
    {
        [JsonPropertyName("message")]
#pragma warning disable CS8618
        public string Message { get; set; }
#pragma warning restore CS8618
    }
}
