using SilentApp.Domain.DTO.Internal;
using SilentApp.Domain.Entities;
using SilentApp.Infrastructure.Constants;
using SilentApp.Services.DataProviders.Contracts;

namespace SilentApp.Services.DataProviders;

public class StorageNameIndex : IStorageNameIndex
{
    private readonly IDictionary<string,string> _tableNames;
    private readonly IDictionary<string, string> _queueNames;

    public StorageNameIndex()
    {
        _tableNames = new Dictionary<string, string>()
        {
            { nameof(Alert), AppConstants.AlertsTableName },
            { nameof(Location), AppConstants.AlertsTableName },
            { nameof(AlertNotificationSubscription), AppConstants.AlertsTableName },
            { nameof(TemperatureMeasurement), AppConstants.SensorReadingTableName },
        };

        _queueNames = new Dictionary<string, string>()
        {
            { nameof(AlertMessage), AppConstants.AlertsQueueName }
        };
    }
    
    public string GetTableName(string entityName)
    {
        return _tableNames.ContainsKey(entityName)
            ? _tableNames[entityName]
            : throw new ArgumentException("Unable to find a table name for entity {entityName}", entityName);
    }

    public string GetQueueName(string entityName)
    {
        return _queueNames.ContainsKey(entityName)
            ? _queueNames[entityName]
            : throw new ArgumentException("Unable to find a table name for entity {entityName}", entityName);
    }
}