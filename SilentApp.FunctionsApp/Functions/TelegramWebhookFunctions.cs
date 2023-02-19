using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SilentApp.Services.Contracts;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using SilentApp.FunctionsApp.Services.Commands;
using Telegram.Bot.Types;

namespace SilentApp.FunctionsApp.Functions
{
    public class TelegramWebhookFunction : FunctionBase
    {
        public TelegramWebhookFunction(IRequestDispatcher requestDispatcher) : base(requestDispatcher)
        {
        }

        [FunctionName("TelegramWebhook")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "telegram/webhook")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("TelegramWebhook function received a request.");

            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var update = JsonSerializer.Deserialize<Update>(requestBody, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var command = new ProcessTelegramUpdateCommand(update);
                var result = await _requestDispatcher.DispatchCommand(command);

                if (!result.IsSuccessful)
                {
                    LogError(log, nameof(TelegramWebhookFunction), result.Error);
                }

                log.LogInformation("Telegram update processed successfully.");
                return new OkResult();
            }
            catch (JsonException ex)
            {
                LogError(log, nameof(TelegramWebhookFunction),
                    new Error(ErrorType.Other, "INVALID_JSON", $"Unable to deserialize Telegram update JSON. Error: {ex.Message}"));
                return new BadRequestResult();
            }
        }
    }
}
