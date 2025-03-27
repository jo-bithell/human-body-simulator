namespace SharedLogic.Models
{
    public class Air
    {
        private static readonly int _defaultConcentration = 10;
        public int OxygenCount { get; set; } = _defaultConcentration;
        public int CarbonDioxideCount { get; set; } = _defaultConcentration;
    }
}