using SharedLogic.Models.Cells;
using SharedLogic.Respiration.Services.Interfaces;

namespace SharedLogic.Respiration.Factories
{
    public interface IRespirationProcessorFactory<C> where C : Cell
    {
        IRespirationProcessor<C> GetServiceForRespiration(C cell);
    }
}