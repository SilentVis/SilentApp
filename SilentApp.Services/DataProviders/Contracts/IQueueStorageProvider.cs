using SilentApp.Domain.DTO.Internal;

namespace SilentApp.Services.DataProviders.Contracts
{
    public interface IAlertsQueueDataProvider
    {
        Task Send(AlertMessage message);
    }
}
