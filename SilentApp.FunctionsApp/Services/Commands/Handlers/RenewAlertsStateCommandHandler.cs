using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SilentApp.Domain.DTO.Internal;
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

        private readonly IAlertsQueueDataProvider _alertsQueueDataProvider;

        public RenewAlertsStateCommandHandler(
            IAlertsApiDataProvider alertsApiDataProvider,
            IAzureStorageTableDataProvider azureStorageTableDataProvider,
            IAlertsQueueDataProvider alertsQueueDataProvider)
        {
            _alertsApiDataProvider = alertsApiDataProvider;
            _azureStorageTableDataProvider = azureStorageTableDataProvider;
            _alertsQueueDataProvider = alertsQueueDataProvider;
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
            var locationIds = currentAlerts.Select(a => a.LocationId).ToArray();
            var alertedLocations = currentAlerts.Select(s => new Location
            {
                Name = s.LocationTitle,
                Region = s.LocationRegion,
                Type = LocationTypeParser.Parse(s.LocationType),
                RowKey = s.LocationId,
                PartitionKey = Location.EntityPartitionKey
            });

            var locations = await _azureStorageTableDataProvider.GetRecords<Location>(Location.EntityPartitionKey);
            var foundLocationIds = locations.Select(s => s.RowKey).ToList();

            var newLocationIds = locationIds.Where(id => !foundLocationIds.Contains(id)).ToList();
            var newLocations = alertedLocations.Where(l => newLocationIds.Contains(l.RowKey));

            foreach (var location in newLocations)
            {
                if (location.Type != LocationType.Region)
                {
                    var region = await _azureStorageTableDataProvider.GetRecord<Location>(x => 
                        x.PartitionKey == Location.EntityPartitionKey
                        && x.Name == location.Region 
                        && x.Type == LocationType.Region);

                    location.RegionId = region.RowKey;
                }

                await _azureStorageTableDataProvider.UpsertRecord(location);
            }
        }

        private async Task ActualizeAlerts(AlertDTO[] currentAlerts)
        {
            var currentAlertIds = currentAlerts.Select(a => a.FormattedId).ToList();

            var existingAlerts = await _azureStorageTableDataProvider.GetRecords<Alert>(Alert.EntityPartitionKey);
            var existingAlertIds = existingAlerts.Select(a => a.RowKey).ToList();

            var endedAlertIds = existingAlertIds.Except(currentAlertIds).ToList();
            var newAlertIds = currentAlertIds.Except(existingAlertIds).ToList();

            var alertsToDelete = existingAlerts.Where(a => endedAlertIds.Contains(a.RowKey)).ToList();
            var alertsToSave = currentAlerts.Where(c => newAlertIds.Contains(c.FormattedId)).ToList();

            var messages = new List<AlertMessage>();

            foreach (var alert in alertsToDelete)
            {
                await _azureStorageTableDataProvider.DeleteRecord(Alert.EntityPartitionKey, alert.RowKey);

                messages.Add(new AlertMessage(AlertMessageType.EndAlert, alert.LocationId, alert.RowKey));
            }

            foreach (var alertDto in alertsToSave)
            {
                var alert = new Alert()
                {
                    RowKey = alertDto.FormattedId,
                    PartitionKey = Alert.EntityPartitionKey,
                    AlertType = AlertTypeParser.Parse(alertDto.AlertType),
                    LocationId = alertDto.LocationId
                };

                await _azureStorageTableDataProvider.UpsertRecord(alert);

                messages.Add(new AlertMessage(AlertMessageType.StartAlert, alert.LocationId, alert.RowKey));
            }

            foreach (var alertMessage in messages)
            {
                await _alertsQueueDataProvider.Send(alertMessage);
            }
        }
    }
}
