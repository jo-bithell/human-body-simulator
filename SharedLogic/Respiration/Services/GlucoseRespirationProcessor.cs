using SharedLogic.Models.Cells;
using SharedLogic.Models.Enums;
using SharedLogic.Models.Enzymes;
using SharedLogic.Respiration.Services.Interfaces;

namespace SharedLogic.Respiration.Services
{
    public class GlucoseRespirationProcessor<C> : IRespirationProcessor<C> where C : Cell
    {
        private Cell _cell;
        private Enzyme? _atpSynthase;
        public GlucoseRespirationProcessor(C cell)
        {
            _cell = cell;
            SetATPSynthase(cell);
        }

        public void Process()
        {
            PerformGlycolysis();
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

        //Cytoplasm
        private void PerformGlycolysis()
        {
            ConvertGlucoseToPyruvate();
        }

        private void ConvertGlucoseToPyruvate()
        {
            _cell.NutrientConcentrations.GlucoseCount -= 1;
        }

        // Mitochondria
        private void PerformKrebsCycle()
        {
            BreakDownPyruvateIntoAceytlCoA();
            BreakDownAcetylCoA();
        }

        private void BreakDownAcetylCoA()
        {
            _cell.NutrientConcentrations.ATPCount += 2;
            _cell.NutrientConcentrations.CarbonDioxideCount += 4;
        }

        private void BreakDownPyruvateIntoAceytlCoA()
        {
            _cell.NutrientConcentrations.ATPCount -= 2;
            _cell.NutrientConcentrations.CarbonDioxideCount += 2;
        }

        private void PerformElectronTransportChain()
        {
            if (_atpSynthase != null)
            {
                _atpSynthase.PerformAction();
                _cell.NutrientConcentrations.OxygenCount -= 6;
                _cell.NutrientConcentrations.WaterCount += 1;
                _cell.NutrientConcentrations.ATPCount += 34;
                _atpSynthase.InUse = false;
            }
        }
    }
}
