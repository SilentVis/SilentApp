namespace SilentApp.Domain.Enums
{
    public enum LocationType
    {
        Region = 10,
        District = 20,
        Subdistrict = 30,
        City = 40,
        
        Other = -1
    }

    public class LocationTypeParser
    {
        public static LocationType Parse(string value)
        {
            switch (value.Trim().ToLower())
            {
                case "oblast": return LocationType.Region;
                case "raion": return LocationType.District;
                case "hromada": return LocationType.Subdistrict;
                case "city": return LocationType.City;
                case "unknown": return LocationType.Other;
            }

            return LocationType.Other;
        }
    }
}
