namespace SilentApp.Services.Contracts
{
    public interface IQueryRunner<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<RequestResult<TResult>> HandleAsync(TQuery query);
    }
}
