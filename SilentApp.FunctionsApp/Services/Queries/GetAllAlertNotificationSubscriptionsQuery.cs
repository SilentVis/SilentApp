using System.Collections.Generic;
using SilentApp.Domain.Entities;
using SilentApp.Services.Contracts;

namespace SilentApp.FunctionsApp.Services.Queries
{
    public class GetAllAlertNotificationSubscriptionsQuery : IQuery<IEnumerable<AlertNotificationSubscription>>
    {
        public GetAllAlertNotificationSubscriptionsQuery()
        {
        }
    }
}