using SilentApp.Domain.DTO.Internal;

namespace SilentApp.Services.DataProviders.Contracts
{
    public interface IAzureQueueDataProvider
    {
        Task Send<T>(T message) where T : BaseQueueMessage;
    }
}
