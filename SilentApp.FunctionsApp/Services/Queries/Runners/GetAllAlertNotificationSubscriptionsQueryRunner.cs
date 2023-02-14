using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SilentApp.Domain.Entities;
using SilentApp.Services.Contracts;
using SilentApp.Services.DataProviders.Contracts;

namespace SilentApp.FunctionsApp.Services.Queries.Runners
{
    public class GetAllAlertNotificationSubscriptionsQueryRunner : IQueryRunner<GetAllAlertNotificationSubscriptionsQuery, IEnumerable<AlertNotificationSubscription>>
    {
        private readonly IAzureStorageTableDataProvider _tableDataProvider;

        public GetAllAlertNotificationSubscriptionsQueryRunner(IAzureStorageTableDataProvider tableDataProvider)
        {
            _tableDataProvider = tableDataProvider;
        }

        public async Task<RequestResult<IEnumerable<AlertNotificationSubscription>>> HandleAsync(GetAllAlertNotificationSubscriptionsQuery query)
        {
            try
            {
                var subscriptions = await _tableDataProvider.GetRecords<AlertNotificationSubscription>(AlertNotificationSubscription.EntityPartitionKey);

                return new RequestResult<IEnumerable<AlertNotificationSubscription>>(subscriptions);
            }
            catch (Exception ex)
            {
                var error = new Error(ErrorType.DataProviderError, "GetAllAlertNotificationSubscriptionsQueryRunner", ex.Message);
                return new RequestResult<IEnumerable<AlertNotificationSubscription>>(error);
            }
        }
    }
}