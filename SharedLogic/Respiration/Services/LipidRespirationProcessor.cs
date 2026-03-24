using SharedLogic.Models.Cells;
using SharedLogic.Models.Enums;
using SharedLogic.Models.Enzymes;
using SharedLogic.Respiration.Services.Interfaces;

namespace SharedLogic.Respiration.Services
{
    internal class LipidRespirationProcessor<C> : IRespirationProcessor<C> where C : Cell
    {
        private Cell _cell;
        private Enzyme? _atpSynthase;
        private Enzyme? _lipase;
        internal LipidRespirationProcessor(C cell)
        {
            _cell = cell;
            _atpSynthase = cell.GetAvailableEnzyme(EnzymeType.ATPSynthase);
            _lipase = cell.GetAvailableEnzyme(EnzymeType.Lipase);
        }

        public void Process()
        {
            if (_lipase == null)
                return;

            ActivateFattyAcids();
            PerformBetaOxidization();
            PerformKrebsCycle();
            PerformElectronTransportChain();
        }

        private void ActivateFattyAcids()
        {
            _cell.NutrientConcentrations.ATPCount =- 2;
        }

        private void PerformBetaOxidization()
        {
            // In mitochrondria
            // Produces 2 carbons
            // Shortens fatty acids
        }

        private void PerformKrebsCycle()
        {
            _cell.NutrientConcentrations.ATPCount += 28;
        }

        private void PerformElectronTransportChain()
        {
            if (_atpSynthase == null)
                return;

            _cell.NutrientConcentrations.ATPCount += 80;
            _cell.NutrientConcentrations.FattyAcidsCount -= 1;
        }
    }
}
