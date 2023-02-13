namespace SilentApp.Domain.Enums
{
    public enum AlertType
    {
        AirRaid = 1,
        ArtilleryShelling = 2,
        UrbanFights = 3,
        Chemical = 4,
        Nuclear = 5,

        Unknown = -1
    }

    public class AlertTypeParser
    {
        public static AlertType Parse(string value)
        {
            switch (value.Trim().ToLower())
            {
                case "air_raid": return AlertType.AirRaid;
                case "artillery_shelling": return AlertType.ArtilleryShelling;
                case "urban_fights": return AlertType.UrbanFights;
                case "chemical": return AlertType.Chemical;
                case "nuclear": return AlertType.Nuclear;
            }

            return AlertType.Unknown;
        }
    }
}
