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
        private Enzyme? _cpt_ISynthase;
        internal LipidRespirationProcessor(C cell)
        {
            _cell = cell;
            SetATPSynthase(cell);
            SetCPT_ISynthase(cell);
        }

        public void Process()
        {
            if (_cpt_ISynthase == null)
                return;

            ActivateFattyAcids();
            PerformBetaOxidization();
            PerformKrebsCycle();
            PerformElectronTransportChain();
        }

        private void SetATPSynthase(C cell)
        {
            var atpSynthase = cell.Enzymes.Where(o => o.EnzymeType == EnzymeType.ATPSynthase).FirstOrDefault(o => !o.InUse);
            if (atpSynthase != null)
            {
                _atpSynthase = atpSynthase;
                _atpSynthase.InUse = true;
            }
        }

        private void SetCPT_ISynthase(C cell)
        {
            var cpt_ISynthase = cell.Enzymes.Where(o => o.EnzymeType == EnzymeType.CPT_I).FirstOrDefault(o => !o.InUse);
            if (cpt_ISynthase != null)
            {
                _cpt_ISynthase = cpt_ISynthase;
                _cpt_ISynthase.InUse = true;
            }
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
