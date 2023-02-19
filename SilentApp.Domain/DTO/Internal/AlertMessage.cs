using SilentApp.Domain.Enums;

namespace SilentApp.Domain.DTO.Internal
{
    public class AlertMessage : BaseQueueMessage
    {
        public AlertMessage(AlertMessageType type, string locationId, string alertId)
        {
            Type = type;
            LocationId = locationId;
            AlertId = alertId;
        }

        public AlertMessageType Type { get; }

        public string LocationId { get; }

        public string AlertId { get; }
    }
}
