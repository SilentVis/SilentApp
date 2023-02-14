using System;

namespace SilentApp.FunctionsApp.Dto
{
    public class SensorReading
    {
        public string DeviceName { get; set; }
        public decimal Temperature { get; set; }
        public decimal Humidity { get; set; }
    }
}
