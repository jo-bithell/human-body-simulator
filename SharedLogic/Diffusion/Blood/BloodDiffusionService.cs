using SharedLogic.Models;
using SharedLogic.Models.Cells;

namespace SharedLogic.Diffusion
{
    public class BloodDiffusionService : IDiffusionService
    {
        private Cell _cell;
        private Blood _blood;
        public BloodDiffusionService(Cell cell, Blood blood)
        {
            _cell = cell;
            _blood = blood;
        }

        public void Diffuse()
        {
            Console.WriteLine("Diffusing nutrients");

            // Can't simulate diffusion completely due to blood having different local concentrations of nutrients
            DiffuseCarbonDioxide();
            DiffuseGlucose();
            DiffuseOxygen();
            DiffuseWater();

            Console.WriteLine("Nutrients diffused");
        }

        private void DiffuseWater()
        {
            while (!_cell.ConcentrationHigherInCell(_cell.WaterCount, _blood.WaterCount))
            {
                _cell.WaterCount += 1;
                _blood.WaterCount -= 1;
            }

            while (_cell.ConcentrationHigherInCell(_cell.WaterCount, _blood.WaterCount))
            {
                _cell.WaterCount -= 1;
                _blood.WaterCount += 1;
            }
        }

        private void DiffuseGlucose()
        {
            while (!_cell.ConcentrationHigherInCell(_cell.GlucoseCount, _blood.GlucoseCount))
            {
                _cell.GlucoseCount += 1;
                _blood.GlucoseCount -= 1;
            }

            while (_cell.ConcentrationHigherInCell(_cell.GlucoseCount, _blood.GlucoseCount))
            {
                _cell.GlucoseCount -= 1;
                _blood.GlucoseCount += 1;
            }
        }

        private void DiffuseOxygen()
        {
            while (!_cell.ConcentrationHigherInCell(_cell.OxygenCount, _blood.OxygenCount))
            {
                _cell.OxygenCount += 1;
                _blood.OxygenCount -= 1;
            }

            while (_cell.ConcentrationHigherInCell(_cell.OxygenCount, _blood.OxygenCount))
            {
                _cell.OxygenCount -= 1;
                _blood.OxygenCount += 1;
            }
        }

        private void DiffuseCarbonDioxide()
        {
            while (!_cell.ConcentrationHigherInCell(_cell.CarbonDioxideCount, _blood.CarbonDioxideCount))
            {
                _cell.CarbonDioxideCount += 1;
                _blood.CarbonDioxideCount -= 1;
            }

            while (_cell.ConcentrationHigherInCell(_cell.CarbonDioxideCount, _blood.CarbonDioxideCount))
            {
                _cell.CarbonDioxideCount -= 1;
                _blood.CarbonDioxideCount += 1;
            }
        }
    }
}
