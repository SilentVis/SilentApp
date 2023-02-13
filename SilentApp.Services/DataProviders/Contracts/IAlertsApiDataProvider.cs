using SilentApp.Domain.DTO.ZenApi;

namespace SilentApp.Services.DataProviders.Contracts
{
    public interface IAlertsApiDataProvider
    {
        Task<AlertsResponseDTO> GetActualAlertsState();
    }
}
