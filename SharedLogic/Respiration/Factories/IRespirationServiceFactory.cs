using SharedLogic.Respiration.Services.Interfaces;

namespace SharedLogic.Respiration.Factories
{
    public interface IRespirationServiceFactory
    {
        Task<IRespirationService> GetServiceForRespiration();
    }
}