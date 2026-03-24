using SharedLogic.Models.Cells;
using SharedLogic.Models.Enums;
using SharedLogic.Models.Enzymes;

namespace SmallIntestine.Models
{
    public class Enterocyte : Cell
    {
        public new List<Enzyme> Enzymes { get; set; } = GetEnterocyteEnzymes();

        private static List<Enzyme> GetEnterocyteEnzymes()
            => new List<Enzyme>
            {
                new Enzyme{ EnzymeType = EnzymeType.ATPSynthase },
                new Enzyme{ EnzymeType = EnzymeType.Lipase },
                new Enzyme{ EnzymeType = EnzymeType.Trypsin },
            };
    }
}
