using SharedLogic.Respiration;

namespace SharedLogic.Models.Cells
{
    public class Cell
    {
        private static readonly int _defaultConcentration = 10;
        internal int ATPCount { get; set; } = _defaultConcentration;
        internal int OxygenCount { get; set; } = _defaultConcentration;
        internal int GlucoseCount { get; set; } = _defaultConcentration;
        internal int AminoAcidsCount { get; set; } = _defaultConcentration;
        internal int FattyAcidsCount { get; set; } = _defaultConcentration;
        internal int WaterCount { get; set; } = _defaultConcentration;
        internal int CarbonDioxideCount { get; set; } = _defaultConcentration;

        public void Respire()
        {
            var respirationService = new RespirationService(this);
            respirationService.PerformRespiration();
        }

        public int GetDefaultConcentration()
            => _defaultConcentration;

        public bool ConcentrationHigherInCell(int concentrationInsideCell, int concentrationOutsideCell)
            => concentrationInsideCell > _defaultConcentration && concentrationOutsideCell < _defaultConcentration;
    }
}
