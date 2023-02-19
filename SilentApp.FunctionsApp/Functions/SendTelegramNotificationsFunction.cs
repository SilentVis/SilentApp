using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SilentApp.Domain.DTO.Internal;
using SilentApp.FunctionsApp.Services.Commands;
using SilentApp.Infrastructure.Constants;
using SilentApp.Services.Contracts;

namespace SilentApp.FunctionsApp.Functions
{
    public class SendTelegramNotificationsFunction : FunctionBase
    {
        public SendTelegramNotificationsFunction(IRequestDispatcher requestDispatcher) : base(requestDispatcher)
        {
        }

        [FunctionName("SendTelegramNotifications")]
        public async Task Run([QueueTrigger(AppConstants.AlertsQueueName, Connection = ConfigurationKeyConstants.AzureStorageConnectionString)] string myQueueItem, ILogger log)
        {
            var alertMessage = JsonSerializer.Deserialize<AlertMessage>(myQueueItem);
            var command = new SendTelegramNotificationsCommand(alertMessage.LocationId, alertMessage.Type);

            var result = await _requestDispatcher.DispatchCommand(command);

            if (!result.IsSuccessful)
            {
                LogError(log, nameof(SendTelegramNotificationsFunction), result.Error);
            }
        }
    }
}
