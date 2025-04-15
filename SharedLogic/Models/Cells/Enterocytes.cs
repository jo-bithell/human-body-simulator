using SharedLogic.Services;

namespace SharedLogic.Models.Cells
{
    public class Enterocyte : Cell
    {
        public void DiffuseNutrients(int glucoseCount)
        {
            DiffuseGlucose(glucoseCount);
        }

        private void DiffuseGlucose(int glucoseCount)
        {
            while (!ConcentrationHigherInCell(GlucoseCount, glucoseCount))
            {
                GlucoseCount += 1;
                glucoseCount -= 1;
            }       

            while (ConcentrationHigherInCell(GlucoseCount, glucoseCount))
            {
                GlucoseCount -= 1;
                glucoseCount += 1;
            }
        }
    }
}
