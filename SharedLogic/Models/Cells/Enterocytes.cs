using SharedLogic.Services;

namespace SharedLogic.Models.Cells
{
    public class Enterocyte : Cell
    {
        private readonly DiffusionService _diffusionService;
        public Enterocyte()
        {
            _diffusionService = new DiffusionService(this);
        }

        public void DiffuseNutrients(int glucoseCount)
        {
            DiffuseGlucose(glucoseCount);
        }

        private void DiffuseGlucose(int glucoseCount)
        {
            while (!_diffusionService.ConcentrationHigherInCell(GlucoseCount, glucoseCount))
            {
                GlucoseCount += 1;
                glucoseCount -= 1;
            }       

            while (_diffusionService.ConcentrationHigherInCell(GlucoseCount, glucoseCount))
            {
                GlucoseCount -= 1;
                glucoseCount += 1;
            }
        }
    }
}
