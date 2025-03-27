namespace CsvCommon
{
    public interface IBaseCsvService
    {
        List<string[]> ReadCsvFile(string filePath);
        void DigestFood(List<string[]> records, string outputDirectory);
    }
}