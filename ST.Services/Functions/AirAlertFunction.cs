using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SilentApp.Services.Contracts;

namespace SilentApp.FunctionApp.Functions
{
    public class AirAlertFunction
    {
        private readonly IRequestDispatcher _dispatcher;

        public AirAlertFunction(IRequestDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [FunctionName("AirAlertFunction")]
        public void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
