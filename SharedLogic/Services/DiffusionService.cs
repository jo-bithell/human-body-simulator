using SharedLogic.Models;
using SharedLogic.Models.Cells;

namespace SharedLogic.Services
{
    internal class DiffusionService
    {
        private Cell _cell;

        internal DiffusionService(Cell cell)
        {
            _cell = cell;
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
            while (!ConcentrationHigherInCell(_cell.WaterCount, incomingBlood.WaterCount))
            {
                _cell.WaterCount += 1;
                incomingBlood.WaterCount -= 1;
            }

            while (ConcentrationHigherInCell(_cell.WaterCount, incomingBlood.WaterCount))
            {
                _cell.WaterCount -= 1;
                incomingBlood.WaterCount += 1;
            }
        }

        private void DiffuseGlucose(Blood incomingBlood)
        {
            while (!ConcentrationHigherInCell(_cell.GlucoseCount, incomingBlood.GlucoseCount))
            {
                _cell.GlucoseCount += 1;
                incomingBlood.GlucoseCount -= 1;
            }

            while (ConcentrationHigherInCell(_cell.GlucoseCount, incomingBlood.GlucoseCount))
            {
                _cell.GlucoseCount -= 1;
                incomingBlood.GlucoseCount += 1;
            }
        }

        private void DiffuseOxygen(Blood incomingBlood)
        {
            while (!ConcentrationHigherInCell(_cell.OxygenCount, incomingBlood.OxygenCount))
            {
                _cell.OxygenCount += 1;
                incomingBlood.OxygenCount -= 1;
            }

            while (ConcentrationHigherInCell(_cell.OxygenCount, incomingBlood.OxygenCount))
            {
                _cell.OxygenCount -= 1;
                incomingBlood.OxygenCount += 1;
            }
        }

        private void DiffuseCarbonDioxide(Blood incomingBlood)
        {
            while (!ConcentrationHigherInCell(_cell.CarbonDioxideCount, incomingBlood.CarbonDioxideCount))
            {
                _cell.CarbonDioxideCount += 1;
                incomingBlood.CarbonDioxideCount -= 1;
            }

            while (ConcentrationHigherInCell(_cell.CarbonDioxideCount, incomingBlood.CarbonDioxideCount))
            {
                _cell.CarbonDioxideCount -= 1;
                incomingBlood.CarbonDioxideCount += 1;
            }
        }
    }
}
