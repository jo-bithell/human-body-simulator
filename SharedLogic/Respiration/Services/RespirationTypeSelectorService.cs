using SharedLogic.Models.Cells;
using SharedLogic.Models.Enums;

namespace SharedLogic.Respiration.Services
{
    public class RespirationTypeSelectorService<C> : IRespirationTypeSelectorService<C> where C : Cell
    {
        private readonly Cell _cell;
        public RespirationTypeSelectorService(Cell cell) 
        {
            _cell = cell;
        }

        public bool CanDoAerobicGlucoseRespiration()
            => _cell.NutrientConcentrations.GlucoseCount >= 1
            && _cell.NutrientConcentrations.OxygenCount >= 6
            && _cell.Enzymes.Any(o => o.EnzymeType == EnzymeType.ATPSynthase);

        public bool CanDoLipidMetabolism()
            => _cell.NutrientConcentrations.FattyAcidsCount >= 1
            && _cell.Enzymes.Any(o => o.EnzymeType == EnzymeType.ATPSynthase)
            && _cell.Enzymes.Any(o => o.EnzymeType == EnzymeType.Lipase);
    }
}
