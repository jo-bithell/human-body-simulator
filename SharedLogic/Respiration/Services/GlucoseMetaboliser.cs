using SharedLogic.Models.Cells;
using SharedLogic.Respiration.Services.Interfaces;

namespace SharedLogic.Respiration.Services
{
    internal class AerobicGlucoseMetabolism : IRespirationService
    {
        private Cell _cell;
        internal AerobicGlucoseMetabolism(Cell cell)
        {
            _cell = cell;
        }

        public void Process()
        {
            PerformGlycolysis();
            PerformKrebsCycle();
            PerformElectronTransportChain();
            //Write back to cache here.
        }

        //Cytoplasm
        private void PerformGlycolysis()
        {
            ConvertGlucoseToPyruvate();
        }

        private void ConvertGlucoseToPyruvate()
        {
            var glucose = _cell.GlucoseCount -= 1;
        }

        // Mitochondria
        private void PerformKrebsCycle()
        {
            BreakDownPyruvateIntoAceytlCoA();
            BreakDownAcetylCoA();
        }

        private void BreakDownAcetylCoA()
        {
            _cell.ATPCount += 2;
            _cell.CarbonDioxideCount += 4;
        }

        private void BreakDownPyruvateIntoAceytlCoA()
        {
            _cell.ATPCount -= 2;
            _cell.CarbonDioxideCount += 2;
        }

        private void PerformElectronTransportChain()
        {
            _cell.WaterCount += 1;
            _cell.ATPCount += 34;
        }
    }
}
