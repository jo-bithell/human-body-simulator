namespace SharedLogic.Models.Cells
{
    public class Myocyte : Cell
    {
        public void PerformMotion(int atpThreshold)
        {
            while (ATPCount < atpThreshold)
            {
                Console.WriteLine("ATP count insufficient, performing respiration.");
                Respire();
            }

            ATPCount -= atpThreshold;
            Console.WriteLine($"Motion performed, {atpThreshold} ATP consumed");
        }
    }
}
