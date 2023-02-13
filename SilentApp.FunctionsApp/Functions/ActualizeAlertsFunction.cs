using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SilentApp.FunctionsApp.Services.Commands;
using SilentApp.Services.Contracts;

namespace SilentApp.FunctionsApp.Functions
{
    public class ActualizeAlertsFunction : FunctionBase
    {
        private readonly IRequestDispatcher _requestDispatcher;

        public ActualizeAlertsFunction(IRequestDispatcher requestDispatcher)
        {
            _requestDispatcher = requestDispatcher;
        }

        [FunctionName("RenewAlerts")]
        public async Task Run([TimerTrigger("*/30 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            var command = new RenewAlertsStateCommand();
            var result = await _requestDispatcher.DispatchCommand(command);

            if (!result.IsSuccessful)
            {
                LogError(log, nameof(ActualizeAlertsFunction), result.Error);
            }
        }
    }
}
