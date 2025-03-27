using CsvCommon;
using SharedLogic.Models.Cells;

namespace Mouth
{
    public class MechanicalDigestionService : BaseCsvService
    {
        private readonly int _atpThreshold = 5;
        private readonly int _chunkSize;
        private readonly List<Myocyte> _myocytes;
        public MechanicalDigestionService(List<Myocyte> mouthCells, int chunkSize)
        {
            _myocytes = mouthCells;
            _chunkSize = chunkSize;
        }

        public override void DigestFood(List<string[]> records, string outputDirectory)
        {
            PerformRespiration();
            int fileIndex = 0;

            for (int i = 0; i < records.Count; i += _chunkSize)
            {
                var chunk = records.GetRange(i, Math.Min(_chunkSize, records.Count - i));
                string outputFilePath = Path.Combine(outputDirectory, $"chunk_{fileIndex}.csv");
                WriteCsvFile(outputFilePath, chunk);
                fileIndex++;
            }
        }

        protected override void PerformRespiration()
        {
            foreach (Myocyte mouthCell in _myocytes)
            {
                mouthCell.PerformMotion(_atpThreshold);
            }
        }
    }
}
