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
            var diffusionService = new DiffusionService(this);
            diffusionService.DiffuseNutrients(incomingBlood);
        }
    }
}
