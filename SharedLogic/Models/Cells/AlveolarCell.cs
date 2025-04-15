using SharedLogic.Diffusion;

namespace SharedLogic.Models.Cells
{
    public class AlveolarCell : Cell
    {
        public void DiffuseNutrientsFromAir(Air air)
        {
            var diffusionService = new DiffusionService(this);
            diffusionService.DiffuseNutrientsFromAir(air);
        }
    }
}
