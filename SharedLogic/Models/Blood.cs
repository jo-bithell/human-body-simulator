namespace SharedLogic.Models
{
    public class Blood
    {
        private static readonly int _defaultConcentration = 10;
        public int OxygenCount { get; set; } = _defaultConcentration;
        public int GlucoseCount { get; set; } = _defaultConcentration;
        public int AminoAcidCount { get; set; } = _defaultConcentration;
        public int FattyAcidCount { get; set; } = _defaultConcentration;
        public int WaterCount { get; set; } = _defaultConcentration;
        public int CarbonDioxideCount { get; set; } = _defaultConcentration;
    }
}
