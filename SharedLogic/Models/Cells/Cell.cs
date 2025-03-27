using SharedLogic.Services;

namespace SharedLogic.Models.Cells
{
    public class Cell
    {
        private static readonly int _defaultConcentration = 10;
        public int ATPCount { get; set; } = _defaultConcentration;
        public int OxygenCount { get; set; } = _defaultConcentration;
        public int GlucoseCount { get; set; } = _defaultConcentration;
        public int AminoAcidsCount { get; set; } = _defaultConcentration;
        public int FattyAcidsCount { get; set; } = _defaultConcentration;
        public int WaterCount { get; set; } = _defaultConcentration;
        public int CarbonDioxideCount { get; set; } = _defaultConcentration;

        public void Respire()
        {
            var respirationService = new RespirationService(this);
            respirationService.PerformRespiration();
        }

        public void DiffuseNutrients(Blood incomingBlood)
        {
            Console.WriteLine("Diffusing nutrients");

            // Can't simulate diffusion completely due to blood having different local concentrations of nutrients
            DiffuseCarbonDioxide(incomingBlood);
            DiffuseGlucose(incomingBlood);
            DiffuseOxygen(incomingBlood);
            DiffuseWater(incomingBlood);

            Console.WriteLine("Nutrients diffused");
        }

        public bool ConcentrationHigherInCell(int concentrationInsideCell, int concentrationOutsideCell)
            => concentrationInsideCell > 10 && concentrationOutsideCell < 10;

        private void DiffuseWater(Blood incomingBlood)
        {
            while (!ConcentrationHigherInCell(WaterCount, incomingBlood.WaterCount))
            {
                WaterCount += 1;
                incomingBlood.WaterCount -= 1;
            }

            while (ConcentrationHigherInCell(WaterCount, incomingBlood.WaterCount))
            {
                WaterCount -= 1;
                incomingBlood.WaterCount += 1;
            }
        }

        private void DiffuseGlucose(Blood incomingBlood)
        {
            while (!ConcentrationHigherInCell(GlucoseCount, incomingBlood.GlucoseCount))
            {
                GlucoseCount += 1;
                incomingBlood.GlucoseCount -= 1;
            }

            while (ConcentrationHigherInCell(GlucoseCount, incomingBlood.GlucoseCount))
            {
                GlucoseCount -= 1;
                incomingBlood.GlucoseCount += 1;
            }
        }

        private void DiffuseOxygen(Blood incomingBlood)
        {
            while (!ConcentrationHigherInCell(OxygenCount, incomingBlood.OxygenCount))
            {
                OxygenCount += 1;
                incomingBlood.OxygenCount -= 1;
            }

            while (ConcentrationHigherInCell(OxygenCount, incomingBlood.OxygenCount))
            {
                OxygenCount -= 1;
                incomingBlood.OxygenCount += 1;
            }
        }

        private void DiffuseCarbonDioxide(Blood incomingBlood)
        {
            while (!ConcentrationHigherInCell(CarbonDioxideCount, incomingBlood.CarbonDioxideCount))
            {
                CarbonDioxideCount += 1;
                incomingBlood.CarbonDioxideCount -= 1;
            }

            while (ConcentrationHigherInCell(CarbonDioxideCount, incomingBlood.CarbonDioxideCount))
            {
                CarbonDioxideCount -= 1;
                incomingBlood.CarbonDioxideCount += 1;
            }
        }
    }
}
