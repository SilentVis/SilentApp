#pragma warning disable CS8618
namespace SilentApp.Domain.Entities
{
    public class TemperatureMeasurement : BaseTableEntity
    {
        public const string EntityPartitionKey = "TemperatureMeasurements";

        public string DeviceName { get; set; }

        public DateTimeOffset Date { get; set; }

        public decimal Temperature { get; set; }

        public decimal Humidity { get; set; }
    }

}
