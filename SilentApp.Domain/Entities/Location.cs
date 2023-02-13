using SilentApp.Domain.Enums;

namespace SilentApp.Domain.Entities
{
    public class Location : BaseStorageEntity
    {
        public const string EntityPartitionKey = "Locations";

        public string Name { get; set; }

        public LocationType Type { get; set; }

        public string? Region { get; set; }

        public string? RegionId { get; set; }
    }
}
