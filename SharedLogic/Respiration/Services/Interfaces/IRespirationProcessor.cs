using SharedLogic.Models.Cells;

namespace SharedLogic.Respiration.Services.Interfaces
{
    public interface IRespirationProcessor<C> where C : Cell
    {
        void Process();
    }
}
