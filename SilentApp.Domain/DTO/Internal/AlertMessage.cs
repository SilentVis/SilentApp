using SilentApp.Domain.Enums;

namespace SilentApp.Domain.DTO.Internal
{
    public class AlertMessage
    {
        public AlertMessage(AlertMessageType type, string locationId)
        {
            Type = type;
            LocationId = locationId;
        }

        public AlertMessageType Type { get; }

        public string LocationId { get; }
    }
}
