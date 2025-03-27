namespace CsvCommon
{
    public abstract class BaseCsvService : IBaseCsvService
    {
        public List<string[]> ReadCsvFile(string filePath)
        {
            var records = new List<string[]>();

            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line?.Split(',');
                    if (values != null)
                    {
                        records.Add(values);
                    }
                }
            }

            return records;
        }

        public virtual void DigestFood(List<string[]> records, string outputDirectory)
        {
            var chunkSize = 10;
            int fileIndex = 0;

            for (int i = 0; i < records.Count; i += chunkSize)
            {
                var chunk = records.GetRange(i, Math.Min(chunkSize, records.Count - i));
                string outputFilePath = Path.Combine(outputDirectory, $"chunk_{fileIndex}.csv");
                WriteCsvFile(outputFilePath, chunk);
                fileIndex++;
            }
        }

        public void WriteCsvFile(string filePath, List<string[]> records)
        {
            using (var writer = new StreamWriter(filePath))
            {
                foreach (var record in records)
                {
                    writer.WriteLine(string.Join(",", record));
                }
            }
        }

        protected abstract void PerformRespiration();
    }
}
