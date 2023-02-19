using Microsoft.Extensions.Logging;
using SilentApp.Services.Contracts;

namespace SilentApp.Services.Decorators
{
    public class QueryRunnerLoggingDecorator<T,R>:LoggingDecoratorBase, IQueryRunner<T,R> where T: IQuery<R>
    {
        private readonly IQueryRunner<T, R> _decoratee;

        public QueryRunnerLoggingDecorator(ILogger logger, IQueryRunner<T, R> decoratee) : base(logger)
        {
            _decoratee = decoratee;
        }

        public async Task<RequestResult<R>> HandleAsync(T query)
        {
            var requestName = typeof(T).Name;

            _logger.LogDebug("Executing {requestName} query", requestName);
            try
            {
                var result = await _decoratee.HandleAsync(query);

                if (!result.IsSuccessful)
                {
                    LogError(requestName, result.Error!);
                }

                return result;
            }
            catch (Exception e)
            {
                LogException(requestName, e);

                return new RequestResult<R>(new Error(ErrorType.InternalError, "Exception", e.Message));
            }
        }
    }
}
