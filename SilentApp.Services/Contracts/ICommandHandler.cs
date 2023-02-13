namespace SilentApp.Services.Contracts
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task<RequestResult> HandleAsync(TCommand command);
    }
}
