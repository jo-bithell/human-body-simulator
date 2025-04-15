using SharedLogic.Models;
using SharedLogic.Models.Cells;

namespace SharedLogic.Diffusion
{
    internal class DiffusionService
    {
        private Cell _cell;

        internal DiffusionService(Cell cell)
        {
            _cell = cell;
        }

        internal void DiffuseNutrientsFromBlood(Blood incomingBlood)
        {
            Console.WriteLine("Diffusing nutrients");

            // Can't simulate diffusion completely due to blood having different local concentrations of nutrients
            DiffuseCarbonDioxide(incomingBlood);
            DiffuseGlucose(incomingBlood);
            DiffuseOxygen(incomingBlood);
            DiffuseWater(incomingBlood);

            Console.WriteLine("Nutrients diffused");
        }

        internal void DiffuseNutrientsFromAir(Air incomingAir)
        {
            // Can't simulate diffusion completely due to air having different local concentrations of gases
            DiffuseCarbonDioxide(incomingAir);
            DiffuseOxygen(incomingAir);
        }

        private void DiffuseWater(Blood incomingBlood)
        {
            while (!_cell.ConcentrationHigherInCell(_cell.WaterCount, incomingBlood.WaterCount))
            {
                _cell.WaterCount += 1;
                incomingBlood.WaterCount -= 1;
            }

            while (_cell.ConcentrationHigherInCell(_cell.WaterCount, incomingBlood.WaterCount))
            {
                _cell.WaterCount -= 1;
                incomingBlood.WaterCount += 1;
            }
        }

        private void DiffuseGlucose(Blood incomingBlood)
        {
            while (!_cell.ConcentrationHigherInCell(_cell.GlucoseCount, incomingBlood.GlucoseCount))
            {
                _cell.GlucoseCount += 1;
                incomingBlood.GlucoseCount -= 1;
            }

            while (_cell.ConcentrationHigherInCell(_cell.GlucoseCount, incomingBlood.GlucoseCount))
            {
                _cell.GlucoseCount -= 1;
                incomingBlood.GlucoseCount += 1;
            }
        }

        private void DiffuseOxygen(Blood incomingBlood)
        {
            while (!_cell.ConcentrationHigherInCell(_cell.OxygenCount, incomingBlood.OxygenCount))
            {
                _cell.OxygenCount += 1;
                incomingBlood.OxygenCount -= 1;
            }

            while (_cell.ConcentrationHigherInCell(_cell.OxygenCount, incomingBlood.OxygenCount))
            {
                _cell.OxygenCount -= 1;
                incomingBlood.OxygenCount += 1;
            }
        }

        private void DiffuseCarbonDioxide(Blood incomingBlood)
        {
            while (!_cell.ConcentrationHigherInCell(_cell.CarbonDioxideCount, incomingBlood.CarbonDioxideCount))
            {
                _cell.CarbonDioxideCount += 1;
                incomingBlood.CarbonDioxideCount -= 1;
            }

            while (_cell.ConcentrationHigherInCell(_cell.CarbonDioxideCount, incomingBlood.CarbonDioxideCount))
            {
                _cell.CarbonDioxideCount -= 1;
                incomingBlood.CarbonDioxideCount += 1;
            }
        }

        private void DiffuseCarbonDioxide(Air incomingAir)
        {
            while (!_cell.ConcentrationHigherInCell(_cell.CarbonDioxideCount, incomingAir.CarbonDioxideCount))
            {
                _cell.CarbonDioxideCount += 1;
                incomingAir.CarbonDioxideCount -= 1;
            }

            while (_cell.ConcentrationHigherInCell(_cell.CarbonDioxideCount, incomingAir.CarbonDioxideCount))
            {
                _cell.CarbonDioxideCount -= 1;
                incomingAir.CarbonDioxideCount += 1;
            }
        }

        private void DiffuseOxygen(Air incomingAir)
        {
            while (!_cell.ConcentrationHigherInCell(_cell.OxygenCount, incomingAir.OxygenCount))
            {
                _cell.OxygenCount += 1;
                incomingAir.OxygenCount -= 1;
            }

            while (_cell.ConcentrationHigherInCell(_cell.OxygenCount, incomingAir.OxygenCount))
            {
                _cell.OxygenCount -= 1;
                incomingAir.OxygenCount += 1;
            }
        }
    }
}
