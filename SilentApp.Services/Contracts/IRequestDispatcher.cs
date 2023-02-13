namespace SilentApp.Services.Contracts
{
    public interface IRequestDispatcher
    {
        Task<RequestResult> DispatchCommand<TCommand>(TCommand command) where TCommand : ICommand;

        Task<RequestResult<TResult>> DispatchQuery<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
    }
}
