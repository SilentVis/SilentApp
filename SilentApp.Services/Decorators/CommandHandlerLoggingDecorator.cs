using Microsoft.Extensions.Logging;
using SilentApp.Services.Contracts;

namespace SilentApp.Services.Decorators
{
    public class CommandHandlerLoggingDecorator<T> : LoggingDecoratorBase, ICommandHandler<T> where T: ICommand
    {
        private readonly ICommandHandler<T> _decoratee;

        public CommandHandlerLoggingDecorator(ILogger logger, ICommandHandler<T> decoratee) : base(logger)
        {
            _decoratee = decoratee;
        }

        public async Task<RequestResult> HandleAsync(T command)
        {
            var requestName = typeof(T).Name;

            _logger.LogDebug("Executing {requestName} command", requestName);
            try
            {
                var result = await _decoratee.HandleAsync(command);

                if (!result.IsSuccessful)
                {
                    LogError(requestName, result.Error!);
                }

                return result;
            }
            catch (Exception e)
            {
                LogException(requestName, e);

                return new RequestResult(new Error(ErrorType.InternalError, "Exception", e.Message));
            }
        }
    }
}
