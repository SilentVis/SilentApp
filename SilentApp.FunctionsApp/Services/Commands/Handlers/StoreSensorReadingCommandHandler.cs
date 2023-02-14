using System;
using System.Threading.Tasks;
using SilentApp.Domain.Entities;
using SilentApp.Services.Contracts;
using SilentApp.Services.DataProviders.Contracts;

namespace SilentApp.FunctionsApp.Services.Commands
{
    public class StoreSensorReadingCommandHandler : ICommandHandler<StoreSensorReadingCommand>
    {
        private readonly IAzureStorageTableDataProvider _tableDataProvider;

        public StoreSensorReadingCommandHandler(IAzureStorageTableDataProvider tableDataProvider)
        {
            _tableDataProvider = tableDataProvider;
        }

        public async Task<RequestResult> HandleAsync(StoreSensorReadingCommand command)
        {
            try
            {
                var measurement = new TemperatureMeasurement
                {
                    RowKey = command.DeviceName + "_" + DateTime.UtcNow.Ticks,
                    Temperature = command.Temperature,
                    Humidity = command.Humidity,
                    DeviceName = command.DeviceName,
                    Timestamp = DateTimeOffset.UtcNow,
                    PartitionKey = TemperatureMeasurement.EntityPartitionKey
                };

                await _tableDataProvider.UpsertRecord(measurement);

                return new RequestResult();
            }
            catch (Exception ex)
            {
                var error = new Error(ErrorType.InternalError, "StoreSensorReadingCommandHandler", ex.Message);
                return new RequestResult(error);
            }
        }
    }
}