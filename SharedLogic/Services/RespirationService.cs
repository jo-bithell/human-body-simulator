using SharedLogic.Models.Cells;
using SharedLogic.Respiration;

namespace SharedLogic.Services
{
    public class RespirationService
    {
        private Cell _cell;

        public RespirationService(Cell cell)
        {
            _cell = cell;
        }

        public void PerformRespiration()
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

            return new BaseRespiration();
        }
    }
}
