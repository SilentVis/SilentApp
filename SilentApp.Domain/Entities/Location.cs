using SilentApp.Domain.Enums;
#pragma warning disable CS8618

namespace SilentApp.Domain.Entities
{
    public class Location : BaseTableEntity
    {
        public const string EntityPartitionKey = "Locations";

        public string Name { get; set; }

        public LocationType Type { get; set; }

        public string? Region { get; set; }

        public string? RegionId { get; set; }
    }
}
