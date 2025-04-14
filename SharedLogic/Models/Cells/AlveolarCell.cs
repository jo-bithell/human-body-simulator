using SharedLogic.Services;

namespace SharedLogic.Models.Cells
{
    public class AlveolarCell : Cell
    {
        private readonly DiffusionService _diffusionService;
        public AlveolarCell()
        {
            _diffusionService = new DiffusionService(this);
        }

        public void DiffuseGases(Air incomingAir)
        {
            // Can't simulate diffusion completely due to air having different local concentrations of gases
            DiffuseCarbonDioxide(incomingAir);
            DiffuseOxygen(incomingAir);
        }

        private void DiffuseCarbonDioxide(Air incomingAir)
        {
            while (!_diffusionService.ConcentrationHigherInCell(CarbonDioxideCount, incomingAir.CarbonDioxideCount))
            {
                CarbonDioxideCount += 1;
                incomingAir.CarbonDioxideCount -= 1;
            }

            while (_diffusionService.ConcentrationHigherInCell(CarbonDioxideCount, incomingAir.CarbonDioxideCount))
            {
                CarbonDioxideCount -= 1;
                incomingAir.CarbonDioxideCount += 1;
            }
        }

        private void DiffuseOxygen(Air incomingAir)
        {
            while (!_diffusionService.ConcentrationHigherInCell(OxygenCount, incomingAir.OxygenCount))
            {
                OxygenCount += 1;
                incomingAir.OxygenCount -= 1;
            }

            while (_diffusionService.ConcentrationHigherInCell(OxygenCount, incomingAir.OxygenCount))
            {
                OxygenCount -= 1;
                incomingAir.OxygenCount += 1;
            }
        }
    }
}
