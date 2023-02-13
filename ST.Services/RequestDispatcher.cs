using System.Threading.Tasks;
using SilentApp.Services.Contracts;
using SimpleInjector;

namespace SilentApp.FunctionApp
{
    public class RequestDispatcher : IRequestDispatcher
    {
        private readonly Container _container;

        public RequestDispatcher(Startup.Completion completor, Container container)
        {
            _container = container;
        }

        public Task<RequestResult> DispatchCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = _container.GetInstance<ICommandHandler<TCommand>>();

            return handler.HandleAsync(command);
        }

        public Task<RequestResult<TResult>> DispatchQuery<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            var runner = _container.GetInstance<IQueryRunner<TQuery, TResult>>();

            return runner.HandleAsync(query);
        }
    }
}
