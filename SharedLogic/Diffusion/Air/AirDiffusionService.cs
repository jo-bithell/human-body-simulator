using SharedLogic.Models.Cells;
using SharedLogic.Models;

namespace SharedLogic.Diffusion
{
    public class AirDiffusionService : IDiffusionService
    {
        private Cell _cell;
        private Air _air;
        public AirDiffusionService(Cell cell, Air air)
        {
            _cell = cell;
            _air = air;
        }

        public void Diffuse()
        {
            // Can't simulate diffusion completely due to air having different local concentrations of gases
            DiffuseCarbonDioxide();
            DiffuseOxygen();
        }

        private void DiffuseCarbonDioxide()
        {
            while (!_cell.ConcentrationHigherInCell(_cell.CarbonDioxideCount, _air.CarbonDioxideCount))
            {
                _cell.CarbonDioxideCount += 1;
                _air.CarbonDioxideCount -= 1;
            }

            while (_cell.ConcentrationHigherInCell(_cell.CarbonDioxideCount, _air.CarbonDioxideCount))
            {
                _cell.CarbonDioxideCount -= 1;
                _air.CarbonDioxideCount += 1;
            }
        }

        private void DiffuseOxygen()
        {
            while (!_cell.ConcentrationHigherInCell(_cell.OxygenCount, _air.OxygenCount))
            {
                _cell.OxygenCount += 1;
                _air.OxygenCount -= 1;
            }

            while (_cell.ConcentrationHigherInCell(_cell.OxygenCount, _air.OxygenCount))
            {
                _cell.OxygenCount -= 1;
                _air.OxygenCount += 1;
            }
        }
    }
}
