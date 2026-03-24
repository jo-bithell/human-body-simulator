using SharedLogic.Models.Cells;
using SharedLogic.Models.Enums;

namespace SharedLogic.Respiration.Services
{
    public class RespirationTypeSelectorService<C> : IRespirationTypeSelectorService<C> where C : Cell
    {
        public bool CanDoAerobicGlucoseRespiration(C cell)
            => cell.NutrientConcentrations.GlucoseCount > 0
            && cell.NutrientConcentrations.OxygenCount > 0
            && cell.Enzymes.Any(o => o.EnzymeType == EnzymeType.ATPSynthase);

        public bool CanDoLipidRespiration(C cell)
            => cell.NutrientConcentrations.FattyAcidsCount > 0
            && cell.Enzymes.Any(o => o.EnzymeType == EnzymeType.ATPSynthase)
            && cell.Enzymes.Any(o => o.EnzymeType == EnzymeType.Lipase);
    }
}
