using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SilentApp.Services.Contracts;
using System.Text.Json;
using System.Threading.Tasks;
using SilentApp.FunctionsApp.Dto;
using SilentApp.FunctionsApp.Services.Commands;

namespace SilentApp.FunctionsApp.Functions
{
    public class RegisterSubscriptionFunction : FunctionBase
    {
        public RegisterSubscriptionFunction(IRequestDispatcher requestDispatcher) : base(requestDispatcher)
        {
        }

        [FunctionName("RegisterSubscription")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("RegisterSubscription processed a request.");

            // Parse the input data
            var requestBody = await req.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<SubscriptionRequestDto>(requestBody);

            // Create the command
            var command = new RegisterSubscriptionCommand(data.ChatId, data.LocationId);

            // Dispatch the command and handle the result
            var result = await _requestDispatcher.DispatchCommand(command);
            if (result.IsSuccessful)
            {
                return new OkResult();
            }
            else
            {
                return new BadRequestObjectResult(result.Error.Message);
            }
        }
    }
}
