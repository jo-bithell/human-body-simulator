using SharedLogic.Models.Cells;

namespace SharedLogic.Respiration
{
    internal class RespirationService
    {
        private Cell _cell;

        internal RespirationService(Cell cell)
        {
            _cell = cell;
        }

        internal void PerformRespiration()
        {
            GetProcessorForRespiration().Process();
            Console.WriteLine("Performed respiration.");
        }

        private BaseRespiration GetProcessorForRespiration()
        {
            if (_cell.GlucoseCount > 0 && _cell.OxygenCount > 0)
            {
                Console.WriteLine("Aerobic glucose metabolism selected.");
                return new AerobicGlucoseMetabolism(_cell);
            }

            if (_cell.FattyAcidsCount > 0)
            {
                return new FattyAcidMetabolism();
            }

            if (_cell.AminoAcidsCount > 0)
            {
                return new AminoAcidMetabolism();
            }

            throw new InvalidOperationException("Unsupported respiration type.");
        }
    }
}
