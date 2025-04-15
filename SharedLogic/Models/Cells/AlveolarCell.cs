using SharedLogic.Diffusion;

namespace SharedLogic.Models.Cells
{
    public class AlveolarCell : Cell
    {
        public void DiffuseNutrientsFromAir(Air air)
        {
            var diffusionService = new IDiffusionService(this);
            diffusionService.DiffuseNutrientsFromAir(air);
        }
    }
}
