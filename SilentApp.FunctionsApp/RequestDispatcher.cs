using System.Threading.Tasks;
using SilentApp.Services.Contracts;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace SilentApp.FunctionsApp
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
            using (var scope = AsyncScopedLifestyle.BeginScope(_container))
            {

                var handler = _container.GetInstance<ICommandHandler<TCommand>>();

                return handler.HandleAsync(command);
            }
        }

        public Task<RequestResult<TResult>> DispatchQuery<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            using (var scope = AsyncScopedLifestyle.BeginScope(_container))
            {
                var runner = _container.GetInstance<IQueryRunner<TQuery, TResult>>();

                return runner.HandleAsync(query);
            }
        }
    }
}
