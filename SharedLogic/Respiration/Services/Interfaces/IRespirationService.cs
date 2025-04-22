using SharedLogic.Models.Cells;

namespace SharedLogic.Respiration.Services.Interfaces
{
    public interface IRespirationService<C> where C : Cell
    {
        Task Process();
    }
}