using SharedLogic.Models.Cells;
using SharedLogic.Models.Enums;
using SharedLogic.Respiration.Services;
using SharedLogic.Respiration.Services.Interfaces;

namespace SharedLogic.Respiration.Factories
{
    public class RespirationProcessorFactory<C> : IRespirationProcessorFactory<C> where C : Cell
    {
        public IRespirationProcessor<C> GetServiceForRespiration(C cell)
        {
            var respirationService = GetRespirationService(cell);
            return respirationService;
        }

        private IRespirationProcessor<C> GetRespirationService(C cell)
        {
            if (CanDoAerobicGlucoseRespiration(cell))
            {
                Console.WriteLine("Aerobic glucose metabolism selected.");
                return new GlucoseRespirationProcessor<C>(cell);
            }

            if (CanDoLipidRespiration(cell))
            {
                Console.WriteLine("Aerobic glucose metabolism selected.");
                return new LipidRespirationProcessor<C>(cell);
            }

            throw new InvalidOperationException("Unsupported respiration type.");
        }

        private bool CanDoAerobicGlucoseRespiration(C cell)
            => cell.NutrientConcentrations.GlucoseCount > 0 
            && cell.NutrientConcentrations.OxygenCount > 0
            && cell.Enzymes.Any(o => o.EnzymeType == EnzymeType.ATPSynthase);

        private bool CanDoLipidRespiration(C cell)
            => cell.NutrientConcentrations.FattyAcidsCount > 0 
            && cell.Enzymes.Any(o => o.EnzymeType == EnzymeType.ATPSynthase)
            && cell.Enzymes.Any(o => o.EnzymeType == EnzymeType.CPT_I);
    }
}