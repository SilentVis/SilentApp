using Microsoft.Extensions.Logging;
using SilentApp.Services.Contracts;

namespace SilentApp.Services.Decorators
{
    public abstract class LoggingDecoratorBase
    {
        protected readonly ILogger _logger;

        protected LoggingDecoratorBase(ILogger logger)
        {
            _logger = logger;
        }

        protected void LogError(string requestName, Error error) 
        {
            _logger.LogError(@"An error occurred during {requestName} execution: 
Error type: {type}
Error code: {code}
Error message: {message}",
                requestName, error.Type.ToString(), error.Code, error.Message
            );
        }

        protected void LogException(string requestName, Exception ex) 
        {
            _logger.LogError(@"Exception of type {exceptionType} thrown during {requestName}: 
Message: {message}
Stack trace: {stackTrace}",
                ex.GetType().Name, requestName, ex.Message, ex.StackTrace
            );
        }
    }
}
