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

    public class NutrientConcentrations
    {
        public NutrientConcentrations() { }

        public NutrientConcentrations(int defaultConcentration)
        {
            ATPCount = defaultConcentration;
            OxygenCount = defaultConcentration;
            GlucoseCount = defaultConcentration;
            AminoAcidsCount = defaultConcentration;
            FattyAcidsCount = defaultConcentration;
            WaterCount = defaultConcentration;
            CarbonDioxideCount = defaultConcentration;
        }

        public int ATPCount { get; set; }
        public int OxygenCount { get; set; }
        public int GlucoseCount { get; set; }
        public int AminoAcidsCount { get; set; }
        public int FattyAcidsCount { get; set; }
        public int WaterCount { get; set; }
        public int CarbonDioxideCount { get; set; }
    }
}
