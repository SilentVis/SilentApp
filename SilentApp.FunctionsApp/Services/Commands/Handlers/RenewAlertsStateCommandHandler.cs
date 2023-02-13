using System.Linq;
using System.Threading.Tasks;
using SilentApp.Domain.DTO.ZenApi;
using SilentApp.Domain.Entities;
using SilentApp.Domain.Enums;
using SilentApp.Services.Contracts;
using SilentApp.Services.DataProviders.Contracts;
using Telegram.Bot.Types;
using Location = SilentApp.Domain.Entities.Location;

namespace SilentApp.FunctionsApp.Services.Commands.Handlers
{
    internal class RenewAlertsStateCommandHandler : ICommandHandler<RenewAlertsStateCommand>
    {
        private readonly IAlertsApiDataProvider _alertsApiDataProvider;
        private readonly IAzureStorageTableDataProvider _azureStorageTableDataProvider;

        public RenewAlertsStateCommandHandler(
            IAlertsApiDataProvider alertsApiDataProvider,
            IAzureStorageTableDataProvider azureStorageTableDataProvider)
        {
            _alertsApiDataProvider = alertsApiDataProvider;
            _azureStorageTableDataProvider = azureStorageTableDataProvider;
        }

        public async Task<RequestResult> HandleAsync(RenewAlertsStateCommand command)
        {
            var currentAlertsResponse = await _alertsApiDataProvider.GetActualAlertsState();

            if (!currentAlertsResponse.IsSuccessful)
            {
                var error = new Error(
                    ErrorType.DataProviderError, 
                    "ERROR_GETTING_ALERTS_STATE",
                    $"$Unable to get alerts state because of \"{currentAlertsResponse.Error.Message}\"");

                return new RequestResult(error);
            }

            await UpdateLocations(currentAlertsResponse.Alerts);
            await ActualizeAlerts(currentAlertsResponse.Alerts);

            return new RequestResult();
        }

        private async Task UpdateLocations(AlertDTO[] currentAlerts)
        {
            var locationIds = currentAlerts.Select(a => a.LocationUid.ToString()).ToArray();
            var alertedLocations = currentAlerts.Select(s => new Location()
            {
                Name = s.LocationTitle,
                Region = s.LocationRegion,
                Type = LocationType.Other,
                RowKey = s.LocationUid.ToString(),
                PartitionKey = Location.EntityPartitionKey
            });

            var locations = await _azureStorageTableDataProvider.GetRecords<Location>(Location.EntityPartitionKey, locationIds);
            var foundLocationIds = locations.Select(s => s.RowKey).ToList();

            var newLocationIds = locationIds.Where(id => !foundLocationIds.Contains(id)).ToList();
            var newLocations = alertedLocations.Where(l => newLocationIds.Contains(l.RowKey));

            foreach (var location in newLocations)
            {
                if (location.Type != LocationType.Region)
                {
                    continue;
                }

                await _azureStorageTableDataProvider.UpsertRecord(location);
            }
        }

        private async Task ActualizeAlerts(AlertDTO[] currentAlerts)
        {
            var currentAlertIds = currentAlerts.Select(a => a.FormattedId).ToList();

            var existingAlerts = await _azureStorageTableDataProvider.GetRecords<Alert>(Alert.EntityPartitionKey);
            var existingAlertIds = existingAlerts.Select(a => a.RowKey).ToList();

            var endedAlertIds = existingAlertIds.Except(currentAlertIds);
            var newAlertIds = currentAlertIds.Except(existingAlertIds);

            var alertsToDelete = existingAlerts.Where(a => endedAlertIds.Contains(a.RowKey));
            var alertsToSave = currentAlerts.Where(c => newAlertIds.Contains(c.FormattedId));

            foreach (var alert in alertsToDelete)
            {
                await _azureStorageTableDataProvider.DeleteRecord(Alert.EntityPartitionKey, alert.RowKey);
            }

            foreach (var alertDto in alertsToSave)
            {
                var alert = new Alert()
                {
                    RowKey = alertDto.FormattedId,
                    PartitionKey = Alert.EntityPartitionKey,
                    AlertType = AlertType.AirRaid,
                    LocationId = alertDto.LocationFormattedId
                };

                await _azureStorageTableDataProvider.UpsertRecord(alert);

                // Push an event 
            }
        }
    }
}
