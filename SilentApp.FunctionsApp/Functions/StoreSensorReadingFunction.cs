using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using SilentApp.FunctionsApp.Dto;
using SilentApp.FunctionsApp.Services.Commands;
using SilentApp.Services.Contracts;

namespace SilentApp.FunctionsApp.Functions
{
    public class StoreSensorReadingFunction : FunctionBase
    {
        public StoreSensorReadingFunction(IRequestDispatcher requestDispatcher) : base(requestDispatcher) { }

        [FunctionName("StoreSensorReading")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
        {
            // Parse the request body
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var sensorReading = JsonSerializer.Deserialize<SensorReading>(requestBody);

            // Create the StoreSensorReading command and dispatch it
            var command = new StoreSensorReadingCommand(
                sensorReading.DeviceName, 
                DateTimeOffset.UtcNow, 
                sensorReading.Temperature, 
                sensorReading.Humidity);

            var result = await _requestDispatcher.DispatchCommand(command);

            // Check the result and return an appropriate response
            if (result.IsSuccessful)
            {
                return new OkObjectResult(sensorReading);
            }
            else
            {
                return new BadRequestObjectResult(result.Error);
            }
        }
    }
}