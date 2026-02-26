using SharedLogic.Models.Enums;

namespace SharedLogic.Models.Enzymes
{
    public class Enzyme
    {
        public EnzymeType EnzymeType { get; set; }
        public int ShapeIntactness { get; set; } = 100;
        public bool InUse { get; set; }

        public void PerformAction()
        {
            ShapeIntactness -= 1;
        }
    }
}