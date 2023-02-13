using System.Text.Json.Serialization;

namespace SilentApp.Domain.DTO.ZenApi
{
    public class ErrorDTO
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
