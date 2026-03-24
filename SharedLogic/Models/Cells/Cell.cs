using SharedLogic.Models.Enums;
using SharedLogic.Models.Enzymes;

namespace SharedLogic.Models.Cells
{
    public class Cell
    {
        private static readonly int _defaultConcentration = 10;
        public NutrientConcentrations NutrientConcentrations { get; set; } = new NutrientConcentrations(_defaultConcentration);
        public List<Enzyme> Enzymes { get; set; } = new List<Enzyme>
        {
            new Enzyme{ EnzymeType = EnzymeType.ATPSynthase },
            new Enzyme{ EnzymeType = EnzymeType.Lipase },
        };

        public bool ConcentrationHigherInCell(int concentrationInsideCell, int concentrationOutsideCell)
            => concentrationInsideCell > _defaultConcentration && concentrationOutsideCell < _defaultConcentration;

        public Enzyme? GetAvailableEnzyme(EnzymeType enzymeType)
            => Enzymes.Where(o => !o.InUse)?.FirstOrDefault(o => o.EnzymeType == enzymeType);
    }


    public class NutrientConcentrations(int defaultConcentration)
    {
        public int ATPCount { get; set; } = defaultConcentration;
        public int OxygenCount { get; set; } = defaultConcentration;
        public int GlucoseCount { get; set; } = defaultConcentration;
        public int AminoAcidsCount { get; set; } = defaultConcentration;
        public int FattyAcidsCount { get; set; } = defaultConcentration;
        public int WaterCount { get; set; } = defaultConcentration;
        public int CarbonDioxideCount { get; set; } = defaultConcentration;
    }
}
