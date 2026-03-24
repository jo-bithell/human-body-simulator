using SharedLogic.Models.Cells;

namespace SharedLogic.Respiration.Services
{
    public interface IRespirationTypeSelectorService<C> where C : Cell
    {
        bool CanDoAerobicGlucoseRespiration(C cell);

        bool CanDoLipidRespiration(C cell);
    }
}
