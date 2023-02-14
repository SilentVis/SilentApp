using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SilentApp.Domain.Entities;
using SilentApp.FunctionsApp.Services.Queries;
using SilentApp.Services.Contracts;

namespace SilentApp.FunctionsApp.Functions
{
    public class LoadSubscriptionsFunction : FunctionBase
    {
        public LoadSubscriptionsFunction(IRequestDispatcher requestDispatcher)
            : base(requestDispatcher)
        {
        }

        [FunctionName("LoadSubscriptions")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "subscriptions")] HttpRequest req,
            ILogger log)
        {
            var query = new GetAllAlertNotificationSubscriptionsQuery();
            var result = await _requestDispatcher.DispatchQuery<GetAllAlertNotificationSubscriptionsQuery, IEnumerable<AlertNotificationSubscription>>(query);

            if (!result.IsSuccessful)
            {
                LogError(log, nameof(LoadSubscriptionsFunction), result.Error);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return new OkObjectResult(result.Data);
        }
    }
}