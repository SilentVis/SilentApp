using System;
using SilentApp.Services.Contracts;

namespace SilentApp.FunctionsApp.Services.Commands
{
    public class StoreSensorReadingCommand : ICommand
    {
        public StoreSensorReadingCommand(string deviceName, DateTimeOffset date, decimal temperature, decimal humidity)
        {
            DeviceName = deviceName;
            Date = date;
            Temperature = temperature;
            Humidity = humidity;
        }

        public string DeviceName { get; }
        public DateTimeOffset Date { get; }
        public decimal Temperature { get; }
        public decimal Humidity { get; }
    }
}
