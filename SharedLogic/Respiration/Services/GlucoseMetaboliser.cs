using SharedLogic.Models.Cells;
using SharedLogic.Respiration.Services.Interfaces;

namespace SharedLogic.Respiration.Services
{
    internal class AerobicGlucoseMetabolism : IBaseRespirationService
    {
        private Cell _cell;
        internal AerobicGlucoseMetabolism(Cell cell)
        {
            _cell = cell;
        }

        internal void Process()
        {
            PerformGlycolysis();
            PerformKrebsCycle();
            PerformElectronTransportChain();
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
